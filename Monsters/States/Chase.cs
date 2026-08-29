using Godot;

public partial class Chase : State
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void PhysicsUpdate(double delta)
    {
        if (Global.Player == null || Global.Monster == null) return;

        // 1. Keep track of where the player is heading
        Global.Monster.NavigationAgent.TargetPosition = Global.Player.GlobalTransform.Origin;

        // 2. Fetch the path coordinates
        Vector3 currentLocation = Global.Monster.GlobalTransform.Origin;
        Vector3 nextLocation = Global.Monster.NavigationAgent.GetNextPathPosition();
        
        // 3. Find the horizontal direction (X and Z)
        Vector3 direction = (nextLocation - currentLocation).Normalized();

        // 4. Preserve the monster's current Y velocity (gravity calculated in Monster.cs)
        Vector3 currentVelocity = Global.Monster.Velocity;
        
        currentVelocity.X = direction.X * Global.Monster.MovementSpeed;
        currentVelocity.Z = direction.Z * Global.Monster.MovementSpeed;

        // 5. Hand the updated velocity back to the monster
        Global.Monster.Velocity = currentVelocity;
        base.PhysicsUpdate(delta);

        if (!Global.Monster.Alert)
        {
            EmitTransition("Idle");
        }
    }


    public override void Exit()
    {
        base.Exit();
    }

}
