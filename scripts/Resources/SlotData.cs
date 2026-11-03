using Godot;
using System;

public partial class SlotData : Resource
{
    private const int MAXSTACK = 10;
    
    [Export] public ItemData ItemData;
    [Export(PropertyHint.Range, "1,10")]
    public int Quantity { get; set; } = 1;
}
