using Godot;
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

	private	OptionManager optionManager;
	private Color previousColor;
	private List<Option> options = new List<Option>();

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		optionManager.WheelProgressParent = this;
		options = optionManager.CreatedOptions;
				
		WheelProgressUpdate += CreateProgressBars;
	}

    // --------------------------------
    //		EVENT CALL FUNCTIONS	
    // --------------------------------

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
				TextureProgressBar newBar = (TextureProgressBar)progressBarTemplate.Instantiate();
				AddChild(newBar);
				ProgressBar progressBar = (ProgressBar)newBar;
				options[i].OptionProgressBar = progressBar;
				progressBar.AssignedOption = options[i];
				progressBar.SetName(options[i].OptionName);

				newBar.RadialFillDegrees = (360.0f / Option.GetTotalWeight(options)) * options[i].OptionWeight;
				newBar.RadialInitialAngle = previousAngle;
				previousAngle += newBar.RadialFillDegrees;
				progressBar.AssignTextRotation();

				if(i == options.Count - 1 && options.Count % 2 == 1)
				{
					newBar.RadialFillDegrees -= .5f;
				}

				if (previousColor == Color.Color8(0,0,0,0) || previousColor == optionManager.SecondaryColor)
				{
					previousColor = optionManager.PrimaryColor;
					newBar.TintProgress = optionManager.PrimaryColor;
				}
				else
				{
					previousColor = optionManager.SecondaryColor;
					newBar.TintProgress = optionManager.SecondaryColor;
				}
			}
		}
	}
}
