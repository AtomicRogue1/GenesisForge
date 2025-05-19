using Godot;
using System;
using System.Linq.Expressions;

public partial class SidewayMoveSlider : HSlider
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.DragEnded += ResetSlider;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _GuiInput(InputEvent @event)
    {
        if(@event is InputEventScreenDrag screenDrag)
		{
			switch(screenDrag.Index)
			{
				case 0:
				case 1:
				case 2:
					GD.Print($"Dragging movement slider with finger {screenDrag.Index} and value is {Value}.");
					EmitSignal("drag_started");
					EmitSignal("value_changed", Value);
					break;
			}
		}
    }

	private void ResetSlider(bool valueChanged)
	{
		Value=0;
	}
}
