using Godot;
using System;

public partial class Robot : CharacterBody2D
{
	public const float Speed = 300.0f;
    public TileMap PathGrid { get; set; }
    public override void _Ready()
    {
        PathGrid = (TileMap) this.GetParent().GetNode("TileMap/AstarTileMap");
    }

    public override void _PhysicsProcess(double delta)
    {
        var walkableCells = PathGrid.GetUsedCells(0);
        var blockedCells = PathGrid.GetUsedCells(1);
		MoveAndSlide();
	}
}
