using Godot;
using System;

public partial class CrouchingState : State
{

    public override void Enter()
    {
        base.Enter();
        Global.Player._currentSpeed = Global.Player.MovementSpeed - 0.4f;
        

    }

    public override void Update(double delta)
    {
        base.Update(delta);
        GD.Print("Crouching!");
        Global.Player._collision.Hide();
        Global.Player._collision3dCrouch.Show();

        Global.Player._camera.Position.Y = 2;

        if(!Input.IsActionPressed("crouch"))
            EmitTransition("IdleState");
    }

}
