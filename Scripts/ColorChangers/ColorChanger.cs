using Godot;
using System;

[GlobalClass]
public partial class ColorChanger : Node
{
    [Export]
    protected ColorPalette.Colors watchedColor = ColorPalette.Colors.Background;
    private Color lastColor = new Color(0, 0, 0, 0);

    public override void _Ready()
    {
        CustomizationManager.ColorPalletChanged += OnColorPaletteChanged;
    }

    private void OnColorPaletteChanged(ColorPalette newPalette) 
    {
        Color newColor = newPalette.GetColor(watchedColor);
        if (newColor != lastColor) 
        {
            lastColor = newColor;
            ChangeColor(newColor);
        }
    }

    protected virtual void ChangeColor(Color newColor) { }
}
