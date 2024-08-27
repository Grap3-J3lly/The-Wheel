using Godot;
using System;

public partial class LoadListButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Export]
	private CustomizeButton customizeButton;
	[Export]
	private OptionButton loadListOptionButton;
	private OptionManager optionManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		optionManager = OptionManager.Instance;
		this.Pressed += OnButtonPress;

	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Opens the customization area before loading in the option selected in the designated OptionButton
    /// </summary>
    private void OnButtonPress()
	{
		customizeButton.OnButtonPressed();
		optionManager.LoadGame(loadListOptionButton.Text);
	}
}
