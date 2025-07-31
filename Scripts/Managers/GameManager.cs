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
	private Array<Option> createdOptions = new Array<Option>();
	private Array<Option> disabledOptions = new Array<Option>();

	private string listName = "New List";

	private bool wheelSpinning;
	private WheelProgress wheelProgressParent;

	// Customizable UI Elements
	[Export]
	private SpinButton spinButton;

	[ExportGroup("Option Data")]
	// Option Data
    [Export]
    private PackedScene optionTemplate;
    [Export]
    private Control optionParent;

	// Default Color Data
	[ExportGroup("Default Color Data")]
    [Export]
    private ColorPalette default_palette;
	//private Array<Color> defaultColors = new Array<Color>();
    public ColorPalette currentColors;


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
    public Array<Option> CreatedOptions {  get { return createdOptions; } }
	public Array<Option> DisabledOptions { get { return disabledOptions; } }

	public string ListName { get => listName; set => listName = value; }

	public bool WheelSpinning { get => wheelSpinning; set => wheelSpinning = value; }
	public WheelProgress WheelProgressParent { get => wheelProgressParent; set => wheelProgressParent = value; }

	public SpinButton SpinButton { get => spinButton; }

	// Option Data
	public PackedScene OptionTemplate { get => optionTemplate; }
	public Control OptionParent { get => optionParent; }

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
        CustomizationManager.ColorPalletChanged += UpdateWheelColors;
        default_palette.ReadyColors();
        ResetColors();
		twitchInfoArea.Visible = false;
	}

    // --------------------------------
    //		CUSTOMIZATION LOGIC
    // --------------------------------

	/// <summary>
	/// Assigns the Progress Bars to their primary or secondary colors
	/// </summary>
	public void UpdateWheelColors(ColorPalette palett)
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
		foreach(Color color in currentColors)
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
        for(int i = 0; i < newColors.Count; i++) 
        {
            Array<float> color = newColors[i];
            currentColors[i] = new Color(color[0], color[1], color[2], color[3]);
        }
		CustomizationManager.UpdateColors();
    }

    public void ResetColor(int colorIndex)
    {
        currentColors[colorIndex] = default_palette[colorIndex];
        CustomizationManager.UpdateColors();
    }

    public void ResetColors() 
    {
        currentColors = new ColorPalette(default_palette);
        CustomizationManager.UpdateColors();
    }
}