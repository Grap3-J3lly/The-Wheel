using Godot;

public partial class DisableButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private Option optionParent;

	private GameManager gameManager;
	private bool enabled = true;
    [Export]
    private TextureRect checkmark;

    // --------------------------------
    //		    PROPERTIES
    // --------------------------------

    public bool Enabled { get => enabled; set => enabled = value; }

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		gameManager = GameManager.Instance;
		enabled = true;
        ButtonPressed = enabled;
        optionParent = (Option)GetParentControl();
		this.Pressed += PressButton;
	}

    // --------------------------------
    //		    BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Enables or disables assigned option based on current enabled value, moving the option to the correct list for tracking, and refreshes the wheel
    /// </summary>
    private void PressButton()
	{
        if (gameManager.WheelSpinning) { return; }

        enabled = !enabled;
        optionParent.OptionEnabled = enabled;
        
        if(enabled)
        {
            checkmark.Modulate = new Color(255, 255, 255, 1);
            gameManager.DisabledOptions.Remove(optionParent);
            gameManager.CreatedOptions.Add(optionParent);
        }
        else
        {
            checkmark.Modulate = new Color(255, 255, 255, 0);
            gameManager.DisabledOptions.Add(optionParent);
            gameManager.CreatedOptions.Remove(optionParent);
        }
        gameManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
        ButtonPressed = enabled;
    }
}
