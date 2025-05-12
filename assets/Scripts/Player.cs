using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
	// Accessing input
	Button jumpButton;
	HSlider movementInputSlider;

	[ExportCategory("Movement")]
	[Export]
	public float moveSpeed = 300.0f;
	[Export]
	public float JumpVelocity = -400.0f;
	[Export]
	public float acceleration = 3;
	[Export]
	public float deceleration = 3;
	bool sidewayInputBeingDragged;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		// Getting them from the tree
		movementInputSlider = GetNode<HSlider>("../CanvasLayer/MovementInputSlider");
		jumpButton = GetNode<Button>("../CanvasLayer/JumpButton");
		sidewayInputBeingDragged=false;
	}

	public float lerp(float a, float b, float t)
	{
		return a + (b - a) * t;
	}

	public override void _PhysicsProcess(double delta)
	{
		float sidewayInput = (float)movementInputSlider.Value;
		Godot.Vector2 velocity = Velocity;

		velocity.X = moveSpeed * sidewayInput; // => This is zero acceleration movement.

		velocity.Y += (float)(gravity * delta);

		Velocity = velocity;
		MoveAndSlide();
	}

	private void _on_movement_input_slider_drag_started()
	{
		sidewayInputBeingDragged=true;
	}

	private void _on_movement_input_slider_drag_ended(bool value_changed)
	{
		sidewayInputBeingDragged=false;
	}
}
