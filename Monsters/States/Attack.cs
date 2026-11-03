using Godot;
using Timer = Godot.Timer;

public partial class Attack : State
{
    private Timer _attackTimer;
    public override void _Ready()
    {
        _attackTimer = new Timer()
        {
            WaitTime = 3.0,
            OneShot = true,
            Autostart = true
        };
        AddChild(_attackTimer);
        _attackTimer.Timeout += OnTimerTimeout;
        
    }

    public override void Enter()
    {
        base.Enter();
        OnTimerTimeout();
        _attackTimer.Start();
    }

    public override void PhysicsUpdate(double delta)
    {
        

        
    }
        void OnTimerTimeout()
        {
            GD.Print("ATTACK!!!");
        }
}
