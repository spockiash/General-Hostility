using Godot;
using System;
using GeneralHostility.Helpers;
using Godot.Collections;
using LogicModule;
using LogicModule.Helpers;

public partial class Robot : CharacterBody2D
{
    private int _pathIndex = 0;
    public const float Speed = 300.0f;
    public const int move_speed = 30;
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
        PathCoordinates = SetDefaultPathway();
    }

    public Vector2[] PathCoordinates { get; set; }


    public override void _PhysicsProcess(double delta)
    {
        var currentCoordinate = PathCoordinates[_pathIndex];
        if (Math.Abs(currentCoordinate.X - this.GlobalPosition.X) < 5)
        {
            currentCoordinate.X = 0;
        }
        if (Math.Abs(currentCoordinate.Y - this.GlobalPosition.Y) < 5)
        {
            currentCoordinate.Y = 0;
        }

        var direction = VCalculationsHelper.RawVectorToInput(currentCoordinate);

        if (direction == Vector2.Zero && PathCoordinates.Length > _pathIndex)
        {
            _pathIndex++;
        }

        Velocity = direction * move_speed;
        MoveAndSlide();
    }



    private Vector2[] SetDefaultPathway()
    {
        var local = PathGrid.ToLocal(this.GlobalPosition);
        var playerLocal = PathGrid.ToLocal(PlayerInstance.GlobalPosition);
        var start = PathGrid.LocalToMap(local);
        var destination = PathGrid.LocalToMap(playerLocal);
        return AStar.GetPointUnitPath(start, destination);
    }
}
