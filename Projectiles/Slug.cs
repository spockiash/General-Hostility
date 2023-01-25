using Godot;
using System;
using System.Timers;

public partial class Slug : RigidBody2D
{

	public const int Damage = 2;
	private static System.Timers.Timer aTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.AddCollisionExceptionWith(this);
		SetTimer(500);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print("raw");
	}

	private void SetTimer(int timer)
   {
        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(timer);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = false;
        aTimer.Enabled = true;
    }

	private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
		if(IsInstanceValid(this))
		{
			this.QueueFree();
		}
		else
		{
			this.Dispose();
		}
    }
}
