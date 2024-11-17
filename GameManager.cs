using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class GameManager : Node
{
    // --------------------------------
	//			VARIABLES	
    // --------------------------------

	// Save Data
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
	private TextureRect backgroundTexture;
	[Export]
	private ColorRect listBackground;
	[Export]
	private SpinButton spinButton;

	// Option Data
    [Export]
    private PackedScene optionTemplate;
    [Export]
    private Control optionParent;

	// Default Color Data
	[Export]
	private Color default_generalBackgroundColor;
    [Export]
    private Color default_secondaryBackgroundColor;
    [Export]
    private Color default_wheelPrimaryColor;
    [Export]
    private Color default_wheelSecondaryColor;
    [Export]
    private Color default_wheelButtonColor;
    [Export]
    private Color default_listBackgroundColor;
    [Export]
    private Color default_listFontColor;
    [Export]
    private Color default_popupBackgroundColor;
    [Export]
    private Color default_popupFontColor;
	private List<Color> defaultColors = new List<Color>();

	// Audio Info
	[Export]
	private AudioStreamPlayer audioStreamPlayer;

	// Twitch Variables
	[Export]
	private ColorRect twitchInfoArea;

    // Focus Info
    [Export]
    private Control option_leftBegin;
    [Export]
    private Control option_rightEnd;
    [Export]
    private Control option_topBegin;
    [Export]
    private Control option_bottomEnd;


    // --------------------------------
    //			PROPERTIES	
    // --------------------------------

    public static GameManager Instance { get; private set; }

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
	public TextureRect BackgroundTexture { get => backgroundTexture; }
	public ColorRect ListBackground { get => listBackground; }
	public SpinButton SpinButton { get => spinButton; }

	// Option Data
	public PackedScene OptionTemplate { get => optionTemplate; }
	public Control OptionParent { get => optionParent; }

	// Default Color Data
	public List<Color> DefaultColors { get => defaultColors; }

	// Audio Info
	public AudioStreamPlayer AudioStreamPlayer { get => audioStreamPlayer; }

	// Twitch Variables
	public ColorRect TwitchInfoArea { get => twitchInfoArea; }

    // Focus Info
    public Control Option_LeftBegin {  get => option_leftBegin; }
    public Control Option_RightEnd {  get => option_rightEnd; }
    public Control Option_TopBegin { get => option_topBegin; }
    public Control Option_BottomEnd { get => option_bottomEnd; }

    // --------------------------------
    //		STANDARD FUNCTIONS
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		Instance = this;
		SetListToDefaultValues(colors);
		SetListToDefaultValues(defaultColors);
		twitchInfoArea.Visible = false;
	}

    // --------------------------------
    //		CUSTOMIZATION LOGIC
    // --------------------------------

	/// <summary>
	/// Populates the given list with the assigned default color values
	/// </summary>
	private void SetListToDefaultValues(List<Color> list)
	{
		list.Add(default_generalBackgroundColor);
		list.Add(default_secondaryBackgroundColor);
        list.Add(default_wheelPrimaryColor);
		list.Add(default_wheelSecondaryColor);
        list.Add(default_wheelButtonColor);
        list.Add(default_listBackgroundColor);
		list.Add(default_listFontColor);
        list.Add(default_popupBackgroundColor);
		list.Add(default_popupFontColor);
	}

	/// <summary>
	/// Assigns the Progress Bars to their primary or secondary colors
	/// </summary>
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

	/// <summary>
	/// Prepares all necessary data for saving into the appropriate list
	/// </summary>
    private void PopulateDataToSave()
	{
		List<Option> allOptions = new List<Option>();
		allOptions.AddRange(createdOptions);
		allOptions.AddRange(disabledOptions);

		Array colorsArray = new Array();
		
		foreach(Color color in colors)
		{
			colorsArray.Add(color.ToHtml());
		}
		
		Array optionsArray = new Array();

		foreach (Option option in allOptions)
		{
			optionsArray.Add(option.GetOptionData());
		}

		dataToSave.Add(ListName);
		dataToSave.Add(colorsArray);
		dataToSave.Add(optionsArray);
	}

	/// <summary>
	/// Stores all data in the appropriate data list into a json object and writes it into a file, using the listName for the fileName
	/// </summary>
	public void SaveGame()
	{
		PopulateDataToSave();
        using var saveFile = FileAccess.Open("user://" + listName + ".save", FileAccess.ModeFlags.Write);

		Array dataToSaveArray = new Array(dataToSave.ToArray());

		var jsonData = Json.Stringify(dataToSaveArray);
		saveFile.StoreLine(jsonData);
		dataToSave.Clear();
	}

	/// <summary>
	/// Takes in a specific file and attempts to load in the data of said file. Deletes relevant data from the application prior to loading in new data.
	/// </summary>
	/// <param name="specificFile"></param>
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

			// Update ListName
			listName = (string)data[0];
			// Update Colors
			Array colorsArray = (Array)data[1];
			PopulateColors(colorsArray);
			CustomizationManager.Instance.AssignColorsToList(colors);

			// Update Option List
			Array options = (Array)data[2];
			PopulateOptions(options);
        }
    }

	/// <summary>
	/// Deletes all options objects from a given list
	/// </summary>
	/// <param name="listToRemove"></param>
	private void DeleteOptions(List<Option> listToRemove)
	{
		if(listToRemove == null || listToRemove.Count == 0) return;

		foreach(Option option in listToRemove)
		{
			option.QueueFree();
		}
		listToRemove.Clear();
	}

	/// <summary>
	/// Grabs all files at correct location and returns any qualifying save files
	/// </summary>
	/// <returns>A list of .save files</returns>
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

	/// <summary>
	/// Takes in a generic array (should be strings of hexcode) and loads them into the colors list
	/// </summary>
	/// <param name="newColors"></param>
	private void PopulateColors(Array newColors)
	{
		for(int i = 0; i < newColors.Count; i++)
		{
			newColors[i] = (string)newColors[i];
		}

		colors.Clear();
		foreach(string color in newColors)
		{
			colors.Add(new Color(color));
		}
	}

	/// <summary>
	/// Creates and populates options based off the general array provided (primarily through the load system)
	/// </summary>
	/// <param name="loadOptions"></param>
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