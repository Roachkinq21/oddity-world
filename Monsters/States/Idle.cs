using Godot;
using System;

public partial class Idle : State
{
    

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update(double delta)
    {
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
