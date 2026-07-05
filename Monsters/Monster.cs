using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Monster : CharacterBody3D
{
    
    public float MovementSpeed { get; set; } = 2f;

    public Area3D _monsterDetect;

    public MonsterStateMachine MonsterStateMachine{get;set;}

    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();


    public override void _Ready()
    {
        _monsterDetect = GetNode<Area3D>("MonsterDetect");
        _monsterDetect.AreaEntered += _on_monster_detect_area_entered;

        MonsterStateMachine = GetNode<MonsterStateMachine>("MonsterStateMachine");
    }


    public override void _PhysicsProcess(double delta)
    {
        float fDelta = (float)delta;

        Vector3 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y -= Gravity * fDelta;
        }

        Velocity = velocity;
        MoveAndSlide();
    }


    //Signal Events
    private void _on_monster_detect_area_entered(Area3D area)
    {
        GD.Print("Player Spotted!");
    }
}
