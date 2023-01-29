using Godot;
using System;

namespace GeneralHostility.Helpers
{
    public static class VectorHelper
    {
        public static Vector2 GetAbsoluteDelta(Vector2 a, Vector2 b) => new Vector2(
            Math.Abs(a.X) - Math.Abs(b.X),
            Math.Abs(a.Y) - Math.Abs(b.Y)
        );

        public static Vector2 GetVector2(Vector2I vect) => new(vect.X, vect.Y);
        public static Vector2I GetVector2I(Vector2 vect) => new((int) Math.Round(vect.X), (int)Math.Round(vect.X));
    }
}