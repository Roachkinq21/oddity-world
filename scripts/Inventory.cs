using Godot;
using System;

public partial class Inventory : PanelContainer
{
    GridContainer gridContainer;
    public PackedScene Slot = GD.Load<PackedScene>("res://UI/Slot.tscn");

    public override void _Ready()
    {
        gridContainer = GetNode<GridContainer>("MarginContainer/GridContainer");
    }

    public void PopulateItemGrid(Godot.Collections.Array<SlotData> slotDatas)
    {
        foreach (SlotData slotData in slotDatas)
        {
            var slot = Slot.Instantiate<Slot>();
            gridContainer.AddChild(slot);
            slot.SetSlotData(slotData);
        }
    }
}
