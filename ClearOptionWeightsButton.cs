using Godot;

public partial class ClearOptionWeightsButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private OptionManager optionManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		optionManager = OptionManager.Instance;
		this.Pressed += OnButtonPress;
	}

    // --------------------------------
    //		    BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Resets the weight of all options, disabled or otherwise to their default value (1)
    /// </summary>
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
