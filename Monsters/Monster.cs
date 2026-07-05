using Godot;
using System;

public partial class Monster : CharacterBody3D
{
    
    public float MovementSpeed {get;set;} = 2f;

    public Area3D _monsterDetect;




    
    public void _on_monster_detect_area_entered()
    {
        GD.Print("Player Spotted!");
    }
}
