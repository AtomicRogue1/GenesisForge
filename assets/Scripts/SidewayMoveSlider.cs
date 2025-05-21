using Godot;
using System;

public partial class SidewayMoveSlider : HSlider
{
	// [Signal]
	// public delegate void DragEndedWithOtherFingerEventHandler();
	public override void _Ready()
	{
		this.DragEnded += ResetSlider;
		// Connect("DragEndedWithOtherFinger", new Callable(this, nameof(work)));
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventScreenDrag screenDrag)
		{
			switch (screenDrag.Index)
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

	// This function is to reset the slider once dragging ends, just for looking neat.
	private void ResetSlider(bool valueChanged)
	{
		Value = 0;
	}
}
