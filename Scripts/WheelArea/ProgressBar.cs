using Godot;

public partial class ProgressBar : TextureProgressBar
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    CustomizationManager customizationManager;

    [Export]
	private float rotationOffset = -90;
	private RichTextLabel optionName;
	private Option assignedOption;
    private static Color previousColor;
	private GameManager gameManager;

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
		gameManager = GameManager.Instance;
        customizationManager = CustomizationManager.Instance;
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
		float rotation = (RadialFillDegrees/2) + RadialInitialAngle + rotationOffset;
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
        // Use 2 Previous Colors instead of one
        // Assign Previous Color beforeHand? 
        Color primary = customizationManager.CurrentColors.GetColor(ColorPalette.Colors.WheelPrimary);
        Color secondary = customizationManager.CurrentColors.GetColor(ColorPalette.Colors.WheelSecondary);
        if (previousColor == Color.Color8(0, 0, 0, 0) || previousColor == secondary)
        {
            previousColor = primary;
            AssignBarColor(previousColor);
        }
        else
        {
            previousColor = secondary;
            AssignBarColor(previousColor);
        }
    }

    /// <summary>
    /// Assigns the color of the bar to the given value, and assigns the text to be the opposite value (primary vs secondary colors)
    /// </summary>
    /// <param name="color"></param>
	public void AssignBarColor(Color color)
	{
		TintProgress = color;
        Color primary = customizationManager.CurrentColors.GetColor(ColorPalette.Colors.WheelPrimary);
        Color secondary = customizationManager.CurrentColors.GetColor(ColorPalette.Colors.WheelSecondary);

        if (TintProgress == primary)
        {
            AssignBarTextColor(secondary);
        }
        else
        {
            AssignBarTextColor(primary);
        }
    }
}
