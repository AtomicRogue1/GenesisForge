using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class JumpButton : Button
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchScreenEvent)
        {
            EmitSignal("pressed");
            SetPressedNoSignal(touchScreenEvent.Pressed);
        }
    }

}
