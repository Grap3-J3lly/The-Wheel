using Godot;
using System;

public partial class SaveListButton : Button
{
	[Export]
	private LineEdit listNameField;

	private OptionManager optionManager;

	public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		this.Pressed += SaveList;
	}

	public string GetListName()
	{
		return listNameField.Text;
	}

	public void SaveList()
	{
		optionManager.ListName = GetListName();
		optionManager.SaveGame();
	}
}
