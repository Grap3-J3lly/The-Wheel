using Godot;
using System.Collections.Generic;

public partial class AddOptionButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    private List<Control> createdOptions = new List<Control>();
	private OptionManager optionManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
		optionManager = OptionManager.Instance;

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
		if(optionManager.WheelSpinning) { return; }

		Control newControl = (Control)optionManager.OptionTemplate.Instantiate();
		optionManager.OptionParent.AddChild(newControl);
		optionManager.CreatedOptions.Add((Option)newControl);
		optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, false);
	}
}
