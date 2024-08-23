using Godot;
using System;

public partial class OptionName : TextEdit
{
	OptionManager optionManager;
	Option optionParent;
	
	public override void _Ready()
	{
		optionManager = OptionManager.Instance;
        optionParent = (Option)GetParentControl().GetParentControl();

        this.TextChanged += UpdateOptionName;
    }

	public override void _Process(double delta)
	{
        this.Editable = !optionManager.WheelSpinning && optionParent.OptionEnabled;
    }

	public void UpdateOptionName()
	{
		optionParent.OptionName = this.Text;
		optionParent.OptionProgressBar.SetName(this.Text);
	}

	public void UpdateOptionNameField()
	{
		this.Text = optionParent.OptionName;
        optionParent.OptionProgressBar.SetName(this.Text);
    }
}
