using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
	// Accessing input
	Button jumpButton;
	HSlider movementInputSlider;

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
		// Getting them from the tree
        movementInputSlider=GetNode<HSlider>("MovementInputSlider");
		jumpButton=GetNode<Button>("JumpButton");
    }

    public override void _PhysicsProcess(double delta)
	{
		float direction = (float)movementInputSlider.Value;
		Godot.Vector2 velocity = Velocity;

		velocity.X = Speed * direction;
		velocity.Y += (float)(gravity * delta);
	}
}
