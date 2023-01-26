// using Godot;
// using Godot.Collections;
// using System;
// using System.Collections.Generic;
// using System.Linq;

// public partial class AstarTileMap : TileMap
// {
// 	[Export]
// 	public Vector2 MapSize { get; set; }
//     public Array<Vector2i> Obstacles { get; private set; }
//     public AStar2D AstarNode { get; set; }
//     public int CellSize { get; private set; }
//     public int HalfCellSize { get; private set; }
//     public Array<Vector2> WalkableCellList { get; private set; }
// 	public Vector2 PathStartPosition { get; set; }
// 	public Vector2 PathEndPosition { get; set; }
//     public Vector2[] PointPath { get; private set; }



//     // Called when the node enters the scene tree for the first time.
//     public override void _Ready()
// 	{
// 		MapSize = Vector2.One * 32;
// 		Obstacles = this.GetUsedCells(1);
// 		AstarNode = new AStar2D();
// 		CellSize = this.CellQuadrantSize;
// 		HalfCellSize = CellSize / 2;
// 		WalkableCellList = AstarAddWalkableCells(Obstacles);
// 		AstarConnectWalkableCells(WalkableCellList);
		
// 	}

// 	// Called every frame. 'delta' is the elapsed time since the previous frame.
// 	public override void _Process(double delta)
// 	{
// 	}

// 	public Array<Vector2> AstarAddWalkableCells(Array<Vector2i> obstacles)
// 	{
// 		var pointsArray = new Array<Vector2>();
// 		for (int y = 0; y < MapSize.y; y++)
// 		{
// 			for (int x = 0; x < MapSize.x; x++)
// 			{
// 				var point = new Vector2i(x, y);
// 				if(obstacles.Contains(point))
// 				{
// 					continue;
// 				}
// 				var pointIndex = CalculatePointIndex(point);
// 				pointsArray.Add(new Vector2(point.x, point.y));
// 				AstarNode.AddPoint(pointIndex, new Vector2(point.x, point.y));
// 			}	
// 		}
// 		return pointsArray;
// 	}
	
// 	public void AstarConnectWalkableCells(Array<Vector2> pointsArray)
// 	{
// 		foreach (var point in pointsArray)
// 		{
// 			var pointIndex = CalculatePointIndex(point);
// 			var pointsRelative = new List<Vector2>
// 			{
// 				point + Vector2.Right,
// 				point + Vector2.Left,
// 				point + Vector2.Down,
// 				point + Vector2.Up,
// 			};
// 			foreach(var pointRelative in pointsRelative)
// 			{
// 				var pointRelativeIndex = CalculatePointIndex(pointRelative);
// 				if(IsOutsideBounds(pointRelative) || pointRelative == point)
// 				{
// 					continue;
// 				}
// 				if(AstarNode.HasPoint(pointRelativeIndex))
// 				{
// 					continue;
// 				}
// 				AstarNode.ConnectPoints(pointIndex, pointRelativeIndex, true);
// 			}

// 		}
// 	}

// 	public long CalculatePointIndex(Vector2i point)
// 	{
// 		return (long)(point.x + MapSize.x * point.y);
// 	}
// 	public long CalculatePointIndex(Vector2 point)
// 	{
// 		return (long)(point.x + MapSize.x * point.y);
// 	}

// 	public bool IsOutsideBounds(Vector2 point)
// 	{
// 		return point.x < 0 || point.y < 0 || point.x >= MapSize.x || point.y >= MapSize.y;
// 	}

// 	public void RecalculatePath(Vector2 pathStartPosition, Vector2 pathEndPosition)
// 	{
// 		var startPointIndex = CalculatePointIndex(pathStartPosition);
// 		var endPointIndex = CalculatePointIndex(pathEndPosition);
// 		PointPath = AstarNode.GetPointPath(startPointIndex, endPointIndex);
// 		var x = 1;
// 	}

// 	public void SetPathStartPosition(Vector2 value)
// 	{
// 		if(Obstacles.Contains((Vector2i)value))
// 		{
// 			return;
// 		}
// 		if(IsOutsideBounds(value))
// 		{
// 			return;
// 		}
// 		PathStartPosition = value;
// 		if(PathEndPosition != PathStartPosition)
// 		{
// 			RecalculatePath(PathStartPosition, PathEndPosition);
// 		}

// 	}

// 	public void SetEndStartPosition(Vector2 value)
// 	{
// 		if(Obstacles.Contains((Vector2i)value))
// 		{
// 			return;
// 		}
// 		if(IsOutsideBounds(value))
// 		{
// 			return;
// 		}
// 		PathEndPosition = value;
// 		if(PathEndPosition != value)
// 		{
// 			RecalculatePath(PathStartPosition, PathEndPosition);
// 		}
// 	}

// 	public Vector2[] GetAstarPath(Vector2 world_start, Vector2 world_end)
// 	{
// 		this.PathStartPosition = LocalToMap(world_start);
// 		this.PathEndPosition = LocalToMap(world_end);
// 		RecalculatePath(this.PathStartPosition, this.PathEndPosition);
// 		var pathWorld = new List<Vector2>();
// 		foreach (var point in PointPath)
// 		{
// 			var pointWorld = this.MapToLocal(new Vector2i((int)point.x + HalfCellSize, (int)point.y + HalfCellSize));
// 			pathWorld.Add(pointWorld);
// 		}
// 		return pathWorld.ToArray();
// 	}
// }
