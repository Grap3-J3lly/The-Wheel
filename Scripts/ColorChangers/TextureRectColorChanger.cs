using Godot;
using System;

[GlobalClass]
public partial class TextureRectColorChanger : ColorChanger
{
    [Export]
    private TextureRect targetRect;

    protected override void ChangeColor(Color newColor)
    {
        targetRect.Modulate = newColor;
    }
}
