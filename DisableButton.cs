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

    public bool Enabled { get=>  enabled;}

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		enabled = true;
        optionParent = (Option)GetParentControl().GetParentControl();
		this.Pressed += PressButton;
	}

    // --------------------------------
    //		INPUT FUNCTIONS	
    // --------------------------------

    private void PressButton()
	{
        if (optionManager.WheelSpinning) { return; }

        enabled = !enabled;
        optionParent.OptionEnabled = enabled;
        
        if(enabled)
        {
            optionManager.DisabledOptions.Remove(optionParent);
            optionManager.CreatedOptions.Add(optionParent);
            optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
        }
        else
        {
            TextureProgressBar targetBar = optionParent.OptionProgressBar;

            optionManager.DisabledOptions.Add(optionParent);
            optionManager.CreatedOptions.Remove(optionParent);

            optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
        }

        
    }
}
