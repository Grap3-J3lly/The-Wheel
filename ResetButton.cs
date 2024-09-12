using Godot;
using System;

public partial class ResetButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Export]
	private ColorPickerButton colorPickerButton;
	private GameManager gameManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		gameManager = GameManager.Instance;
		this.Pressed += OnButtonPressed;
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Assigns the given color picker button back to its default value as determined by the default color list in OptionManager, before refreshing the colors
    /// </summary>
    private void OnButtonPressed()
	{
		int colorPickerIndex = CustomizationManager.Instance.ColorPickerButtons.IndexOf(colorPickerButton);
		colorPickerButton.Color = gameManager.DefaultColors[colorPickerIndex];
		CustomizationManager.Instance.UpdateColors();
	}
}
