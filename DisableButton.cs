using Godot;
using System;

public partial class DisableButton : Button
{
	[Export]
	private TextEdit optionNameField;
	[Export]
	private TextEdit optionWeightField;

	private OptionManager optionManager;
	private bool enabled = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		enabled = true;
		this.Pressed += PressButton;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void PressButton()
	{
        if (optionManager.WheelSpinning) { return; }

        enabled = !enabled;
		optionNameField.Editable = enabled;
		optionWeightField.Editable = enabled;
	}
}
