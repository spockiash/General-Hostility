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
        var local = PathGrid.ToLocal(this.GlobalPosition);
        var playerLocal = PathGrid.ToLocal(PlayerInstance.GlobalPosition);
        var start = PathGrid.LocalToMap(local);
        var destination = PathGrid.LocalToMap(playerLocal);
        var test = AStar.GetPointUnitPath(start, destination);
        MoveAndSlide();
	}
    
}
