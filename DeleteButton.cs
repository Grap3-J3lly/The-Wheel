using Godot;
using System;

public partial class DeleteButton : Button
{
	[Export]
	private ListOptions listOptions;

	public override void _Ready()
	{
		base._Ready();
		this.Pressed += OnButtonPress;
	}

	private void OnButtonPress()
	{
		string fileName = "user://" + listOptions.Text + ".save";
		DirAccess.RemoveAbsolute(fileName);
		listOptions.LoadLists();
	}
}
