using Godot;
using System;
[Tool]
public partial class InitWeapon : Node3D
{
    [Export] public WeaponsData WeaponType;

    private MeshInstance3D WeaponMesh;

    public override void _Ready()
    {
        LoadWeapon();
    }

    private void LoadWeapon()
    {
        WeaponMesh.Mesh = WeaponType.Mesh;
        
    }
}
