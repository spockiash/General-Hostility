using Godot;
using System;

namespace GeneralHostility.Data
{
    public class ProjectileData : VectorData
    {
		public Vector2 ProjectilePosition { get; set; }
		public float ProjectileRotation { get; set; }
		public Vector2 Impulse { get; set; }
		public Vector2 MuzzlePosition { get; set; }
		public float MuzzleRotation { get; set; }

		public ProjectileData(Vector2 input_direction, Vector2 character_position)
		{
			var projectileData = GetProjectileData(input_direction, character_position);
			ProjectilePosition = projectileData.projectilePosition;
			ProjectileRotation = projectileData.projectileRotation;
			Impulse = projectileData.impulse;
			MuzzlePosition = projectileData.muzzlePosition;
			MuzzleRotation = projectileData.muzzleRotation;
		}

		public (Vector2 projectilePosition, float projectileRotation, Vector2 impulse, Vector2 muzzlePosition, float muzzleRotation) GetProjectileData(Vector2 input_direction, Vector2 character_position)
		{
			var data = (Vector2.Zero, 0f, Vector2.Zero, Vector2.Zero, 0f);
			if(input_direction == RIGHT)
			{
				return (new Vector2(14 + character_position.x, 8 + character_position.y), 90, new Vector2(500, 0), new Vector2(19, 7), 0);
			}
			if(input_direction == LEFT)
			{
				return (new Vector2(-14 + character_position.x, 8 + character_position.y), -90, new Vector2(-500, 0), new Vector2(-19, 7), 135);
			}
			if(input_direction == UP)
			{
				return (new Vector2(character_position.x, -20 + character_position.y), 0, new Vector2(0, -500), new Vector2(0, -17), 55);
			}
			if(input_direction == DOWN)
			{
				return (new Vector2(character_position.x, 20 + character_position.y), 180, new Vector2(0, 500), new Vector2(0, 23), -55);
			}
			return data;
		}
    }
}