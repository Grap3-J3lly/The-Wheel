using Godot;
using System;

public partial class LoadListButton : Button
{
	[Export]
	private CustomizeButton customizeButton;
	[Export]
	private OptionButton loadListOptionButton;
	private OptionManager optionManager;

	public override void _Ready()
	{
		optionManager = OptionManager.Instance;
		this.Pressed += OnButtonPress;

	}

	private void OnButtonPress()
	{
		customizeButton.OnButtonPressed();
		optionManager.LoadGame(loadListOptionButton.Text);
	}
}
