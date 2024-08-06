using Godot;
using System;

public partial class OptionName : TextEdit
{
	OptionManager optionManager;
	Option optionParent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		optionManager = OptionManager.Instance;
        optionParent = (Option)GetParentControl().GetParentControl();

        this.TextChanged += UpdateOptionName;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        this.Editable = !optionManager.WheelSpinning && optionParent.OptionEnabled;
    }

	private void UpdateOptionName()
	{
		optionParent.OptionName = this.Text;
	}
}
