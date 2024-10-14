using Godot;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Schema;


public partial class ImagePuzzleManager : Node
{
	//Internal co-ordinate system is (y, x) because the arrays are arrays of rows. With 0,0 being in the top left corner.
	[ExportGroup("Settings")]
	[Export] public bool isActive = true;
	
	[ExportGroup("Material Settings")]
	[Export] public Material unrecognizedMaterial;
	[Export] public Godot.Collections.Array<Material> possibleMaterials { get; set; }
	
	[ExportGroup("References")]
	[Export] public Node3D socketParent;
	[Export] public Node3D clueImageParent; //The parent node of the rows
	[Export] public Node3D createdImageParent; //The parent node of the rows
	[Export] public Node3D colourSpawnerParent;

	[Signal] public delegate void CompletedEventHandler();
	
	//Other variables
	private Godot.Collections.Array<Godot.Collections.Array<PlacementMonitor>> socketArray;
	private Godot.Collections.Array<Godot.Collections.Array<CsgBox3D>> clueImageArray;
	private Godot.Collections.Array<Godot.Collections.Array<CsgBox3D>> createdImageArray;
	private Godot.Collections.Array<PlacementMonitor> colourBoxSpawners;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get all the sockets
		var socketRows = socketParent.GetChildren();
		socketArray = new Godot.Collections.Array<Godot.Collections.Array<PlacementMonitor>>();
		for(int i = 0; i < socketRows.Count; i++)
		{
			var mappedSockets = socketRows[i].GetChildren()
			.Where(child => child is PlacementMonitor)
        	.Cast<PlacementMonitor>();

			socketArray.Add(new Godot.Collections.Array<PlacementMonitor>());

			foreach (var socket in mappedSockets)
			{
				socketArray[i].Add(socket);
				socket.ItemPlacedRaw += ItemPlacedInSocket; //Subscribe to the socket
				socket.ItemRemovedRaw += ItemRemovedFromSocket;
			}
		}
		
		//Get all the clue Image boxes
		var clueImageRows = clueImageParent.GetChildren();
		clueImageArray = new Godot.Collections.Array<Godot.Collections.Array<CsgBox3D>>();
		for(int i = 0; i < clueImageRows.Count; i++)
		{
			var mappedBoxes = clueImageRows[i].GetChildren()
			.Where(child => child is CsgBox3D)
        	.Cast<CsgBox3D>();

			clueImageArray.Add(new Godot.Collections.Array<CsgBox3D>());

			foreach (var box in mappedBoxes)
			{
				clueImageArray[i].Add(box);
				if(!possibleMaterials.Contains(box.Material)) possibleMaterials.Add(box.Material);
			}
		}

		//Get all the created Image boxes
		var createdImageRows = createdImageParent.GetChildren();
		createdImageArray = new Godot.Collections.Array<Godot.Collections.Array<CsgBox3D>>();
		for(int i = 0; i < createdImageRows.Count; i++)
		{
			var mappedBoxes = createdImageRows[i].GetChildren()
			.Where(child => child is CsgBox3D)
        	.Cast<CsgBox3D>();

			createdImageArray.Add(new Godot.Collections.Array<CsgBox3D>());

			foreach (var box in mappedBoxes)
			{
				createdImageArray[i].Add(box);
				box.Material = unrecognizedMaterial;
			}
		}
		
		//Setup material resources 
		var mappedSpawners = colourSpawnerParent.GetChildren()
		.Where(child => child is PlacementMonitor)
        .Cast<PlacementMonitor>();

		colourBoxSpawners = new Godot.Collections.Array<PlacementMonitor>();
		foreach (var spawner in mappedSpawners) colourBoxSpawners.Add(spawner);

		for(int i = 0; i < colourBoxSpawners.Count; i++)
		{
			var mat = possibleMaterials[i%possibleMaterials.Count];
			//colourBoxSpawners[i].spawnItem();
			var item = new CsgBox3D
        	{
            	Material = mat
        	};
			colourBoxSpawners[i].attachItem(item);
		}

		for(int i = 0; i <= 20; i += 11)
		{
			var matInFrame = possibleMaterials[i%possibleMaterials.Count];
			Node3D itemInFrame = new CsgBox3D {Material = matInFrame};
			socketArray[i%socketArray.Count][i%socketArray[i%socketArray.Count].Count].attachItem(itemInFrame);
		}
		
		/*
		GD.Print("Sockets:");
		GD.Print(socketArray);
		GD.Print("ClueImage:");
		GD.Print(clueImageArray);
		GD.Print("CreatedImage:");
		GD.Print(createdImageArray);
		*/
	}

	private void ItemPlacedInSocket(Node3D itemNode, PlacementMonitor monitor)
    {
		(int,int) socketYX = FindSocketYX(monitor);

		//TODO ----- Change to work with models rather than just Cgsboxes
		Material itemMaterial = ((CsgBox3D)itemNode).Material;
		
		if(possibleMaterials.Contains(itemMaterial))
		{
			createdImageArray[socketYX.Item1][socketYX.Item2].Material = itemMaterial;
			CheckCompletion();
		}
		else createdImageArray[socketYX.Item1][socketYX.Item2].Material = unrecognizedMaterial;
	}

	private void ItemRemovedFromSocket(PlacementMonitor monitor)
	{
		(int,int) socketYX = FindSocketYX(monitor);
		createdImageArray[socketYX.Item1][socketYX.Item2].Material = unrecognizedMaterial;
	}

	private void CheckCompletion()
	{
		if(isIdentical()) EmitSignal(SignalName.Completed);
	}

	private (int y, int x) FindSocketYX(PlacementMonitor monitor) //(-1,-1) if the socket does not exist in the array
	{
		for(int y = 0; y < socketArray.Count; y++)
		{
			for(int x = 0; x < socketArray[y].Count; x++)
			{
				if(socketArray[y][x] == monitor) return (y, x);
			}
		}
		GD.PushError(GetPath()," ERROR: Socket Not Found");
		return (-1, -1);
	}

	private bool isIdentical() 
	{
		for(int y = 0; y < clueImageArray.Count; y++)
		{
			for(int x = 0; x < clueImageArray[y].Count; x++)
			{
				GD.Print("Checking Location: ", x,",",y);
				GD.Print("Clue Material: ", clueImageArray[y][x].Material);
				GD.Print("Created Material: ", createdImageArray[y][x].Material);
				if(clueImageArray[y][x].Material != createdImageArray[y][x].Material) return false;
				GD.Print("Material is the Same");
				GD.Print("");
			}
		}
		return true;
	}

	public void setActive(bool active)
	{
		for(int y = 0; y < socketArray.Count; y++)
		{
			for(int x = 0; x < socketArray[y].Count; x++)
			{
				socketArray[y][x].setSocketActive(active);
			}
		}

	}
}
