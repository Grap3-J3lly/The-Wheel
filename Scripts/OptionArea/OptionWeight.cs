using Godot;

public partial class OptionWeight : LineEdit
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private GameManager gameManager;
	[Export]
	private Option optionParent;
	[Export]
	private ColorRect weightBackground;
	[Export]
	private Color defaultColor;
	[Export]
	private Color disabledColor;

	private const bool CONST_WheelNeedsReset = true;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------
    public override void _Ready()
	{
		gameManager = GameManager.Instance;
		
		TextChanged += UpdateOptionWeight;
	}

	public override void _Process(double delta)
	{
        Editable = !gameManager.WheelSpinning && optionParent.OptionEnabled;
		ChangeColor(optionParent.OptionEnabled);
	}

	private void ChangeColor(bool isEnabled)
	{
		if (isEnabled)
		{
			weightBackground.Color = defaultColor;
			return;
		}
		weightBackground.Color = disabledColor;
		
	}

    // --------------------------------
    //			WEIGHT LOGIC	
    // --------------------------------

	/// <summary>
	/// Attempts to assign the text in the weight field as an integer, before refreshing the wheel
	/// </summary>
	/// <param name="newText"></param>
    public void UpdateOptionWeight(string newText)
	{
		if(int.TryParse(Text, out int result))
		{
			optionParent.OptionWeight = result;
		}
        gameManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, CONST_WheelNeedsReset);
    }

	/// <summary>
	/// Assigns the text in the weight field to the value on the option, before refreshing the wheel
	/// </summary>
	public void UpdateOptionWeightField()
	{
		Text = optionParent.OptionWeight.ToString();
        gameManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, CONST_WheelNeedsReset);
    }
}
