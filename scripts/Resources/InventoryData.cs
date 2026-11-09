using Godot;
using System;

[GlobalClass]
public partial class InventoryData : Resource
{
    [Export] public Godot.Collections.Array<SlotData> SlotDatas { get; set; } = new();

}
