using Godot;
using System;

public partial class SprintState : State
{
     
    public override void Enter()
    {

        Global.Player._currentSpeed = Global.Player.MovementSpeed * Global.Player.SprintMultiplier;
        Global.Player._currentBobSpeed = Global.Player._SprintBobSpeed;
        // Global.Player._camera.Fov = 10f;
    }
    

    public override void Update(double delta)
    {
        Global.Player._camera.Fov = Mathf.Lerp(Global.Player._camera.Fov, Global.Player.SprintFov, (float)delta * 20f);

        if (!Input.IsActionPressed("shift"))
            EmitTransition("WalkingState");
            
        
    }
    


}
