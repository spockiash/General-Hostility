using Godot;
using System;
using GeneralHostility.Helpers;
using Godot.Collections;
using PathfindingModule;

public partial class Robot : CharacterBody2D
{
	public const float Speed = 300.0f;
    public const int UnitSize = 16;
    public TileMap PathGrid { get; set; }
    public (Array<Vector2I> walkable, Array<Vector2I> blocked, Vector2I offset) PathfinderCells { get; set; }
    public Vector2I GridSize { get; set; }
    public AStarSetup AStar { get; set; }
    public Player PlayerInstance { get; set; }
    public Vector2I GridOffset { get; set; }

    public override void _Ready()
    {
        PathGrid = (TileMap) this.GetParent().GetNode("TileMap/AstarTileMap");
        PlayerInstance = (Player)this.GetParent().GetNode("Player");
        PathfinderCells = GridHelper.GetTilesForPathfinding(PathGrid, 0, 1);
        GridSize = GridHelper.GetTileMapSize(PathfinderCells.walkable, PathfinderCells.blocked);
        GridOffset = PathfinderCells.offset;
        AStar = AStarSetup.CreateInstance(GridSize.X + 1, GridSize.Y + 1, UnitSize);
        AStar.Initialize();
        AStar.SetSolidPoint(PathfinderCells.blocked);
    }

    

    public override void _PhysicsProcess(double delta)
    {
        var test = AStar.GetPointUnitPath(VectorHelper.GetVector2I(this.GlobalPosition),
            VectorHelper.GetVector2I(PlayerInstance.GlobalPosition));
        MoveAndSlide();
	}
}
