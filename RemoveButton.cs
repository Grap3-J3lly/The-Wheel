using Godot;
using System;

public partial class RemoveButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private Option optionParent;
	private OptionManager optionManager;

    // --------------------------------
    //			STANDARD LOGIC	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
		optionManager = OptionManager.Instance;
		optionParent = (Option)GetParentControl();
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

	/// <summary>
	/// If the wheel isn't spinning, removes the current button from whichever list it's in (disabled or otherwise), before deleting the entire option and refreshing the wheel
	/// </summary>
    private void PressButton()
	{
        if (optionManager.WheelSpinning || optionManager.CreatedOptions.Count <= 1) { return; }

		if(optionManager.CreatedOptions.Contains(optionParent))
		{
			optionManager.CreatedOptions.Remove(optionParent);
		}	
		if(optionManager.DisabledOptions.Contains(optionParent))
		{
			optionManager.DisabledOptions.Remove(optionParent);
		}

		Option.DeleteOption(optionParent);

		optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
	}
}
