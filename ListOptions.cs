using Godot;
using System.Collections.Generic;

public partial class ListOptions : OptionButton
{
	OptionManager optionManager;

	public override void _Ready()
	{
		optionManager = OptionManager.Instance;
		LoadLists();
	}

	private void LoadLists()
	{
		List<string> saveFiles = optionManager.GetSaveFiles();

		foreach (string file in saveFiles) 
		{
			this.AddItem(file.Substring(0, file.LastIndexOf('.')));
		}
	}
}
