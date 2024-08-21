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

	// Option Data
    [Export]
    private PackedScene optionTemplate;
    [Export]
    private Control optionParent;

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

	// Option Data
	public PackedScene OptionTemplate { get => optionTemplate; }
	public Control OptionParent { get => optionParent; }

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
        using var saveFile = FileAccess.Open("user://" + listName + ".save", FileAccess.ModeFlags.Write);

		Array dataToSaveArray = new Array(dataToSave.ToArray());

		var jsonData = Json.Stringify(dataToSaveArray);
		saveFile.StoreLine(jsonData);
	}

	public void LoadGame(string specificFile)
	{
		DeleteOptions(createdOptions);
		DeleteOptions(disabledOptions);

        using var saveFile = FileAccess.Open("user://" + specificFile + ".save", FileAccess.ModeFlags.Read);

		while (saveFile.GetPosition() < saveFile.GetLength())
		{
			var jsonString = saveFile.GetLine();

			var json = new Json();
			var parseResult = json.Parse(jsonString, true);

            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                continue;
            }

			Array data = new Godot.Collections.Array((Godot.Collections.Array)json.Data);

			// ListName
			listName = (string)data[0];
			// Colors
			Color[] colors = (Color[])data[1];
			PopulateColors(colors);

			// Options
			Array options = (Array)data[2];
			PopulateOptions(options);
        }
    }

	private void DeleteOptions(List<Option> listToRemove)
	{
		foreach(Option option in listToRemove)
		{
			option.QueueFree();
		}
		listToRemove.Clear();
	}

	public List<string> GetSaveFiles()
	{
		string[] allFiles = DirAccess.GetFilesAt("user://");
        List<string> saveFiles = new List<string>();

		foreach (string file in allFiles)
		{
			if(file.EndsWith(".save"))
			{
				saveFiles.Add(file);
			}
		}

		return saveFiles;
    }

	private void PopulateColors(Color[] newColors)
	{
		colors.Clear();
		foreach(Color color in newColors)
		{
			colors.Add(color);
		}
	}

	private void PopulateOptions(Array loadOptions)
	{
		foreach(Array option in loadOptions)
		{
			Option newOption = Option.CreateOptions(option, optionTemplate, optionParent);
			if(newOption.OptionEnabled)
			{
				createdOptions.Add(newOption);
				continue;
			}
			disabledOptions.Add(newOption);
		}
        WheelProgressParent.EmitSignal(WheelProgress.SignalName.WheelProgressUpdate, true);
    }
}