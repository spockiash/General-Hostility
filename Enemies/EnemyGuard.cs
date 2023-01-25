//using Godot;
//using System;
//using System.Collections.Generic;
//using System.Timers;
//using GeneralHostility.Helpers;
//using GeneralHostility.Data;

//public enum EnemyState
//{
//	Idle,
//	Reload,
//	Alert,
//	Seeker,
//	Engaged
//}

//public partial class EnemyGuard : CharacterBody2D
//{
//	public Vector2 UP = Vector2.Up;
//	public Vector2 DOWN = Vector2.Down;
//	public Vector2 LEFT = Vector2.Left;
//	public Vector2 RIGHT = Vector2.Right;
//	[Export]
//	public int move_speed = 75;
//	public RayCast2D rayCast;

//	public CharacterBody2D player;

//	public Vector2[] cast_coordinates;

//	public const float angle_cone_of_vision = 6.28f;
//	public const float angle_between_rays = 0.05f;
//	public const float max_view_distance = 103f;	
//	public Vector2 latestPlayerPosition = Vector2.Zero;
//	public Vector2 targetPositon = Vector2.Zero;
//	public Vector2 move_direction = Vector2.Zero;
//	public Vector2 fire_direction = Vector2.Zero;
//	public int HP = 4;

//	public EnemyState currentState;
//	public EnemyState latestState;
//	public AnimationTree animationTree;
//	public AnimationNodeStateMachinePlayback stateMachine;
//	public AnimationPlayer muzzlePlayer;
//	public Node2D muzzleFlash;

//	public bool isReloading = false;
//	private static System.Timers.Timer aTimer;

//	public override void _Ready()
//	{
//		muzzleFlash = GetNode<Node2D>("Muzzle");
//		muzzlePlayer = muzzleFlash.GetNode<AnimationPlayer>("MuzzleAnimationPlayer");
//		animationTree = GetNode<AnimationTree>("AnimationTree");
//		currentState = EnemyState.Idle;
//		cast_coordinates = PrecalculateRayCoordinates();
//		rayCast = GetNode<RayCast2D>("RayCast2D");
//		rayCast.SetPhysicsProcess(true);
//		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
//	}

//	public override void _PhysicsProcess(double delta)
//	{
//		if(HP <= 0)
//		{
//			this.QueueFree();
//		}
//		Vector2 velocity = Velocity;

//		foreach(var ray_coordinates in cast_coordinates)
//		{
//			rayCast.TargetPosition = ray_coordinates;
//			rayCast.ForceRaycastUpdate();
//			var debug = rayCast.IsColliding();
//			if(rayCast.IsColliding())
//			{
//				var target = rayCast.GetCollider();
//				if(target is CharacterBody2D)
//				{	
//					var character = (CharacterBody2D)target;
//					if(character.Name == "Player")
//					{
//						this.currentState = EnemyState.Alert;
//						player = character;
//						latestPlayerPosition = player.GlobalPosition;
//						break;
//					}
//					else
//					{
//						player = null;
//					}
//				}
//			}
//			else
//			{
//				if(currentState == EnemyState.Engaged)
//				{
//					currentState = EnemyState.Seeker;
//				}
//			}
//		}

//		if(player != null)
//		{
//			move_direction = GetInputDirectionVector(this.Position, player.Position);
//		}
		
//				//Alerted enemies have player within their active range
//		if(currentState == EnemyState.Alert)
//		{
//			// var engageData = GetEngagePositionAndDirection(latestPlayerPosition, this.GlobalPosition);
//			// latestPlayerPosition = engageData.targetPosition;
//			// input_direction = engageData.inputDirection;
			
//			if(move_direction == Vector2.Zero && !isReloading)
//			{
//				currentState = EnemyState.Engaged;
//			}
//		}
//		if(currentState == EnemyState.Engaged)
//		{
//			fire_direction = GetInputDirectionVector(this.Position, player.Position, true);
//			var absDelta = GetAbsoluteVector(GetAbsoluteDelta(this.Position, player.Position));
//			// if(absDelta.x > 18 || absDelta.y > 18)
//			// {
//			// 	move_direction = fire_direction; 
//			// }
//			Fire(fire_direction);
//			GD.Print("Fire, fire, fire!");
//		}
//		//After ray casting loop and player identification calculate coordinates
//		Velocity = move_direction * move_speed;
//		AnimationUpdate(move_direction);

//		PickNewState();

//		if(!isReloading)
//		{
//			MoveAndSlide();
//		}
//	}
//	//Returns vector with max values of 1 or -1
//	public Vector2 GetInputDirectionVector(Vector2 enemyPosition, Vector2 playerPosition, bool zeroesPerAxis = false)
//	{
//		var result = Vector2.Zero;
//		var absoluteDelta = GetAbsoluteDelta(enemyPosition, playerPosition);
//		if(!zeroesPerAxis)
//		{
//			if((absoluteDelta.x < 3 && absoluteDelta.x > -3) || (absoluteDelta.y < 3 && absoluteDelta.y > -3))
//			{
//				return result;
//			}
//		}
//		var delta = SubtractVectors(enemyPosition, playerPosition);
//		if(zeroesPerAxis)
//		{
//			if(absoluteDelta.x < 3 && absoluteDelta.x > -3)
//			{
//				delta.x = 0;
//			}
//			if(absoluteDelta.y < 3 && absoluteDelta.y > -3)
//			{
//				delta.y = 0;
//			}
//		}

//		return RawVectorToInput(delta);
//	}

//	public Vector2 SubtractVectors(Vector2 a, Vector2 b) => a - b;

//	public Vector2 FlipVector(Vector2 vector) => new Vector2(vector.x * -1, vector.y * -1);

//	public Vector2 RawVectorToInput(Vector2 a) => new Vector2(RawToInput(a.x), RawToInput(a.y));
//	public float RawToInput(float a)
//	{
//		var result = 0f;

//		if(a > 0 )
//		{
//			return -1;
//		}
//		if(a < 0)
//		{
//			return 1;
//		}
//		return result;
//	}
//	public Vector2[] PrecalculateRayCoordinates()
//	{
//		var cast_vectors = new List<Vector2>();

//		var cast_count = (int)Math.Round(angle_cone_of_vision / angle_between_rays + 1);

//		for(int i = 0; i < cast_count; i++ )
//		{
//			var cast_vector = max_view_distance * Vector2.Up.Rotated(angle_between_rays * (i - cast_count) /2 );
//			cast_vectors.Add(cast_vector);
//		}

//		return cast_vectors.ToArray();
//	}

//	public Vector2 GetAbsoluteDelta(Vector2 a, Vector2 b) => new Vector2(
//		Math.Abs(a.x) - Math.Abs(b.x),
//		Math.Abs(a.y) - Math.Abs(b.y)
//	);

//	public Vector2 GetAbsoluteVector(Vector2 vector) => new Vector2(Math.Abs(vector.x), Math.Abs(vector.x));
//	public (Vector2 targetPosition, Vector2 inputDirection) GetEngagePositionAndDirection(Vector2 playerGlobal, Vector2 enemyGlobal)
//	{
//		var deltaXAbs = Math.Abs(enemyGlobal.x) - Math.Abs(playerGlobal.x);
//		var deltaYAbs = Math.Abs(enemyGlobal.y) - Math.Abs(playerGlobal.y);
//		if(deltaXAbs < 3 && deltaXAbs > -3)
//		{
//			return (enemyGlobal, new Vector2(0,0));
//		}
//		if(deltaXAbs >= deltaYAbs)
//		{
//			return (new Vector2(enemyGlobal.x, playerGlobal.y), deltaYAbs > 0 ?  new  Vector2(0,-1) : new  Vector2(0,1));
//		}
//		if(deltaYAbs >= deltaXAbs)
//		{
//			return (new Vector2(playerGlobal.x, enemyGlobal.y), deltaXAbs > 0 ?  new  Vector2(-1,0) : new  Vector2(1,0));
//		}
//		return (enemyGlobal, new Vector2(0,0));
//	}

//	public Vector2[] GetAttackPath()
//	{
//		var path_vectors = new List<Vector2>();
//		//calculate position
//		return path_vectors.ToArray();
//	}

//	public void AnimationUpdate(Vector2 move_input)
//	{
//		if(move_input != Vector2.Zero)
//		{
//			animationTree.Set("parameters/Walk/blend_position", move_input);
//			animationTree.Set("parameters/Idle/blend_position", move_input);
//		}
//	}
//	public void ReloadAnimationUpdate(Vector2 move_input)
//	{
				
//		animationTree.Set("parameters/Reload/blend_position", move_input);
//	}

//	public void Fire(Vector2 fire_direction)
//	{
//		SetReloadTimer();
//		currentState = EnemyState.Reload;
//		isReloading = true;
//		var scene = GD.Load<PackedScene>("res://Projectiles/Slug.tscn");
//		var projectile_instance = (RigidBody2D)scene.Instantiate();
//		ReloadAnimationUpdate(fire_direction);
//		// var projectileData = GetProjectileData(fire_direction, this.Position);
//		var projectileData = new ProjectileData(fire_direction, this.Position);
//		muzzleFlash.Position = projectileData.MuzzlePosition;
//		muzzleFlash.Rotation = projectileData.MuzzleRotation;
//		muzzleFlash.Visible = true;
//		muzzlePlayer.Play("Shotgun_flash");
//		projectile_instance.Position = projectileData.ProjectilePosition;
//		projectile_instance.RotationDegrees = projectileData.ProjectileRotation;
//		projectile_instance.ApplyImpulse(projectileData.Impulse);
//		GetParent<Node2D>().AddChild(projectile_instance);
//	}

//	public void StopReload()
//	{
//		isReloading = false;
//		currentState = EnemyState.Idle;
//		PickNewState();
//	}

//	public void PickNewState()
//	{
//		switch(currentState)
//		{
//			case EnemyState.Idle:
//				stateMachine.Travel("Idle");
//				break;
//			case EnemyState.Alert:
//				stateMachine.Travel("Walk");
//				break;
//			case EnemyState.Reload:
//				stateMachine.Travel("Reload");
//				break; 
//		}
//	}

//	private void SetReloadTimer()
//   	{
//        // Create a timer with a two second interval.
//        aTimer = new System.Timers.Timer(400);
//        // Hook up the Elapsed event for the timer. 
//        aTimer.Elapsed += OnTimedEvent;
//        aTimer.AutoReset = false;
//        aTimer.Enabled = true;
//    }

//    private void OnTimedEvent(object source, ElapsedEventArgs e)
//    {
//		isReloading = false;
//		currentState = EnemyState.Idle;
//		PickNewState();
//    }

//	public void _on_area_2d_body_entered(Node2D body)
//	{
//		if(body.Name == "Slug")
//		{
//			Hit(2);
//		}
//	}
//	public void Hit(int dmg)
//	{
//		HP = HP - dmg;
//	}
//}
