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

        if(!Input.IsActionPressed("crouch"))
            EmitTransition("IdleState");
    }

}
