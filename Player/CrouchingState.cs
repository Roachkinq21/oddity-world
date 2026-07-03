using Godot;
using System;

public partial class CrouchingState : State
{

    public override void Enter()
    {
        base.Enter();
        Global.Player._currentSpeed = Global.Player.MovementSpeed - 0.4f;
        Global.Player.SetCrouch(true);
        Global.Player.SetHeadHeight(-0.3f);
        

    }

    public override void Update(double delta)
    {
        base.Update(delta);
        GD.Print("Crouching!");

        if(!Input.IsActionPressed("crouch"))
            EmitTransition("IdleState");
    }

    public override void Exit()
    {
        base.Exit();
        Global.Player.SetHeadHeight(0.0f);
        Global.Player.SetCrouch(false);
        
    }


}
