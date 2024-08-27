using Godot;
using System;

public partial class OptionName : TextEdit
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private OptionManager optionManager;
	private Option optionParent;

    // --------------------------------
    //		STANDARD LOGIC	
    // --------------------------------

    public override void _Ready()
	{
		optionManager = OptionManager.Instance;
        optionParent = (Option)GetParentControl();

        this.TextChanged += UpdateOptionName;
    }

	public override void _Process(double delta)
	{
        this.Editable = !optionManager.WheelSpinning && optionParent.OptionEnabled;
    }

    // --------------------------------
    //			TEXT LOGIC	
    // --------------------------------

    /// <summary>
    /// Attached to the TextChanged listener, this assigns the Options name value based off the current text value. Also updates the text on the assigned ProgressBar
    /// </summary>
    public void UpdateOptionName()
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
