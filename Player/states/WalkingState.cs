using Godot;
using System;

public partial class WalkingState : State
{

    public override void Enter()
    {
        Global.Player._currentSpeed = Global.Player.MovementSpeed;
        Global.Player._currentBobSpeed = Global.Player._BaseBobSpeed;
    }


    public override void Update(double delta)
{
    Vector3 flatVelocity = new Vector3(Global.Player.Velocity.X, 0, Global.Player.Velocity.Z);
    if (flatVelocity.Length() < 0.1f)
        EmitTransition("IdleState");

    if (Input.IsActionPressed("shift"))
        EmitTransition("SprintState");
}
}
