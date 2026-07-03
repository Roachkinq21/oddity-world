using Godot;
using System;

public partial class IdleState : State
{
    public override void Update(double delta)
{
    Global.Player._camera.Fov = Mathf.Lerp(Global.Player._camera.Fov, Global.Player.BaseFov, (float)delta * 10f);

    Vector3 flatVelocity = new Vector3(Global.Player.Velocity.X, 0, Global.Player.Velocity.Z);

    if (flatVelocity.Length() > 0.1f)
        EmitTransition("WalkingState");
        
    if(Input.IsActionPressed("crouch"))
        EmitTransition("CrouchingState");
}

}
