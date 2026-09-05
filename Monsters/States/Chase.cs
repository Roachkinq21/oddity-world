using Godot;

public partial class Chase : State
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void PhysicsUpdate(double delta)
    {
        var monster = Global.Monster;
        var player = Global.Player;

        if (monster == null || player == null)
            return;

        var agent = monster.NavigationAgent;

        agent.TargetPosition = player.GlobalPosition;

        if (agent.IsNavigationFinished())
        {
            var velocity = monster.Velocity;
            velocity.X = 0;
            velocity.Z = 0;
            monster.Velocity = velocity;
            return;
        }

        Vector3 nextPosition = agent.GetNextPathPosition();
        Vector3 direction = monster.GlobalPosition.DirectionTo(nextPosition);

        var chaseVelocity = monster.Velocity;
        chaseVelocity.X = direction.X * monster.MovementSpeed;
        chaseVelocity.Z = direction.Z * monster.MovementSpeed;
        monster.Velocity = chaseVelocity;

        if (!monster.Alert)
            EmitTransition("Idle");
    }


    public override void Exit()
    {
        base.Exit();
    }

}
