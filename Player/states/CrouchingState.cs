using Godot;

namespace OddityWorld.Player.states;

public partial class CrouchingState : State
{

    public override void Enter()
    {
        base.Enter();
        Global.Player.CurrentSpeed = Global.Player.MovementSpeed - 0.4f;
        Global.Player.SetCrouch(true);



    }

    public override void Update(double delta)
    {
        base.Update(delta);
        Global.Player.Camera.Position = Global.Player.Camera.Position.Lerp(
            new Vector3(Global.Player.Camera.Position.X, -0.3f, Global.Player.Camera.Position.Z),
            (float)delta * 10f);


        if (!Input.IsActionPressed("crouch"))
        {
            EmitTransition("IdleState");
            
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        //Global.Player.SetHeadHeight(0.0f);
        Global.Player.SetCrouch(false);
        
    }


}