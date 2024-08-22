using Godot;
using System;

public partial class RemoveButton : Button
{
	private Option optionParent;
	private OptionManager optionManager;

	public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
		optionManager = OptionManager.Instance;
		optionParent = (Option)GetParentControl().GetParentControl();
	}

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
