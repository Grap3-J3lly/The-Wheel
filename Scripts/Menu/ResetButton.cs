using Godot;

public partial class ResetButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    [Export]
    private CustomizationMenu customizationMenu;

    [Export]
	private ColorPickerButton colorPickerButton;
	private GameManager gameManager;

    [Export]
    private ColorPalette.Colors colorToReset;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		gameManager = GameManager.Instance;
		Pressed += OnButtonPressed;
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Assigns the given color picker button back to its default value as determined by the default color list in OptionManager, before refreshing the colors
    /// </summary>
    private void OnButtonPressed()
	{
        CustomizationManager customizationManager = CustomizationManager.Instance;

        colorPickerButton.Color = customizationManager.DefaultColors[(int)colorToReset];
        customizationManager.ResetColor((int)colorToReset);
	}
}
