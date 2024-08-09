using Godot;
using System;

public partial class ProgressBar : TextureProgressBar
{
	[Export]
	private float rotationOffset = -90;
	private RichTextLabel optionName;
	private Option assignedOption;

	public Option AssignedOption { get => assignedOption; set => assignedOption = value; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		optionName = GetChild<RichTextLabel>(0);
	}

	public void SetName(string name)
	{
		optionName.Text = "[right]" + name + "[/right]";
	}

	public void AssignTextRotation()
	{
		float rotation = (this.RadialFillDegrees/2) + this.RadialInitialAngle + rotationOffset;
		optionName.RotationDegrees = rotation;
	}
}
