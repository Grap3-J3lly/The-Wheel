using Godot;
using System.Collections.Generic;

public partial class ListOptions : OptionButton
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    OptionManager optionManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		optionManager = OptionManager.Instance;
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
		List<string> saveFiles = optionManager.GetSaveFiles();

		this.Clear();

		foreach (string file in saveFiles) 
		{
			this.AddItem(file.Substring(0, file.LastIndexOf('.')));
		}
	}
}
