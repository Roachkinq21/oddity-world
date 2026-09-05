using Godot;

public partial class Idle : State
{
    

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update(double delta)
    {

        var velocity = Global.Monster.Velocity;
        velocity.X = 0;
        velocity.Z = 0;
        Global.Monster.Velocity = velocity;
        
        base.Update(delta);
        if (Global.Monster.Alert)
        {
            EmitTransition("Chase");
        }
    }

    public override void Exit()
    {
        
        base.Exit();
    }






}
