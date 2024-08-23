using Godot;
using System;

public partial class SaveListButton : Button
{
	[Export]
	private LineEdit listNameField;

	[Export]
	private ListOptions listOptions;

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
		listOptions.LoadLists();
	}
}
