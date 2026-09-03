using Godot;

namespace OddityWorld.Player.states;

internal partial class IdleState : State
{
    public override void Update(double delta)
    {
        Global.Player.Camera.Fov = Mathf.Lerp(Global.Player.Camera.Fov, Global.Player.BaseFov, (float)delta * 10f);
        Global.Player.Camera.Position = Global.Player.Camera.Position.Lerp(
            new Vector3(Global.Player.Camera.Position.X, 0.0f, Global.Player.Camera.Position.Z),
            (float)delta * 10f);

        Vector3 flatVelocity = new Vector3(Global.Player.Velocity.X, 0, Global.Player.Velocity.Z);

        if (flatVelocity.Length() > 0.1f)
            EmitTransition("WalkingState");
        
        if(Input.IsActionPressed("crouch"))
            EmitTransition("CrouchingState");
    }

}