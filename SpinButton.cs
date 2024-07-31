using Godot;
using System;

public partial class SpinButton : Button 
{
	[Export]
	// Node2D wheel;
	Sprite2D wheel;


	bool startRotation = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		this.Pressed += PressButton;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (startRotation)
		{
			GD.Print("entered if statement");
			GD.Print(wheel.RotationDegrees);
			wheel.RotationDegrees += 1;
		}
	}

	private void PressButton()
	{
		GD.Print("Spinning Wheel!");
		startRotation = true;
	}
}
