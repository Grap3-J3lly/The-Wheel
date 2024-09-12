using Godot;
using System.Collections.Generic;

public partial class AddOptionButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    private List<Control> createdOptions = new List<Control>();
	private GameManager gameManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
		gameManager = GameManager.Instance;

		// Create Default Option
		PressButton();
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

	/// <summary>
	/// Creates a new option and adds it to the option list. 
	/// Triggers the WheelProgressUpdate event to update the Wheel
	/// </summary>
    private void PressButton()
	{
		if(gameManager.WheelSpinning) { return; }

		Control newControl = (Control)gameManager.OptionTemplate.Instantiate();
		gameManager.OptionParent.AddChild(newControl);
		gameManager.CreatedOptions.Add((Option)newControl);
		gameManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, false);
	}
}
