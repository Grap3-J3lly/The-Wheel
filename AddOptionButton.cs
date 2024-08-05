using Godot;
using System.Collections.Generic;

public partial class AddOptionButton : Button
{
	[Export]
	private PackedScene optionTemplate;
	[Export]
	private Control optionParent;
	private List<Control> createdOptions = new List<Control>();
	private OptionManager optionManager;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
		optionManager = OptionManager.Instance;

		// Create Default Option
		PressButton();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void PressButton()
	{
		if(optionManager.WheelSpinning) { return; }

		Control newControl = (Control)optionTemplate.Instantiate();
		optionParent.AddChild(newControl);
		optionManager.CreatedOptions.Add(newControl);
		optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, new TextureProgressBar());
	}
}
