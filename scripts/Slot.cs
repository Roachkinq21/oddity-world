using Godot;
using System;

public partial class Slot : PanelContainer
{
    TextureRect textureRect;
    Label label;

    public override void _Ready()
    {
        textureRect = GetNode<TextureRect>("MarginContainer/TextureRect");
        label = GetNode<Label>("Label");
    }
    
    public void SetSlotData(SlotData SlotData)
    {
        var item_data = SlotData.ItemData;
        textureRect.Texture = item_data.Texture;
    }
}
