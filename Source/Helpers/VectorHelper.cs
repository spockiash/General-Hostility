using Godot;
using System;

namespace GeneralHostility.Helpers
{
    public static class VectorHelper
    {
        public static Vector2 GetAbsoluteDelta(Vector2 a, Vector2 b) => new Vector2(
            Math.Abs(a.x) - Math.Abs(b.x),
            Math.Abs(a.y) - Math.Abs(b.y)
        );
    }
}