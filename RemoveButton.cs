using Godot;
using System;

public partial class RemoveButton : Button
{
	[Export]
	private Control optionParent;
	private OptionManager optionManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
		optionManager = OptionManager.Instance;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void PressButton()
	{
        if (optionManager.WheelSpinning || optionManager.CreatedOptions.Count <= 1) { return; }
		
		GD.Print(optionManager.CreatedProgressBars.Count);
		TextureProgressBar targetBar = optionManager.CreatedProgressBars[optionManager.CreatedOptions.IndexOf(optionParent)];

		// Remove Option from List of Options
        optionManager.CreatedOptions.Remove(optionParent);
		optionParent.QueueFree();

		optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, targetBar);
	}
}
