using Godot;
using System;

public partial class ClearOptionWeightsButton : Button
{
	private OptionManager optionManager;

	public override void _Ready()
	{
		optionManager = OptionManager.Instance;
		this.Pressed += OnButtonPress;
	}

	private void OnButtonPress()
	{
		foreach (Option option in optionManager.CreatedOptions)
		{
            option.ResetDefaultWeight();
        }
		foreach(Option option in optionManager.DisabledOptions)
		{
            option.ResetDefaultWeight();
        }
	}	
}
