using Godot;
using System;
using PathfindingModule;
public partial class Robot : RigidBody2D
{
	[Export]
	public int move_speed = 110;

	private int _unitSize = 16;
    private Vector2i MapSize = new Vector2i(32, 32);

    public NodePath AstarTilesPath { get; set; }
	public NodePath PlayerPath { get; set; }
    public Player Player { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		AstarTilesPath = new NodePath("TileMap/AstarTileMap");
		PlayerPath = new NodePath("Player");
		Player = (Player)this.GetParent().GetNode(PlayerPath);
        Tiles = (TileMap)this.GetNode(AstarTilesPath);
        AStar = AStarSetup.CreateInstance(32,32,_unitSize);
		GetNewAstarPath();
	}

    public AStarSetup AStar { get; set; }

    public TileMap Tiles { get; set; }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GetNewAstarPath()
	{
		var node = (AstarTileMap)this.GetParent().GetNode(AstarTilesPath);
		var nodes = node.GetAstarPath(this.GlobalPosition, Player.GlobalPosition);
		var x = 1;
	}
}
