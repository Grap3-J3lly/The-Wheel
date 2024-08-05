using Godot;
using System.Collections.Generic;

public partial class WheelProgress : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Signal]
	public delegate void WheelProgressUpdateEventHandler(TextureProgressBar targetBar);

	[Export]
	private PackedScene progressBarTemplate;

	private	OptionManager optionManager;
	private Color previousColor;
	private List<TextureProgressBar> progressBars = new List<TextureProgressBar>();

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		optionManager.WheelProgressParent = this;
		progressBars = optionManager.CreatedProgressBars;
				
		WheelProgressUpdate += CreateProgressBars;
	}

    // --------------------------------
    //		EVENT CALL FUNCTIONS	
    // --------------------------------

    private void CreateProgressBars(TextureProgressBar targetBar)
	{
		if (progressBars == null) { return; }


        // If less progress bars are needed
        if (progressBars.Count > optionManager.CreatedOptions.Count)
        {
            // Remove Option from Wheel
            GD.Print("Called During RemoveButton");

            optionManager.CreatedProgressBars.Remove(targetBar);
            targetBar.QueueFree();
        }

		// If more progress bars are needed
        if (progressBars.Count <= optionManager.CreatedOptions.Count)
		{
			if (progressBars.Count > 0)
			{
				foreach (var progressBar in progressBars)
				{
					progressBar.QueueFree();
				}
				progressBars.Clear();
			}

			// Assign Options Initial Angle = Fill Degree * Option Number (using array indices)
			// Reduce Fill Degree for every option on wheel to evenly distribute across wheel
            for (int i = 0; i < optionManager.CreatedOptions.Count; i++)
			{
				TextureProgressBar newBar = (TextureProgressBar)progressBarTemplate.Instantiate();
				AddChild(newBar);
				progressBars.Add(newBar);
				newBar.RadialFillDegrees = 360.0f / optionManager.CreatedOptions.Count;
				newBar.RadialInitialAngle = newBar.RadialFillDegrees * i;

				if(i == optionManager.CreatedOptions.Count - 1 && optionManager.CreatedOptions.Count % 2 == 1)
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
