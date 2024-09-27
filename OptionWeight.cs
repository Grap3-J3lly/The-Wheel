using Godot;
using System;

public partial class OptionWeight : LineEdit
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private GameManager gameManager;
	[Export]
	private Option optionParent;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------
    public override void _Ready()
	{
		gameManager = GameManager.Instance;
		
		this.TextChanged += UpdateOptionWeight;
	}

	public override void _Process(double delta)
	{
        this.Editable = !gameManager.WheelSpinning && optionParent.OptionEnabled;
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
		if(int.TryParse(this.Text, out int result))
		{
			optionParent.OptionWeight = result;
		}
        gameManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
    }

	/// <summary>
	/// Assigns the text in the weight field to the value on the option, before refreshing the wheel
	/// </summary>
	public void UpdateOptionWeightField()
	{
		this.Text = optionParent.OptionWeight.ToString();
        gameManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
    }
}
