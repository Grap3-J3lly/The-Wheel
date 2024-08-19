using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class OptionManager : Node
{
    // --------------------------------
	//			VARIABLES	
    // --------------------------------

	private List<Variant> dataToSave = new List<Variant>();
	private List<Color> colors = new List<Color>();
	private List<Option> createdOptions = new List<Option>();
	private List<Option> disabledOptions = new List<Option>();

	private string listName = "New List";

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

    // --------------------------------
    //			PROPERTIES	
    // --------------------------------

    public static OptionManager Instance { get; private set; }

	public List<Color> Colors { get => colors; set => colors = value; }
    public List<Option> CreatedOptions {  get { return createdOptions; } }
	public List<Option> DisabledOptions { get { return disabledOptions; } }

	public string ListName { get => listName; set => listName = value; }

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

    // --------------------------------
    //		SAVE/LOAD LOGIC
    // --------------------------------

    // Save/Load in Godot:
	// https://docs.godotengine.org/en/stable/tutorials/io/saving_games.html

    private void PopulateDataToSave()
	{
		List<Option> allOptions = new List<Option>();
		allOptions.AddRange(createdOptions);
		allOptions.AddRange(disabledOptions);

		Color[] colorsArray = colors.ToArray();
		Array optionsArray = new Array();

		foreach (Option option in allOptions)
		{
			optionsArray.Add(option.GetOptionData());
		}

		dataToSave.Add(ListName);
		dataToSave.Add(colorsArray);
		dataToSave.Add(optionsArray);
	}

	public void SaveGame()
	{
		PopulateDataToSave();
        using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);

		Array dataToSaveArray = new Array(dataToSave.ToArray());

		var jsonData = Json.Stringify(dataToSaveArray, "\t");
		saveFile.StoreLine(jsonData);
	}
}