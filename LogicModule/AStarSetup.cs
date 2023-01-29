using Godot;
using Godot.Collections;

namespace PathfindingModule
{
    public class AStarSetup
    {
        public static AStarSetup CreateInstance(int mapX, int mapY, int unitSize)
        {
            return new AStarSetup(mapX, mapY, unitSize);
        }

        private AStarSetup(int mapDimensionX, int mapDimensionY, int mapUnitSize)
        {
            MapDimensionX = mapDimensionX;
            MapDimensionY = mapDimensionX;
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
            AStarGrid.Size = new Vector2I(MapDimensionX, MapDimensionY);
            AStarGrid.CellSize = new Vector2(MapUnitSize, MapUnitSize);
            AStarGrid.Update();
        }

        public void SetSolidPoint(Array<Vector2I> points)
        {
            foreach (var point in points)
            {
                AStarGrid.SetPointSolid(point);
            }
        }

        public Vector2[] GetPointUnitPath(Vector2I start, Vector2I destination)
        {
           return AStarGrid.GetPointPath(start, destination);
        }
    }
}