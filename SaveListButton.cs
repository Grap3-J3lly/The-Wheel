using Godot;
using System;

public partial class SaveListButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

	private GameManager gameManager;

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
		gameManager = GameManager.Instance;
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
		string name = listNameField.Text;
		if(name == "")
		{
			name = listOptions.Text;
			GD.Print(name);
		}
		return name;
	}

	/// <summary>
	/// Grabs the list name, calls on the save game logic in OptionManager, and reloads the lists to display the newly saved list
	/// </summary>
	public void SaveList()
	{
		gameManager.ListName = GetListName();
		gameManager.SaveGame();
		listOptions.LoadLists();
	}
}
