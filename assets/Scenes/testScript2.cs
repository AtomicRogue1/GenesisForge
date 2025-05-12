using Godot;
using System;

public partial class testScript2 : Button
{
    [Export]
    public string Action = "";

    public override void _Ready()
    {
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchScreenEvent)
        {
            switch (touchScreenEvent.Index)
            {
                case 0:
                case 1:
                case 2:
                    if (touchScreenEvent.Pressed)
                    {
                        GD.Print($"{Action} was pressed with finger {touchScreenEvent.Index}");
                        SetPressedNoSignal(touchScreenEvent.Pressed);
                    }
                    else
                    {
                        GD.Print($"{Action} released.");
                        SetPressedNoSignal(touchScreenEvent.Pressed);
                    }
                    break;
            }
        }
    }
}