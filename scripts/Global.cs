using Godot;
using System;

public partial class Global : Node
{
    public static Global Instance { get; private set; }
    public static PlayerMovementC Player { get; set; }
    public static Debugger Debug { get; set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Player = null;
        Debug = null;
        Instance = null;
    }

}
