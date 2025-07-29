using Godot;

public partial class RemoveOptionButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private Option optionParent;
	private GameManager gameManager;

    // --------------------------------
    //			STANDARD LOGIC	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
		gameManager = GameManager.Instance;
		optionParent = (Option)GetParentControl();
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

	/// <summary>
	/// If the wheel isn't spinning, removes the current button from whichever list it's in (disabled or otherwise), before deleting the entire option and refreshing the wheel
	/// </summary>
    private void PressButton()
	{
        if (gameManager.WheelSpinning) { return; }

		if (gameManager.CreatedOptions.Count > 1)
		{
            if (gameManager.CreatedOptions.Contains(optionParent))
            {
                gameManager.CreatedOptions.Remove(optionParent);
            }
            if (gameManager.DisabledOptions.Contains(optionParent))
            {
                gameManager.DisabledOptions.Remove(optionParent);
            }

            Option.DeleteOption(optionParent);

            gameManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
        }
        else
        {
            optionParent.OptionName = "";
            optionParent.ResetDefaultWeight();
        }
	}
}
