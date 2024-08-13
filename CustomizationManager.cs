using Godot;
using System;

public partial class CustomizationManager : Control
{
    // public static CustomizationManager Instance { get; private set; }

    [Export]
    private ColorPickerButton generalBackgroundColor;
    [Export]
    private ColorPickerButton wheelPrimaryColor;
    [Export]
    private ColorPickerButton wheelSecondaryColor;

    private OptionManager optionManager;

    public override void _Ready()
	{
        base._Ready();
        optionManager = OptionManager.Instance;
        generalBackgroundColor.Color = optionManager.ApplicationBackground.Color;
        wheelPrimaryColor.Color = optionManager.PrimaryColor;
        wheelSecondaryColor.Color = optionManager.SecondaryColor;

        generalBackgroundColor.ColorChanged += ChangeGeneralBackgroundColor;
        wheelPrimaryColor.ColorChanged += ChangeWheelPrimaryColor;
        wheelSecondaryColor.ColorChanged += ChangeWheelSecondaryColor;
	}

    private void ChangeGeneralBackgroundColor(Color color)
    {
        optionManager.ApplicationBackground.Color = color;
    }

    private void ChangeWheelPrimaryColor(Color color)
    {
        optionManager.PrimaryColor = color;
        optionManager.UpdateWheelColors();
    }

    private void ChangeWheelSecondaryColor(Color color)
    {
        optionManager.SecondaryColor = color;
        optionManager.UpdateWheelColors();
    }
}
