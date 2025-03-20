using Godot;
using System;
using System.ComponentModel;

public partial class GameStartButton : Button
{
	bool startBlinking=false;
	AnimationPlayer animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		startBlinking=false;
		await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
		startBlinking=true;
		this.Pressed += OnButtonPressed;

		animationPlayer=this.GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Visible=startBlinking;
		if(startBlinking==true)
		animationPlayer.Play("GameStartButtonBlink");
	}

	private void OnButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://assets/Scenes/Game.tscn");
	}
}
