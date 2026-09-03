using Godot;

namespace OddityWorld.Player;

public partial class Flashlight : SpotLight3D
{
    public override void _Ready()
    {
        this.Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("flashlight"))
        {
            Visible = !Visible;
        }
    }
}