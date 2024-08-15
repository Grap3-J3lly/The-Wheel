using Godot;
using System;

public partial class CustomizationManager : Control
{
    [Export]
    private ColorPickerButton generalBackgroundColor;
    [Export]
    private ColorPickerButton wheelPrimaryColor;
    [Export]
    private ColorPickerButton wheelSecondaryColor;
    [Export]
    private ColorPickerButton wheelButtonColor;
    [Export]
    private ColorPickerButton listBackgroundColor;
    [Export]
    private ColorPickerButton listFontColor;

    private OptionManager optionManager;

    public override void _Ready()
	{
        base._Ready();
        PopupManager.Instance.IsCustomizationOpen = true;
        optionManager = OptionManager.Instance;
        generalBackgroundColor.Color = optionManager.ApplicationBackground.Color;
        wheelPrimaryColor.Color = optionManager.PrimaryColor;
        wheelSecondaryColor.Color = optionManager.SecondaryColor;
        wheelButtonColor.Color = optionManager.SpinButton.GetButtonColor();
        listBackgroundColor.Color = optionManager.ListBackground.Color;
        listFontColor.Color = optionManager.GetFontColor();

        generalBackgroundColor.ColorChanged += ChangeGeneralBackgroundColor;
        wheelPrimaryColor.ColorChanged += ChangeWheelPrimaryColor;
        wheelSecondaryColor.ColorChanged += ChangeWheelSecondaryColor;
        wheelButtonColor.ColorChanged += ChangeWheelButtonColor;
        listBackgroundColor.ColorChanged += ChangeListBackgroundColor;
        listFontColor.ColorChanged += ChangeListFontColor;
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

    private void ChangeWheelButtonColor(Color color)
    {
        optionManager.SpinButton.SetButtonColor(color);
    }

    private void ChangeListBackgroundColor(Color color)
    {
        optionManager.ListBackground.Color = color;
    }

    private void ChangeListFontColor(Color color)
    {
        optionManager.SetFontColor(color);
    }
}
