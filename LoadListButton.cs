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
	private GameManager gameManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		gameManager = GameManager.Instance;
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
		gameManager.LoadGame(loadListOptionButton.Text);
	}
}
