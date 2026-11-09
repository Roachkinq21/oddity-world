using Godot;
using System;

[GlobalClass]
public partial class WeaponsData : Resource
{
    [Export] public string WeaponName = "";
    [Export] public int Damage = 0;
    [Export] public int Range = 0;
    
    [ExportCategory("Visual")]
    [Export] public Mesh Mesh;
}
