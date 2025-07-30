using Godot;
using Godot.Collections;

public partial class ListOptions : OptionButton
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    GameManager gameManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		gameManager = GameManager.Instance;
		LoadLists();
	}

    // --------------------------------
    //			LOAD LOGIC	
    // --------------------------------

    /// <summary>
    /// Populates the list options on this object using all appropriate save files
    /// </summary>
    public void LoadLists()
	{
        Array<string> saveFiles = gameManager.GetSaveFiles();

        Clear();

        foreach (string file in saveFiles)
        {
            AddItem(file.Substring(0, file.LastIndexOf('.')));
        }
    }
}
