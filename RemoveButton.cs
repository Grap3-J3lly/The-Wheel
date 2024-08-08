using Godot;
using System;

public partial class RemoveButton : Button
{
	private Option optionParent;
	private OptionManager optionManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
		optionManager = OptionManager.Instance;
		optionParent = (Option)GetParentControl().GetParentControl();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void PressButton()
	{
        if (optionManager.WheelSpinning || optionManager.CreatedOptions.Count <= 1) { return; }

		optionManager.CreatedOptions.Remove(optionParent);
		Option.DeleteOption(optionParent);

		optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
	}
}
