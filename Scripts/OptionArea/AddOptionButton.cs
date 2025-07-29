using Godot;
using System.Collections.Generic;

public partial class AddOptionButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
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

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(Input.IsActionJustPressed("Add Option"))
        {
            PressButton();
            Option newOption = (Option)gameManager.OptionParent.GetChild(gameManager.OptionParent.GetChildCount() - 1);
            newOption.OptionNameField.GrabFocus();
        }
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
        ((Option)newControl).AssignFocus(gameManager.OptionParent);
		gameManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, false);
	}
}
