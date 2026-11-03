using Godot;

public partial class Chase : State
{

    private double _pathTimer;
    public override void Enter()
    {
        base.Enter();
        _pathTimer = 0;
    }

    public override void PhysicsUpdate(double delta)
    {
        var monster = Global.Monster;
        var player = Global.Player;
        var attackDistance = monster.GlobalPosition.DistanceTo(player.GlobalPosition);

        if (monster == null || player == null)
            return;

        if (!monster.Alert)
        {
            StopMoving(monster);
            EmitTransition("Idle");
            return;
        }

        var agent = monster.NavigationAgent;

        _pathTimer -= delta;
        if (_pathTimer <= 0)
        {
            agent.TargetPosition = player.GlobalPosition;
            _pathTimer = 0.2;
        }

        if (agent.IsNavigationFinished())
        {
            if (attackDistance <= 1.5) ;
            EmitTransition("Attack");
            StopMoving(monster);
            return;
        }

        Vector3 nextPosition = agent.GetNextPathPosition();
        Vector3 direction = nextPosition - monster.GlobalPosition;
        direction.Y = 0;
        direction = direction.Normalized();
        monster.LookAt(monster.GlobalPosition + direction);

        var chaseVelocity = monster.Velocity;
        chaseVelocity.X = direction.X * monster.MovementSpeed;
        chaseVelocity.Z = direction.Z * monster.MovementSpeed;
        monster.Velocity = chaseVelocity;

    }

    private static void StopMoving(Monster monster)
    {
        Vector3 velocity = monster.Velocity;
        velocity.X = 0;
        velocity.Z = 0;
        monster.Velocity = velocity;
    }


    public override void Exit()
    {
        base.Exit();
    }

}
