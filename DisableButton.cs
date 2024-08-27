using Godot;
using System;

public partial class DisableButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private Option optionParent;

	private OptionManager optionManager;
	private bool enabled = true;

    // --------------------------------
    //		    PROPERTIES
    // --------------------------------

    public bool Enabled { get => enabled; set => enabled = value; }

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		enabled = true;
        optionParent = (Option)GetParentControl();
		this.Pressed += PressButton;
	}

    // --------------------------------
    //		    BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Enables or disables assigned option based on current enabled value, moving the option to the correct list for tracking, and refreshes the wheel
    /// </summary>
    private void PressButton()
	{
        if (optionManager.WheelSpinning) { return; }

        enabled = !enabled;
        optionParent.OptionEnabled = enabled;
        
        if(enabled)
        {
            optionManager.DisabledOptions.Remove(optionParent);
            optionManager.CreatedOptions.Add(optionParent);
        }
        else
        {
            optionManager.DisabledOptions.Add(optionParent);
            optionManager.CreatedOptions.Remove(optionParent);
        }
        optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
    }
}
