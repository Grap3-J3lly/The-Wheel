using Godot;
using System;

public partial class LoadListButton : Button
{
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
		optionManager.LoadGame(loadListOptionButton.Text);
	}
}
