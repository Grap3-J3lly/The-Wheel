using Godot;
using System;

[GlobalClass]
public partial class ColorRectChanger : ColorChanger
{
    [Export]
    private ColorRect targetRect;

    protected override void ChangeColor(Color newColor)
    {
        targetRect.Color = newColor;
    }
}
