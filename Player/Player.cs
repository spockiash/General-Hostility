using Godot;
using System;

public enum PlayerState
{
	Idle,
	Walk,
	Reload
}

public partial class Player : CharacterBody2D
{
	public Vector2 UP = Vector2.Up;
	public Vector2 DOWN = Vector2.Down;
	public Vector2 LEFT = Vector2.Left;
	public Vector2 RIGHT = Vector2.Right;



	[Export]
	public int move_speed = 110;
	[Export]
	public Vector2 starting_direction = new Vector2(0,1);

	public Sprite2D onHandSprite;
	public AnimationTree animationTree;
	public AnimationNodeStateMachinePlayback stateMachine;

	public Texture2D textureShotgun = ResourceLoader.Load("res://Art/CharacterAssets/Items/Shotgun_Transparent.png") as Texture2D;
	public Texture2D texturePistol = ResourceLoader.Load("res://Art/CharacterAssets/Items/Pistol.png") as Texture2D;
	public PlayerState playerState = PlayerState.Idle;
	public Node2D muzzleFlash;
	public AnimationPlayer muzzlePlayer;
	public Vector2 lastDirection = new Vector2(1, 0);
	public bool isReloading = false;

	public const int HP = 6;

	public override void _Ready()
	{
		muzzleFlash = GetNode<Node2D>("Muzzle");
		muzzlePlayer = muzzleFlash.GetNode<AnimationPlayer>("MuzzleAnimationPlayer");
		onHandSprite = GetNode<Sprite2D>("OnHandSprite");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}


	public override void _PhysicsProcess(double delta)
	{
		var input_direction = new Vector2(
			Input.GetActionStrength("right") - Input.GetActionStrength("left"),
			Input.GetActionStrength("down") - Input.GetActionStrength("up")
		);

		if(input_direction != Vector2.Zero)
		{
			lastDirection = input_direction;
		}

		if(!isReloading)
		{
			if(input_direction != Vector2.Zero)
			{
				playerState = PlayerState.Walk;
			}
			else
			{
				playerState = PlayerState.Idle;
			}

			if(Input.IsActionJustPressed("fire"))
			{
				if(!IsDiagonal(input_direction))
				{
					isReloading = true;
					if(input_direction != Vector2.Zero)
					{
						ReloadAnimationUpdate(input_direction);
					}
					else
					{
						ReloadAnimationUpdate(lastDirection);
					}
					fire(lastDirection);
					playerState = PlayerState.Reload;
				}

			}

			if(Input.IsActionJustPressed("change_equip"))
			{
				onHandSprite.Texture = texturePistol;
			}
		}


		AnimationUpdate(input_direction);

		Velocity = input_direction * move_speed;

		PickNewState();
		if(!isReloading)
		{
			MoveAndSlide();
		}
	}


	
	public void OnArea2DAreaEntered(RigidBody2D projectile)
	{
		// (A)area.QueueFree();
		GD.Print("Hit");
	}
	
	public void AnimationUpdate(Vector2 move_input)
	{
		if(move_input != Vector2.Zero)
		{
			animationTree.Set("parameters/Walk/blend_position", move_input);
			animationTree.Set("parameters/Idle/blend_position", move_input);
		}
	}

	public void ReloadAnimationUpdate(Vector2 move_input)
	{
				
		animationTree.Set("parameters/Reload/blend_position", move_input);
	}


	public void PickNewState()
	{
		switch(playerState)
		{
			case PlayerState.Idle:
				stateMachine.Travel("Idle");
				break;
			case PlayerState.Walk:
				stateMachine.Travel("Walk");
				break;
			case PlayerState.Reload:
				stateMachine.Travel("Reload");
				break; 
		}
	}

	public void StartReload()
	{
		GD.Print("debug");
	}

	public void StopReload()
	{
		playerState = PlayerState.Idle;
		PickNewState();
		isReloading = false;
	}

	public void fire(Vector2 input_direction)
	{
		var scene = GD.Load<PackedScene>("res://Projectiles/Slug.tscn");
		var projectile_instance = (RigidBody2D)scene.Instantiate();
		var projectileData = GetProjectileData(input_direction, GlobalPosition);
		var x = GlobalPosition;
		muzzleFlash.Position = projectileData.muzzlePosition;
		muzzleFlash.Rotation = projectileData.muzzleRotation;
		muzzleFlash.Visible = true;
		muzzlePlayer.Play("Shotgun_flash");
		projectile_instance.Position = projectileData.projectilePosition;
		projectile_instance.RotationDegrees = projectileData.projectileRotation;
		projectile_instance.ApplyImpulse(projectileData.impulse);
		GetParent<Node2D>().AddChild(projectile_instance);
		// CallDeferred("add_child", projectile_instance);
		
	}

	public (Vector2 projectilePosition, float projectileRotation, Vector2 impulse, Vector2 muzzlePosition, float muzzleRotation) GetProjectileData(Vector2 input_direction, Vector2 player_position)
	{
		var data = (Vector2.Zero, 0f, Vector2.Zero, Vector2.Zero, 0f);
		if(input_direction == RIGHT)
		{
			return (new Vector2(14 + player_position.X, 8 + player_position.Y), 90, new Vector2(500, 0), new Vector2(19, 7), 0);
		}
		if(input_direction == LEFT)
		{
			return (new Vector2(-14 + player_position.X, 8 + player_position.Y), -90, new Vector2(-500, 0), new Vector2(-19, 7), 135);
		}
		if(input_direction == UP)
		{
			return (new Vector2(player_position.X, -12 + player_position.Y), 0, new Vector2(0, -500), new Vector2(0, -17), 55);
		}
		if(input_direction == DOWN)
		{
			return (new Vector2(player_position.X, 20 + player_position.Y), 180, new Vector2(0, 500), new Vector2(0, 23), -55);
		}
		return data;
	}

	public Vector2 GetProjectileStartPosition(Vector2 input_direction, Vector2 player_position)
	{
		if(input_direction == RIGHT)
		{
			return new Vector2(14, 8);
		}
		return Vector2.Zero;
	}

		public float GetProjectileStartRotation(Vector2 input_direction)
	{
		if(input_direction == RIGHT)
		{
			return 90;
		}
		return 0;
	}

	public void kill()
	{

	}

	public bool IsDiagonal(Vector2 input_direction)
	{
		var absolute_x = Math.Abs(input_direction.X);
		var absolute_y = Math.Abs(input_direction.Y);
		return absolute_x == absolute_y && input_direction != Vector2.Zero;
	}

}