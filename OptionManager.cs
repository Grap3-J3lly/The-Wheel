using Godot;
using System.Collections.Generic;

public partial class OptionManager : Node
{
    // --------------------------------
	//			VARIABLES	
    // --------------------------------

	private List<Option> createdOptions = new List<Option>();
	private List<Option> disabledOptions = new List<Option>();

	private bool wheelSpinning;
	private WheelProgress wheelProgressParent;

	// Customizable UI Elements
    [Export]
    private Color primaryColor;
    [Export]
    private Color secondaryColor;
	[Export]
	private ColorRect applicationBackground;
	[Export]
	private ColorRect listBackground;
	[Export]
	private SpinButton spinButton;
	[Export]
	private Theme listFontTheme;

    // --------------------------------
    //			PROPERTIES	
    // --------------------------------

    public static OptionManager Instance { get; private set; }
    public List<Option> CreatedOptions {  get { return createdOptions; } }
	public List<Option> DisabledOptions { get { return disabledOptions; } }
	public bool WheelSpinning { get => wheelSpinning; set => wheelSpinning = value; }
	public WheelProgress WheelProgressParent { get => wheelProgressParent; set => wheelProgressParent = value; }

    // Customizable UI Elements
    public Color PrimaryColor 
	{
		get => primaryColor; set => primaryColor = value;
	}
	public Color SecondaryColor 
	{ 
		get => secondaryColor; set => secondaryColor = value;
	}
	public ColorRect ApplicationBackground { get => applicationBackground; }
	public ColorRect ListBackground { get => listBackground; }
	public SpinButton SpinButton { get => spinButton; }
	public Theme ListFontTheme {  get => listFontTheme; }

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

	public Color GetFontColor()
	{
		Color fontColor = (Color)listFontTheme.Get("LineEdit/colors/font_color");
		GD.Print(fontColor);
		return fontColor;
	}

	public void SetFontColor(Color color)
	{
		listFontTheme.Set("LineEdit/colors/font_color", color);
		listFontTheme.Set("TextEdit/colors/font_color", color);
		listFontTheme.Set("TextEdit/colors/font_placeholder_color", new Color(color, color.A * .6f));
		listFontTheme.Set("RichTextLabel/colors/default_color", color);
	}
}
