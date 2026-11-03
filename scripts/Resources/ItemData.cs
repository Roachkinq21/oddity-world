using Godot;
using System;

public partial class ItemData : Resource
{
    [Export] public string ItemName { get; set; } = "";
    [Export(PropertyHint.MultilineText)] public string ItemDescription { get; set; } = "";
    [Export] public bool Stackable { get; set; } = false;
    [Export]public AtlasTexture Texture { get; set; }
}
