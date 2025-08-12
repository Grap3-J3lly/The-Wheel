using Godot;

public partial class DeleteListButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    [Export]
	private ListOptions listOptions;

    private const string CONST_FilePrefix = "user://";
    private const string CONST_FilePostfix = ".saved";

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
		string fileName = CONST_FilePrefix + listOptions.Text + CONST_FilePostfix;
		DirAccess.RemoveAbsolute(fileName);
		listOptions.LoadLists();
	}
}
