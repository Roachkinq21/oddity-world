using Godot;
using System;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string ItemName { get; set; } = "";
    [Export(PropertyHint.MultilineText)] public string ItemDescription { get; set; } = "";
    [Export] public bool Stackable { get; set; } = false;
    [Export] public Texture2D Texture { get; set; }
}
