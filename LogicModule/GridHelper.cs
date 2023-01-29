using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace PathfindingModule
{
    public static class GridHelper
    {
        public static (Array<Vector2I> walkable, Array<Vector2I> blocked, Vector2I offset) GetTilesForPathfinding(TileMap refTileMap ,int walkableLayer, int blockedLayer)
        {
            var walkable = refTileMap.GetUsedCells(walkableLayer);
            var blocked = refTileMap.GetUsedCells(blockedLayer);
            var offset = GetOriginNegativeOffset(walkable, blocked);
            walkable = CorrectForNegativeOffset(walkable, offset.X, offset.Y);
            blocked = CorrectForNegativeOffset(blocked, offset.X, offset.Y);
            return (walkable, blocked, offset);
        }

        public static Vector2I GetTileMapSize(Array<Vector2I> walkable, Array<Vector2I> blocked)
        {
            var offset = GetOriginNegativeOffset(walkable, blocked);
            walkable = CorrectForNegativeOffset(walkable, offset.X, offset.Y);
            blocked = CorrectForNegativeOffset(blocked, offset.X, offset.Y);
            var walkableX = walkable.Select(o => o.X).Max();
            var walkableY = walkable.Select(o => o.Y).Max();
            var blockedX = blocked.Select(o => o.X).Max();
            var blockedY = blocked.Select(o => o.Y).Max();
            return new Vector2I(GetGrater(walkableX, blockedX), GetGrater(walkableY, blockedY));
        }

        public static Array<Vector2I> CorrectForNegativeOffset(Array<Vector2I> vectorArray, int offsetX, int offsetY)
        {
            var result = new Array<Vector2I>();
            foreach (var vec in vectorArray)
            {
                var res = new Vector2I(vec.X + offsetX, vec.Y + offsetY);
                result.Add(res);
            }
            return result;
        }

        public static Vector2I GetOriginNegativeOffset(Array<Vector2I> walkable, Array<Vector2I> blocked)
        {
            var result = new Vector2I(0,0);
            var walkableX = walkable.Select(o => o.X).Min();
            var walkableY = walkable.Select(o => o.Y).Min();
            var blockedX = blocked.Select(o => o.X).Min();
            var blockedY = blocked.Select(o => o.Y).Min();
            var offsetValues = new Vector2I(GetLesser(walkableX, blockedX), GetLesser(walkableY, blockedY));
            if (offsetValues.X < 0)
            {
                result.X = Math.Abs(offsetValues.X);
            }
            if (offsetValues.Y < 0)
            {
                result.Y = Math.Abs(offsetValues.X);
            }

            return result;
        }

        private static int GetGrater(int a, int b)
        {
            return a >= b ? a : b;
        }

        private static int GetLesser(int a, int b)
        {
            return a >= b ? a : b;
        }
    }
}
