using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PathfindingModule
{
    public class AStarSetup
    {
        public static AStarSetup CreateInstance(int mapX, int mapY, int unitSize)
        {
            return new AStarSetup(mapX, mapY, mapY );
        }

        private AStarSetup(int mapDimensionX, int mapDimensionY, int mapUnitSize)
        {
            MapDimensionX = mapDimensionX;
            MapDimensionY = mapDimensionY;
            MapUnitSize = mapUnitSize;
            MapHalfUnitSize = mapUnitSize / 2;
            AStarGrid = new AStarGrid2D();
        }

        public int MapDimensionX { get; set; }
        public int MapDimensionY { get; set; }
        public int MapUnitSize { get; set; }
        public int MapHalfUnitSize { get; set; }
        public AStarGrid2D AStarGrid { get; set; }
        public void Initialize()
        {
            AStarGrid.Size = new Vector2i(MapDimensionX, MapDimensionY);
            AStarGrid.CellSize = new Vector2(MapUnitSize, MapUnitSize);
            AStarGrid.Update();
        }

        public void SetSolidPoint(Vector2i[] points)
        {
            foreach (var point in points)
            {
                AStarGrid.SetPointSolid(point);
            }
        }

        public Vector2[] GetPointUnitPath(Vector2i start, Vector2i destination)
        {
           return AStarGrid.GetPointPath(start, destination);
        }
    }
}