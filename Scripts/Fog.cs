using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

[Tool]
public partial class Fog : MeshInstance3D
{
	[ExportCategory("Fog Offset")]
	[Export] public Texture2D InitialTexture;
	[Export] public Sprite3D sprite;
	[Export] public Timer TransitionTimer;
	public int map_width, map_height;

	private byte[] _primaryImage;
	private Task _combineTask;

	[ExportCategory("Fog Mesh")]
	private Vector2I _subdivisions = new Vector2I(3, 3);
	private Vector2 _planeScale = new Vector2(1, 1);
	[Export]
	public Vector2I Subdivisions
	{
		get => _subdivisions;
		set
		{
			_subdivisions = new Vector2I(Math.Max(value.X, 1), Math.Max(value.Y, 1));
			Refresh();
		}
	}
	[Export]
	public Vector2 PlaneScale
	{
		get => _planeScale;
		set
		{
			_planeScale = new Vector2(Math.Max(value.X, 0f), Math.Max(value.Y, 0f));
			Refresh();
		}
	}
	private ShaderMaterial _fogMaterial = GD.Load<ShaderMaterial>("res://Materials/FogMaterial.tres");
	[Export]
	public ShaderMaterial FogMaterial
	{
		get => _fogMaterial;
		set
		{
			_fogMaterial = value;
			Refresh();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Engine.IsEditorHint())
		{
			var image = InitialTexture.GetImage();
			map_width = image.GetWidth();
			map_height = image.GetHeight();
			_fogMaterial.SetShaderParameter("PRIMARY_DEPTH", InitialTexture);
			image.Convert(Image.Format.R8);
			_primaryImage = image.GetData();
			TransitionTimer.Timeout += endTransition;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!TransitionTimer.IsStopped())
		{
			SetInstanceShaderParameter("SECONDARY_STRENGTH", 1 - (TransitionTimer.TimeLeft / TransitionTimer.WaitTime));
			//GD.Print(TransitionTimer.TimeLeft/TransitionTimer.WaitTime);
		}
		//GD.Print(TransitionTimer.TimeLeft/TransitionTimer.WaitTime);
	}

	public async void Recede(Texture2D image)
	{
		//GD.Print("Receding");
		if (!TransitionTimer.IsStopped())
		{
			endTransition();
		}
		TransitionTimer.Start();
		_fogMaterial.SetShaderParameter("SECONDARY_DEPTH", image);
		//GD.Print(_primaryImage.Length);
		_combineTask = combine(_primaryImage, image, sprite);
	}

	public async void endTransition()
	{
		GD.Print("End");
		await _combineTask;
		//GD.Print(_primaryImage.Length);
		//GD.Print("combineTask");
		TransitionTimer.Stop();
		//new Task(()=>{
		//GD.Print("Newtaskstart");
		Texture2D img = ImageTexture.CreateFromImage(Image.CreateFromData(map_width, map_height, false, Image.Format.R8, _primaryImage[0..65536]));
		//_primaryImage = img.GetData();
		_fogMaterial.SetShaderParameter("SECONDARY_STRENGTH", 0);
		//SetInstanceShaderParameter("PRIMARY_DEPTH", img);
		_fogMaterial.SetShaderParameter("PRIMARY_DEPTH", img);
		//});
	}

	public static async Task combine(byte[] target, Texture2D from, Sprite3D sprite)
	{
		//GD.Print("combine running");
		Stopwatch sw = new Stopwatch(); //Not used
		sw.Start();
		var fromTask = new Task<ushort[]>(() =>
		{
			var image = from.GetImage();
			image.Convert(Image.Format.R8);
			var fromData = image.GetData();
			return fromData.Select(r => (ushort)r).ToArray();
		});
		fromTask.Start();
		var targetTask = new Task<ushort[]>(() => target.Select(r => (ushort)r).ToArray());
		targetTask.Start();

		int length = target.Length;
		int remaining = length % Vector<ushort>.Count;

		ushort[] maxValues = new ushort[Vector<ushort>.Count];
		for (int j = 0; j < Vector<ushort>.Count; j++)
		{
			maxValues[j] = 255;
		}
		Vector<ushort> maxVector = new Vector<ushort>(maxValues, 0);
		await targetTask;
		await fromTask;
		for (int i = 0; i < length - remaining; i += Vector<ushort>.Count)
		{
			var targetVector = new Vector<ushort>(targetTask.Result, i);
			var fromVector = new Vector<ushort>(fromTask.Result, i);
			ushort[] s = new ushort[Vector<ushort>.Count];
			((targetVector * fromVector) / maxVector).CopyTo(s, 0);
			for (int j = 0; j < Vector<ushort>.Count; j++)
			{
				target[i + j] = (byte)s[j];
			}
		}
		//GD.Print("fromtask: ", fromTask.Result.Length);
		//GD.Print("target: ", length);
		for (int i = length - remaining; i < length; i++)
		{
			target[i] = (byte)((target[i] * fromTask.Result[i]) / 255);
		}
		var elapsed = sw.ElapsedMilliseconds;


		//GD.Print("combine done");
		//var img = ImageTexture.CreateFromImage(Image.CreateFromData(256, 256, false, Image.Format.R8, target[0..65536]));
		//sprite.Texture = img;
	}

	public void Refresh()
	{
		if (Engine.IsEditorHint()) MeshGen();
	}

	public async void MeshGen()
	{
		var verticesTask = new Task<Vector3[]>(() =>
		{
			var vertices = new Vector3[(Subdivisions.X + 2) * (Subdivisions.Y + 2)];
			for (int y = 0; y < Subdivisions.Y + 2; y++)
			{
				for (int x = 0; x < Subdivisions.X + 2; x++)
				{
					int i = y * (Subdivisions.X + 2) + x;
					vertices[i] = new Vector3(((x + ((y % 2 == 1) ? 0.25f : -0.25f)) / (Subdivisions.X + 1) - 0.5f) * PlaneScale.X, 0, (((float)y) / ((float)(Subdivisions.Y + 1)) - 0.5f) * PlaneScale.Y);
				}
			}
			return vertices;
		});
		verticesTask.Start();

		var uvTask = new Task<Vector2[]>(() =>
		{
			var uv = new Vector2[(Subdivisions.X + 2) * (Subdivisions.Y + 2)];
			for (int y = 0; y < Subdivisions.Y + 2; y++)
			{
				for (int x = 0; x < Subdivisions.X + 2; x++)
				{
					int i = y * (Subdivisions.X + 2) + x;
					uv[i] = new Vector2((float)x / ((float)Subdivisions.X + (float)1), (float)y / ((float)Subdivisions.Y + (float)1));
				}
			}
			return uv;
		});
		uvTask.Start();
		var indicesTask = new Task<int[]>(() =>
		{
			var indices = new int[(Subdivisions.X + 1) * (Subdivisions.Y + 1) * 2 * 3];
			for (int y = 0; y < Subdivisions.Y + 1; y++)
			{
				for (int x = 0; x < Subdivisions.X + 1; x++)
				{
					int i = (y * (Subdivisions.X + 1) + x) * 6;
					if (y % 2 == 0)
					{
						indices[i] = y * (Subdivisions.X + 2) + x;
						indices[i + 1] = y * (Subdivisions.X + 2) + x + 1;
						indices[i + 2] = (y + 1) * (Subdivisions.X + 2) + x;
						indices[i + 3] = (y + 1) * (Subdivisions.X + 2) + x;
						indices[i + 4] = y * (Subdivisions.X + 2) + x + 1;
						indices[i + 5] = (y + 1) * (Subdivisions.X + 2) + x + 1;
					}
					else
					{
						indices[i] = y * (Subdivisions.X + 2) + x;
						indices[i + 1] = (y + 1) * (Subdivisions.X + 2) + x + 1;
						indices[i + 2] = (y + 1) * (Subdivisions.X + 2) + x;
						indices[i + 3] = y * (Subdivisions.X + 2) + x;
						indices[i + 4] = y * (Subdivisions.X + 2) + x + 1;
						indices[i + 5] = (y + 1) * (Subdivisions.X + 2) + x + 1;
					}
				}
			}
			return indices;
		});
		indicesTask.Start();
		var arrayMesh = new ArrayMesh();
		arrayMesh.CustomAabb=new Aabb(0,0,0,3*_planeScale.X,5,3*_planeScale.Y);
		Mesh = arrayMesh;
		var arrays = new Godot.Collections.Array();
		arrays.Resize((int)Mesh.ArrayType.Max);
		await verticesTask;
		await indicesTask;
		await uvTask;
		arrays[(int)Mesh.ArrayType.Vertex] = verticesTask.Result;
		arrays[(int)Mesh.ArrayType.Index] = indicesTask.Result;
		arrays[(int)Mesh.ArrayType.TexUV] = uvTask.Result;
		arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
		arrayMesh.SurfaceSetMaterial(0, FogMaterial);
	}
}
