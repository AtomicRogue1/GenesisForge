using Godot;
using System;

public partial class testScript1 : Button
{
    [Export]
    public string Action = "";

    public override void _Ready()
    {
        this.Connect("pressed",new Callable(this, nameof(OnButtonPressed)));
        this.Connect("button_up",new Callable(this, nameof(OnButtonReleased)));
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
                        EmitSignal("pressed");
                    }
                    else
                    {
                        GD.Print($"{Action} released.");
                        SetPressedNoSignal(touchScreenEvent.Pressed);
                        EmitSignal("button_up");
                    }
                    break;
            }
        }
    }

    private void OnButtonPressed()
    {
        GD.Print("It works.");
    }
    private void OnButtonReleased()
    {
        GD.Print("Yay!");
    }

}
