using Godot;

public partial class SpinButton : Button 
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

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

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------
    public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		this.Pressed += PressButton;
		timer = 0.0d;
		slowDownValue = timer;
    }

	public override void _Process(double delta)
	{
		base._Process(delta);
		HandleWheelSpin(delta);
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

	/// <summary>
	/// If the wheel isn't spinning, rests the timer, grabs a random value to rotate the wheel by, and assigns necessary values to true to begin wheel spinning
	/// </summary>
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

	/// <summary>
	/// Runs on _Process, handles all wheel spinning logic, such as speeding up, slowing down, and selecting a winner
	/// </summary>
	/// <param name="delta"></param>
	private void HandleWheelSpin(double delta)
	{
		if(optionManager.WheelSpinning)
		{
            // Speeding up the wheel
            WheelSpeedingUp(delta);

			// Apex of wheel speed
			WheelAtApexSpeed();

			// Slowing down the wheel
			WheelSlowingDown(delta);
            
			// Wheel has come to a stop, determine Winner
            SelectingWinner();
        }
    }

    /// <summary>
    /// Handles logic for when the wheel is speeding up, such as updating the timer and the amount that the wheel is rotating
    /// </summary>
    /// <param name="delta"></param>
    private void WheelSpeedingUp(double delta)
	{
        if (startRotation && timer < maxTime)
        {
			timer += delta;
			wheel.RotationDegrees += speed * (float)timer;
        }
    }

	/// <summary>
	/// Handles the logic for when the wheel is at it's apex speed, such as updating the tracker values, storing the timer value and reseting the timer for later
	/// </summary>
	private void WheelAtApexSpeed()
	{
        if (timer >= maxTime)
        {
            startRotation = false;
            slowDownValue = timer;
            timer = 0;
        }
    }

	/// <summary>
	/// Handles the logic for when the wheel is slowing down, such as decreasing the rotation speed, and introducing the random value calculated earlier
	/// </summary>
	/// <param name="delta"></param>
	private void WheelSlowingDown(double delta)
	{
        if (slowDownValue > 0)
        {
            wheel.RotationDegrees += speed * (float)slowDownValue + (extraPerAngle * (float)delta);
            slowDownValue -= delta;
        }
    }

	/// <summary>
	/// Handles the logic for selecting a winner, such as deciding the winner and opening the popup
	/// </summary>
	private void SelectingWinner()
	{
        if (!startRotation && slowDownValue <= 0)
        {
            optionManager.WheelSpinning = false;
            string winnerName = DecideWinner();

            if (winnerName != "")
            {
                Panel result = PopupManager.Instance.CreatePopup(PopupManager.Instance.SelectedOptionPopup);
                PopupManager.Instance.AssignWinningText(result, winnerName);
            }
            else
            {
                GD.PushError(winnerName, "DecideWinner() not returning correct name value");
            }
        }
    }

    /// <summary>
    /// Determines the winning option by running through all options and determining which rotation overlaps with the wheel's final rotation
    /// </summary>
    /// <returns>The name of the winnning option</returns>
    private string DecideWinner()
	{
		float currentRotationDegrees = 360 - (wheel.RotationDegrees % 360);
		foreach(Option option in optionManager.CreatedOptions)
		{
			float initialAngle = option.OptionProgressBar.RadialInitialAngle;
			float finalAngle = initialAngle + option.OptionProgressBar.RadialFillDegrees;
			if(currentRotationDegrees > initialAngle && currentRotationDegrees < finalAngle)
			{
				return option.OptionName;
			}
		}
		return "";
	}
}
