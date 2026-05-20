using Godot;

public partial class Debugger : Control
{
    private Label _label;

    public override void _Ready()
    {
        Global.Debug = this;
        Visible = false;

        _label = GetNode<Label>("Panel/Label");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("debug"))
        {
            Visible = !Visible;
            GetViewport().SetInputAsHandled();
        }
    }

    public override void _Process(double delta)
    {
        if (Global.Player?.StateMachine?.CurrentState != null)
        {
            _label.Text = "State: " + Global.Player.StateMachine.CurrentState.Name;
        }
        
        _label.Text += "\n";
        _label.Text += "Is On Floor: " + Global.Player.IsOnFloor();
        _label.Text += "\n";
        _label.Text += "InputDir: " + Input.GetVector("left", "right", "up", "down");
        _label.Text += "\n";
        _label.Text += "Velocity: " + Global.Player.Velocity;
        _label.Text += "\n";
        _label.Text += "Gravity: " + Global.Player.Gravity;
        _label.Text += "\n";
        _label.Text += "FPS: " + Engine.GetFramesPerSecond();

        // GD.Print("=== Physics Frame ===");
        // GD.Print("IsOnFloor: " + IsOnFloor());
        // GD.Print("InputDir: " + Input.GetVector("left", "right", "up", "down"));
        // GD.Print("Direction: " + _direction);
        // GD.Print("Velocity: " + Velocity);
        // GD.Print("Gravity: " + Gravity);

    }
}