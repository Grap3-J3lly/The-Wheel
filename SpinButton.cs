using Godot;
using System;

public partial class SpinButton : Button
{
	[Export]
	Node wheel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += ButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ButtonPressed()
	{	
		GD.Print("Spinning Wheel!");
		
	}
}
