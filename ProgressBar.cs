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

    public void SetName(string name)
	{
		optionName.Text = "[right]" + name + "[/right]";
	}

	public void AssignTextRotation()
	{
		float rotation = (this.RadialFillDegrees/2) + this.RadialInitialAngle + rotationOffset;
		optionName.RotationDegrees = rotation;
	}

    public void AssignBarTextColor(Color color)
    {
        optionName.AddThemeColorOverride("default_color", color);
    }

    // --------------------------------
    //		COLOR FUNCTIONS	
    // --------------------------------

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
