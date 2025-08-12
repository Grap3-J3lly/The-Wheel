using Godot;
using Godot.Collections;

public partial class CustomizationManager : Node
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    [Export]
    private ColorPalette defaultColors;
    private ColorPalette currentColors;

    // --------------------------------
    //		    PROPERTIES	
    // --------------------------------
    public static CustomizationManager Instance { get; private set; }

    public ColorPalette DefaultColors { get => defaultColors; }
    public ColorPalette CurrentColors { get => currentColors; }

    // --------------------------------
    //	    C# STYLE EVENT LOGIC
    // --------------------------------
    public delegate void OnColorPalletChanged(ColorPalette newColorPalett);
    public static event OnColorPalletChanged ColorPalletChanged;


    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        base._Ready();
        currentColors = defaultColors;
        ColorPalletChanged += UpdateWheelColors;
        Setup();
    }

    // --------------------------------
    //		    SETUP LOGIC
    // --------------------------------

    /// <summary>
    /// Handles Starting Initialization and Misc. Setup Logic
    /// </summary>
    private void Setup()
    {
        Instance = this;
        DefaultColors.ReadyColors();
        CallDeferred("ResetColors");
    }

    // --------------------------------
    //		    COLOR LOGIC
    // --------------------------------

    /// <summary>
	/// Assigns the Progress Bars to their primary or secondary colors
	/// </summary>
	public void UpdateWheelColors(ColorPalette palett)
    {
        Array<Option> options = GameManager.Instance.CreatedOptions;
        foreach (Option option in options)
        {
            option.OptionProgressBar.AssignBarColor(options.IndexOf(option));
        }
    }

    /// <summary>
    /// Force Color Updates across application based off current color picker button colors
    /// </summary>
    public static void UpdateColors()
    {
        // GD.Print($"CustomizationManager.cs: Calling UpdateColors");
        ColorPalletChanged?.Invoke(Instance.currentColors);
    }

    /// <summary>
    /// Takes in a generic array (should be strings of hexcode) and loads them into the colors list
    /// </summary>
    /// <param name="newColors"></param>
    public void PopulateColors(Array<Array<float>> newColors)
    {
        for (int i = 0; i < newColors.Count; i++)
        {
            Array<float> color = newColors[i];
            currentColors[i] = new Color(color[0], color[1], color[2], color[3]);
        }
        UpdateColors();
    }

    public void ResetColor(int colorIndex)
    {
        currentColors[colorIndex] = defaultColors[colorIndex];
        UpdateColors();
    }

    public void ResetColors()
    {
        currentColors = new ColorPalette(defaultColors);
        UpdateColors();
    }
}
