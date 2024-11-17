using Godot;
using System;

public partial class OptionName : LineEdit
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private GameManager gameManager;

    [Export]
    private Option optionParent;
    [Export]
    private ColorRect nameBackground;
    [Export]
    private Color defaultColor;
    [Export]
    private Color disabledColor;


    // --------------------------------
    //		STANDARD LOGIC	
    // --------------------------------

    public override void _Ready()
	{
		gameManager = GameManager.Instance;
        this.TextChanged += UpdateOptionName;
    }

	public override void _Process(double delta)
	{
        this.Editable = !gameManager.WheelSpinning && optionParent.OptionEnabled;
        ChangeColor(optionParent.OptionEnabled);
    }

    private void ChangeColor(bool isEnabled)
    {
        if (isEnabled)
        {
            nameBackground.Color = defaultColor;
            return;
        }
        nameBackground.Color = disabledColor;
    }

    // --------------------------------
    //			TEXT LOGIC	
    // --------------------------------

    /// <summary>
    /// Attached to the TextChanged listener, this assigns the Options name value based off the current text value. Also updates the text on the assigned ProgressBar
    /// </summary>
    public void UpdateOptionName(string newText)
	{
		optionParent.OptionName = this.Text;
		optionParent.OptionProgressBar.SetName(this.Text);
	}

    /// <summary>
    /// Updates the text field to match the option's value, and updates the assigned ProgressBar as well
    /// </summary>
	public void UpdateOptionNameField()
	{
		this.Text = optionParent.OptionName;
        optionParent.OptionProgressBar.SetName(this.Text);
    }
}
