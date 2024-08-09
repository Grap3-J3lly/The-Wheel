using Godot;
using System;

public partial class WinnerSelection : TextureRect
{
	RayCast2D ray;

	public override void _Ready()
	{
		ray = GetChild<RayCast2D>(0);
	}

	
	public override void _Process(double delta)
	{
	}

	public string OnWheelStop()
	{
		Area2D result = (Area2D)ray.GetCollider();
		return result.GetParent<ProgressBar>().AssignedOption.OptionName;

	}
}
