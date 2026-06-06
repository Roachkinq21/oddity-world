using Godot;
using System;

public partial class SprintState : State
{
    public override void Enter()
    {
        Global.Player._currentSpeed = Global.Player.MovementSpeed * Global.Player.SprintMultiplier;
        Global.Player._currentBobSpeed = Global.Player._SprintBobSpeed;
    }

    public override void Update(double delta)
    {
        if (!Input.IsActionPressed("shift"))
            EmitTransition("WalkingState");
    }


}
