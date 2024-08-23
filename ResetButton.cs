using Godot;
using System;

public partial class ResetButton : Button
{
	[Export]
	private ColorPickerButton colorPickerButton;
	private OptionManager optionManager;


	public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		this.Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		int colorPickerIndex = CustomizationManager.Instance.ColorPickerButtons.IndexOf(colorPickerButton);
		colorPickerButton.Color = optionManager.DefaultColors[colorPickerIndex];
		CustomizationManager.Instance.UpdateColors();
	}
}
