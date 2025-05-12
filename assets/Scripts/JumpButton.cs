using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class JumpButton : Button
{
	[Export]
	Label UIComponent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UIComponent.Text = "";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchScreenEvent)
        {
            if (touchScreenEvent.Pressed)
            {
                UIComponent.Text=$"Touch began. Index: {touchScreenEvent.Index}, Position: {touchScreenEvent.Position}";
                SetPressedNoSignal(touchScreenEvent.Pressed);
            }
            else
            {
				UIComponent.Text=$"Touch ended. Index: {touchScreenEvent.Index}";
                SetPressedNoSignal(touchScreenEvent.Pressed);
            }
        }
    }

}
