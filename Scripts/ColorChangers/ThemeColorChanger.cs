using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class ThemeColorChanger : ColorChanger
{
    [Export]
    private Theme targetTheme;
    [Export]
    private Array<ThemeChangerLineOption> lineOptions = new Array<ThemeChangerLineOption>();

    protected override void ChangeColor(Color newColor)
    {
        foreach (ThemeChangerLineOption option in lineOptions) 
        {
            Color lineColor = newColor.Darkened(option.darkenBy);
            lineColor.A = lineColor.A * option.opacity;

            if (option.useStyleBox)
            {
                StyleBox sBox = (StyleBox)targetTheme.Get(option.targetLine);
                sBox.Set(option.styleBoxLine, lineColor);
            }
            else
            {
                targetTheme.Set(option.targetLine, newColor);
            }
        }
    }
}
