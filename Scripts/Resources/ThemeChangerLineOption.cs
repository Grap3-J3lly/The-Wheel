using Godot;
using System;

[GlobalClass]
public partial class ThemeChangerLineOption : Resource
{
    [Export]
    public string targetLine = "";
    [Export]
    public float darkenBy = 0;
    [Export]
    public float opacity = 1;
    [ExportGroup("StyleBox")]
    [Export]
    public bool useStyleBox = false;
    [Export]
    public string styleBoxLine = "";
}
