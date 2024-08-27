using Godot;
using System;

public partial class SaveListButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

	private OptionManager optionManager;

    [Export]
	private LineEdit listNameField;
	[Export]
	private ListOptions listOptions;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		this.Pressed += SaveList;
	}

    // --------------------------------
    //			LIST LOGIC	
    // --------------------------------

	/// <summary>
	/// Grabs the text from the list name field
	/// </summary>
	/// <returns></returns>
    public string GetListName()
	{
		return listNameField.Text;
	}

	/// <summary>
	/// Grabs the list name, calls on the save game logic in OptionManager, and reloads the lists to display the newly saved list
	/// </summary>
	public void SaveList()
	{
		optionManager.ListName = GetListName();
		optionManager.SaveGame();
		listOptions.LoadLists();
	}
}
