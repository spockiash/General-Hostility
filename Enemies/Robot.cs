using Godot;
using System;

public partial class Robot : CharacterBody2D
{
	public const float Speed = 300.0f;

    public override void _Ready()
    {
        root
    }

    public override void _PhysicsProcess(double delta)
	{

		MoveAndSlide();
	}
}
