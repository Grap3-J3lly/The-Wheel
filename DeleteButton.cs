using Godot;
using System;

public partial class DeleteButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    [Export]
	private ListOptions listOptions;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		this.Pressed += OnButtonPress;
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Deletes the list currently selected in the assigned list option element
    /// </summary>
    private void OnButtonPress()
	{
		string fileName = "user://" + listOptions.Text + ".save";
		DirAccess.RemoveAbsolute(fileName);
		listOptions.LoadLists();
	}
}
