using Godot;

namespace LogicModule.Helpers
{
    /// <summary>
    /// Helper for manipulating vectors used in game
    /// </summary>
    public static class VCalculationsHelper
    {
        public static Vector2I SubtractVectors(Vector2I a, Vector2I b) => a - b;
        public static Vector2 SubtractVectors(Vector2 a, Vector2 b) => a - b;
        /// <summary>
        /// Flips vector along both axis.
        /// </summary>
        public static Vector2I FlipVector(Vector2I vector) => new Vector2I(vector.X * -1, vector.Y * -1);
        /// <summary>
        /// Flips vector along both axis.
        /// </summary>
        public static Vector2 FlipVector(Vector2 vector) => new Vector2(vector.X * -1, vector.Y * -1);
        /// <summary>
        /// RawVectorToInput method returns unit vector based on any vector given as input.
        /// Useful for converting target coordinates to useful vector for movement calculations.
        /// </summary>
        public static Vector2I RawVectorToInput(Vector2I a) => new Vector2I(RawToInput(a.X), RawToInput(a.Y));
        /// <summary>
        /// RawVectorToInput method returns unit vector based on any vector given as input.
        /// Useful for converting target coordinates to useful vector for movement calculations.
        /// </summary>
        public static Vector2 RawVectorToInput(Vector2 a) => new Vector2(RawToInput(a.X), RawToInput(a.Y));

        public static int RawToInput(int a)
        {
            return a switch
            {
                > 0 => -1,
                < 0 => 1,
                _ => 0
            };
        }
        public static float RawToInput(float a)
        {
            return a switch
            {
                > 0 => -1,
                < 0 => 1,
                _ => 0
            };
        }
    }
}
