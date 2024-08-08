using Godot;
using System;

public partial class OptionWeight : LineEdit
{
	OptionManager optionManager;
	Option optionParent;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		optionManager = OptionManager.Instance;
		optionParent = (Option)GetParentControl().GetParentControl();
		
		this.TextChanged += UpdateOptionWeight;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        this.Editable = !optionManager.WheelSpinning && optionParent.OptionEnabled;
	}

	public void UpdateOptionWeight(string newText)
	{
		if(int.TryParse(this.Text, out int result))
		{
			optionParent.OptionWeight = result;
		}
        optionManager.WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
    }
}
