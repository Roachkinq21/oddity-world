using Godot;
using System;

public partial class WalkingState : State
{
    public override void Update(double delta)
{
    Vector3 flatVelocity = new Vector3(Global.Player.Velocity.X, 0, Global.Player.Velocity.Z);
    if (flatVelocity.Length() < 0.1f)
        EmitTransition("IdleState");
}
}
