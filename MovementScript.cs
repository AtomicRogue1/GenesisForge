using Godot;
using System;

public partial class MovementScript : RigidBody2D
{
	[Export]	
	float moveSpeed=10f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsActionPressed("ui_left"))
		{
			LinearVelocity=new Vector2(-moveSpeed,LinearVelocity.Y);
		}
		else if(Input.IsActionPressed("ui_right"))
		{
			LinearVelocity=new Vector2(moveSpeed,LinearVelocity.Y);
		}
		else
		{
		}
	}
}
