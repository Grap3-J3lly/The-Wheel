using Godot;

public partial class ClearOptionWeightsButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private GameManager gameManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		gameManager = GameManager.Instance;
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
		foreach (Option option in gameManager.CreatedOptions)
		{
            option.ResetDefaultWeight();
        }
		foreach(Option option in gameManager.DisabledOptions)
		{
            option.ResetDefaultWeight();
        }
	}	
}
