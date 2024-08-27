using Godot;
using System;

public partial class ProgressBar : TextureProgressBar
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Export]
	private float rotationOffset = -90;
	private RichTextLabel optionName;
	private Option assignedOption;
    private static Color previousColor;
	private OptionManager optionManager;

    // --------------------------------
    //			PROPERTIES	
    // --------------------------------
    public Option AssignedOption { get => assignedOption; set => assignedOption = value; }
    public Color PreviousColor { get => previousColor; set => previousColor = value; }

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		optionManager = OptionManager.Instance;
		optionName = GetChild<RichTextLabel>(0);
	}

    // --------------------------------
    //		TEXT FUNCTIONS	
    // --------------------------------

    /// <summary>
    /// Assigns text to the text field on the Progress Bar and aligns it to the right side
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
	{
		optionName.Text = "[right]" + name + "[/right]";
	}

    /// <summary>
    /// Controls the rotation of the text field to match the rotation of the Progress Bar as a whole
    /// </summary>
	public void AssignTextRotation()
	{
		float rotation = (this.RadialFillDegrees/2) + this.RadialInitialAngle + rotationOffset;
		optionName.RotationDegrees = rotation;
	}

    /// <summary>
    /// Assigns the color of the text field to a given value
    /// </summary>
    /// <param name="color"></param>
    public void AssignBarTextColor(Color color)
    {
        optionName.AddThemeColorOverride("default_color", color);
    }

    // --------------------------------
    //		COLOR FUNCTIONS	
    // --------------------------------

    /// <summary>
    /// Assigns the color of the Bar based on the previous color (primary vs secondary)
    /// </summary>
    public void AssignBarColor()
	{
        if (previousColor == Color.Color8(0, 0, 0, 0) || previousColor == optionManager.SecondaryColor)
        {
            previousColor = optionManager.PrimaryColor;
            AssignBarColor(previousColor);
        }
        else
        {
            previousColor = optionManager.SecondaryColor;
            AssignBarColor(previousColor);
        }
    }

    /// <summary>
    /// Assigns the color of the bar to the given value, and assigns the text to be the opposite value (primary vs secondary colors)
    /// </summary>
    /// <param name="color"></param>
	public void AssignBarColor(Color color)
	{
		this.TintProgress = color;

        if (this.TintProgress == optionManager.PrimaryColor)
        {
            AssignBarTextColor(optionManager.SecondaryColor);
        }
        else
        {
            AssignBarTextColor(optionManager.PrimaryColor);
        }
    }
}
