using Godot;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

public partial class Fog : MeshInstance3D
{
	[Export] public Texture2D PrimaryTexture;
	[Export] public Texture2D SecondaryTexture;
	[Export]public Timer TransitionTimer;
	public ShaderMaterial FogMat;
	public int map_width, map_height;

	private byte[] _primaryImage;
	private Task _combineTask; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		FogMat = (ShaderMaterial)GetActiveMaterial(0);
		var image = PrimaryTexture.GetImage();
		map_width = image.GetWidth();
		map_height = image.GetHeight();
		FogMat.SetShaderParameter("PRIMARY_DEPTH", PrimaryTexture);
		//FogMat.SetShaderParameter("SECONDARY_DEPTH", SecondaryTexture);
		image.Convert(Image.Format.R8);
		_primaryImage = image.GetData();
		TransitionTimer.Timeout+=endTransition;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!TransitionTimer.IsStopped())
		{
			SetInstanceShaderParameter("SECONDARY_STRENGTH", 1-(TransitionTimer.TimeLeft/TransitionTimer.WaitTime));
		}
	}
	public async void Recede(Texture2D image)
	{
		if(!TransitionTimer.IsStopped()){
			endTransition();
		}
		TransitionTimer.Start();
		FogMat.SetShaderParameter("SECONDARY_DEPTH", image);
		_combineTask = combine(_primaryImage, image);
	}
	public async void endTransition(){
		await _combineTask;
		TransitionTimer.Stop();
		new Task(()=>{
			var img = ImageTexture.CreateFromImage(Image.CreateFromData(map_width, map_height, false, Image.Format.R8, _primaryImage));
			SetInstanceShaderParameter("HEIGHT_TEXTURE_STRENGTH", 0);
		});
	}
	
	public static async Task combine(byte[] target, Texture2D from)
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();
		var fromTask = new Task<ushort[]>(()=> {
			var fromData = from.GetImage().GetData();
			return fromData.Select(r=>(ushort)r).ToArray();
			} );
		fromTask.Start();
		var targetTask = new Task<ushort[]>(()=> target.Select(r=>(ushort)r).ToArray());
		targetTask.Start();
		
		int length = target.Length;
		int remaining = length % Vector<ushort>.Count;

		ushort[] maxValues = new ushort[Vector<ushort>.Count];
		for (int j = 0; j<Vector<ushort>.Count; j++)
		{
			maxValues[j]=255;
		}
		Vector<ushort> maxVector = new Vector<ushort>(maxValues, 0); 
		await targetTask;		
		await fromTask;
		for (int i = 0; i<length-remaining; i += Vector<ushort>.Count)
		{
			var targetVector = new Vector<ushort>(targetTask.Result, i);
			var fromVector = new Vector<ushort>(fromTask.Result, i);
			ushort[] s = new ushort[Vector<ushort>.Count];
			((targetVector*fromVector)/maxVector).CopyTo(s, 0);
			for (int j = 0; j<Vector<ushort>.Count; j++)
			{
				target[i+j]=(byte)s[j];
			}
		}
		for (int i=length-remaining; i<length; i++){
			target[i] = (byte)((target[i]*fromTask.Result[i])/255);
		}
		var elapsed = sw.ElapsedMilliseconds;
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Keycode == Key.M)
				Recede(SecondaryTexture);
	}
}
