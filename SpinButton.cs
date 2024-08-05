using Godot;
using System;

public partial class SpinButton : Button 
{
	[Export]
	private Control wheel;
	[Export]
	private double maxTime = 3d;
	[Export]
	private float speed = .75f;

	private OptionManager optionManager;	
	private double timer;
	private double slowDownValue;
	private bool startRotation = false;
	private float randomAngle;
	private float extraPerAngle = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		this.Pressed += PressButton;
		timer = 0.0d;
		slowDownValue = timer;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (startRotation && timer < maxTime)
		{
			timer += delta;
			wheel.RotationDegrees += speed * (float)timer;
		}
		if (timer >= maxTime)
		{
            startRotation = false;
            slowDownValue = timer;
			timer = 0;
        }
		if(slowDownValue > 0)
		{
            wheel.RotationDegrees += speed * (float)slowDownValue + (extraPerAngle * (float)delta);
            slowDownValue -= delta;
        }
		if(!startRotation && slowDownValue <= 0)
		{
			optionManager.WheelSpinning = false;
		}

	}

	private void PressButton()
	{
		if(optionManager.WheelSpinning) { return; }
		timer = 0d;
		startRotation = true;

        RandomNumberGenerator rand = new RandomNumberGenerator();
        randomAngle = rand.RandfRange(0, 360);
		extraPerAngle = randomAngle / (float)maxTime;

		optionManager.WheelSpinning = true;
    }
}
