using Godot;
using System.Collections.Generic;

public partial class OptionManager : Node
{
    // --------------------------------
	//			VARIABLES	
    // --------------------------------

	private List<Option> createdOptions = new List<Option>();
	private List<Option> disabledOptions = new List<Option>();

    [Export]
    private Color primaryColor;
    [Export]
    private Color secondaryColor;

	[Export]
	private ColorRect applicationBackground;

	private bool wheelSpinning;
	private WheelProgress wheelProgressParent;

    // --------------------------------
    //			PROPERTIES	
    // --------------------------------

    public static OptionManager Instance { get; private set; }
    public List<Option> CreatedOptions {  get { return createdOptions; } }
	public List<Option> DisabledOptions { get { return disabledOptions; } }

	public Color PrimaryColor 
	{
		get => primaryColor; set => primaryColor = value;
	}
	public Color SecondaryColor 
	{ 
		get => secondaryColor; set => secondaryColor = value;
	}

	public ColorRect ApplicationBackground { get => applicationBackground; }

	public bool WheelSpinning { get => wheelSpinning; set => wheelSpinning = value; }
	public WheelProgress WheelProgressParent { get => wheelProgressParent; set => wheelProgressParent = value; }

    // --------------------------------
    //		STANDARD FUNCTIONS
    // --------------------------------

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}

    // --------------------------------
    //		CUSTOMIZATION LOGIC
    // --------------------------------

	public void UpdateWheelColors()
	{
		foreach(Option option in CreatedOptions)
		{
			option.OptionProgressBar.AssignBarColor();
		}
	}

}
