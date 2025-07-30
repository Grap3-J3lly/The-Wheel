using Godot;
using Godot.Collections;

public partial class GameManager : Node
{
    // --------------------------------
	//			VARIABLES	
    // --------------------------------

	// Save Data
	// private const string CONST_SaveFileName = "wheelData";
	private Array<Variant> dataToSave = new Array<Variant>();
	private Array<Color> colors = new Array<Color>();
	private Array<Option> createdOptions = new Array<Option>();
	private Array<Option> disabledOptions = new Array<Option>();

	private string listName = "New List";

	private bool wheelSpinning;
	private WheelProgress wheelProgressParent;

	// Customizable UI Elements
	[ExportGroup("Customizable UI Elements")]
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

	[ExportGroup("Option Data")]
	// Option Data
    [Export]
    private PackedScene optionTemplate;
    [Export]
    private Control optionParent;

	[ExportGroup("Default Color Data")]
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
	private Array<Color> defaultColors = new Array<Color>();

    [ExportGroup("Audio Info")]
    // Audio Info
    [Export]
	private AudioStreamPlayer audioStreamPlayer;

    [ExportGroup("Twitch Info")]
    // Twitch Variables
    [Export]
	private ColorRect twitchInfoArea;

    [ExportGroup("Focus Info")]
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

	public Array<Color> Colors { get => colors; set => colors = value; }
    public Array<Option> CreatedOptions {  get { return createdOptions; } }
	public Array<Option> DisabledOptions { get { return disabledOptions; } }

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
	public Array<Color> DefaultColors { get => defaultColors; }

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
	private void SetListToDefaultValues(Array<Color> list)
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
		Array<Option> allOptions = new Array<Option>();
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
		Array<Option> allOptions = new Array<Option>();
		allOptions.AddRange(createdOptions);
		allOptions.AddRange(disabledOptions);

		Array<Array> optionData = new Array<Array>();
		for (int i = 0; i < allOptions.Count; i++)
		{
			optionData.Add(allOptions[i].GetOptionData());
		}

		SaveSystem.AddDataItem(ListName, "optionData", optionData);

		Array<Array<float>> colorData = new Array<Array<float>>();
		foreach(Color color in colors)
		{
			Array<float> colorDataObj = [color.R, color.G, color.B, color.A];
			colorData.Add(colorDataObj);
		}

		SaveSystem.AddDataItem(ListName, "colors", colorData);
		SaveSystem.SaveData(ListName);
	}

	/// <summary>
	/// Takes in a specific file and attempts to load in the data of said file. Deletes relevant data from the application prior to loading in new data.
	/// </summary>
	/// <param name="specificFile"></param>
	public void LoadGame(string specificFile)
	{
		
		DeleteOptions(createdOptions);
		DeleteOptions(disabledOptions);

		GD.Print($"GameManager.cs: Specific File to be opened: {specificFile}");

		ListName = specificFile;
		Array<Array> optionDataToPopulate = SaveSystem.GetDataItem(ListName, "optionData", new Array<Array>());
		Array<Array<float>> loadedColors = SaveSystem.GetDataItem(ListName, "colors", new Array<Array<float>>());

		PopulateOptions(optionDataToPopulate);
		PopulateColors(loadedColors);
    }

	/// <summary>
	/// Deletes all options objects from a given list
	/// </summary>
	/// <param name="listToRemove"></param>
	private void DeleteOptions(Array<Option> listToRemove)
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
	public Array<string> GetSaveFiles()
	{
		string[] allFiles = DirAccess.GetFilesAt("user://");
        Array<string> saveFiles = new Array	<string>();

		foreach (string file in allFiles)
		{
			if(file.EndsWith(".saved"))
			{
				GD.Print($"GameManager.cs: SaveFile Name: {file}");
				saveFiles.Add(file);
			}
		}

		return saveFiles;
    }

	/// <summary>
	/// Creates and populates options based off the general array provided (primarily through the load system)
	/// </summary>
	/// <param name="loadOptions"></param>
	private void PopulateOptions(Array<Array> loadOptions)
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

    /// <summary>
    /// Takes in a generic array (should be strings of hexcode) and loads them into the colors list
    /// </summary>
    /// <param name="newColors"></param>
    private void PopulateColors(Array<Array<float>> newColors)
    {
        colors.Clear();
        foreach(Array<float> color in newColors)
		{
			colors.Add(new Color(color[0], color[1], color[2], color[3]));
		}
		CustomizationManager.Instance.AssignColorsToList(colors);
    }
}