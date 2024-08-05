using Godot;
using System;

public partial class OptionWeight : TextEdit
{
	OptionManager optionManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		optionManager = OptionManager.Instance;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        this.Editable = !optionManager.WheelSpinning && GetNode<DisableButton>("../DisableButton").Enabled;
	}
}
