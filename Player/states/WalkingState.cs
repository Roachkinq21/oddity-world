using Godot;
using System;

public partial class WalkingState : State
{

    public override void Enter()
    {
        Global.Player.CurrentSpeed = Global.Player.MovementSpeed;
        Global.Player.CurrentBobSpeed = Global.Player.BaseBobSpeed;
    }


    public override void Update(double delta)
{
    Global.Player.Camera.Fov = Mathf.Lerp(Global.Player.Camera.Fov, Global.Player.BaseFov, (float)delta * 10f);

    Vector3 flatVelocity = new Vector3(Global.Player.Velocity.X, 0, Global.Player.Velocity.Z);
    if (flatVelocity.Length() < 0.1f)
        EmitTransition("IdleState");

    if (Input.IsActionPressed("shift"))
        EmitTransition("SprintState");

    if(Input.IsActionPressed("crouch"))
        EmitTransition("CrouchingState");
}
}
