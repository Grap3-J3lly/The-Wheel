using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class WheelProgress : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Signal]
	public delegate void WheelProgressUpdateEventHandler(bool needsReset);

	[Export]
	private PackedScene progressBarTemplate;

	private	GameManager gameManager;
	private Array<Option> options = new Array<Option>();

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		gameManager = GameManager.Instance;
		gameManager.WheelProgressParent = this;
		options = gameManager.CreatedOptions;
				
		WheelProgressUpdate += CreateProgressBars;
	}

    // --------------------------------
    //		EVENT CALL FUNCTIONS	
    // --------------------------------

	/// <summary>
	/// Creates Progress Bar objects, evaluating the necessary size, rotation, and color, before adding them to the correct option object
	/// </summary>
	/// <param name="needsReset"></param>
    private void CreateProgressBars(bool needsReset)
	{
		if (options == null) { return; }

		// If more progress bars are needed
        if (Option.CheckForMissingProgressBar(options) || needsReset)
		{
			Option.RemoveAllProgressBars(options);

			// Assign Options Initial Angle = Fill Degree * Option Number (using array indices)
			// Reduce Fill Degree for every option on wheel to evenly distribute across wheel

			float previousAngle = 0f;
            for (int i = 0; i < options.Count; i++)
			{
				ProgressBar progressBar = (ProgressBar)progressBarTemplate.Instantiate();
				AddChild(progressBar);
				options[i].OptionProgressBar = progressBar;
				progressBar.AssignedOption = options[i];
				progressBar.SetName(options[i].OptionName);

				progressBar.RadialFillDegrees = (360.0f / Option.GetTotalWeight(options)) * options[i].OptionWeight;
				progressBar.RadialInitialAngle = previousAngle;
				previousAngle += progressBar.RadialFillDegrees;
				progressBar.AssignTextRotation();

				// Gives a slight separation between options when necessary
				if(i == options.Count - 1 && options.Count % 2 == 1)
				{
					progressBar.RadialFillDegrees -= .5f;
				}

				// Handles coloring the wheel slices
				progressBar.AssignBarColor();
			}
		}
	}
}
