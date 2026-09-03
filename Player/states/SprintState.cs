using Godot;

namespace OddityWorld.Player.states;

public partial class SprintState : State
{
     
    public override void Enter()
    {

        Global.Player.CurrentSpeed = Global.Player.MovementSpeed * Global.Player.SprintMultiplier;
        Global.Player.CurrentBobSpeed = Global.Player.SprintBobSpeed;
        // Global.Player._camera.Fov = 10f;
    }
    

    public override void Update(double delta)
    {
        Global.Player.Camera.Fov = Mathf.Lerp(Global.Player.Camera.Fov, Global.Player.SprintFov, (float)delta * 20f);

        if (!Input.IsActionPressed("shift"))
            EmitTransition("WalkingState");
            
        
    }
    


}