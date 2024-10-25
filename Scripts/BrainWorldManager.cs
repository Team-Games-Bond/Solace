using Godot;
using System;

public partial class BrainWorldManager : Node
{
	[ExportGroup("Object References")]
	[Export] public ExitDoors exitDoors;

	[ExportSubgroup("Fog & Neuron References")]
	[Export] public FogManager FogManager;
	[Export] public Neuron powerSocketDias;
	[Export] public Neuron powerSocketMaze;
	[Export] public Neuron powerSocketMiddle;
	[Export] public Neuron powerSocketBridge;
	[Export] public Neuron powerSocketTree;



	[ExportGroup("Listeners")]
	[Export] public ImagePuzzleManager imagePuzzle;
	[Export] public SimonSaysManager simonSays;
	[Export] public ClutterRoomManager clutterRoom;
	[Export] public ExitDiasManager exitDias;

	//Other variables
	bool imagePuzzleDone = false;
	bool simonSaysDone = false;
	bool clutterRoomDone = false;
	bool exitDiasDone = false;

	[Signal] public delegate void StartSignalEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		imagePuzzle.Completed += imagePuzzleCompleted;
		simonSays.Completed += simonSaysCompleted;
		clutterRoom.Completed += clutterRoomCompleted;
		exitDias.Completed += exitDiasCompleted;

		exitDias.CrystalsPlaced += crystalsPlaced;

		powerSocketDias.NeuronActivatedWithReference += neuronActivte;
		powerSocketMaze.NeuronActivatedWithReference += neuronActivte;
		powerSocketMiddle.NeuronActivatedWithReference += neuronActivte;
		powerSocketBridge.NeuronActivatedWithReference += neuronActivte;
		powerSocketTree.NeuronActivatedWithReference += neuronActivte;

		EmitSignal(SignalName.StartSignal);
	}

	private void neuronActivte(Neuron sendingNeuron)
	{
		if(sendingNeuron == powerSocketDias) FogManager.RemoveFog(FogLocation.maze1);
		else if(sendingNeuron == powerSocketMaze) FogManager.RemoveFog(FogLocation.maze2);
		else if(sendingNeuron == powerSocketMiddle) FogManager.RemoveFog(FogLocation.clutterAndSimon);
		else if(sendingNeuron == powerSocketBridge) FogManager.RemoveFog(FogLocation.bridge);
		else if(sendingNeuron == powerSocketTree) FogManager.RemoveFog(FogLocation.tree);
		else GD.PushError("Unhandled Neuron: " + sendingNeuron.Name);
	}


	private void imagePuzzleCompleted()
	{
		GD.Print("Image Puzzle Completed");
		//imagePuzzleFlag.Open();
		//imagePuzzleCover.Open();
		imagePuzzleDone = true;

		imagePuzzle.setActive(false);
	}

	private void simonSaysCompleted()
	{
		GD.Print("Simon Says Completed");
		//simonSaysFlag.Open();
		//simonSaysCover.Open();
		simonSaysDone = true;
		
		simonSays.setActive(false);
	}

	private void clutterRoomCompleted()
	{
		GD.Print("Clutter Room Completed");
		//clutterRoomFlag.Open();
		//clutterRoomCover.QueueFree();
		clutterRoomDone = true;
	}

	private void exitDiasCompleted()
	{
		//TODO end game
		GD.PrintRich("[shake rate=20.0 level=5 connected=1][rainbow]Game Completed![/rainbow][/shake]");
	}

	private void crystalsPlaced()
	{
		GD.Print("All Crystals Placed");
		exitDoors.Open();
	}
}
