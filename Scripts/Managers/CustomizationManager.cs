using Godot;
using Godot.Collections;

public partial class CustomizationManager : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    private System.Collections.Generic.List<ColorPickerButton> colorPickerButtons = new System.Collections.Generic.List<ColorPickerButton>();

    // Color Picker Buttons
    [Export]
    private ColorPickerButton generalBackgroundColor;
    [Export]
    private ColorPickerButton secondaryBackgroundColor;
    [Export]
    private ColorPickerButton wheelPrimaryColor;
    [Export]
    private ColorPickerButton wheelSecondaryColor;
    [Export]
    private ColorPickerButton wheelButtonColor;
    [Export]
    private ColorPickerButton wheelButtonTextColor;
    [Export]
    private ColorPickerButton listBackgroundColor;
    [Export]
    private ColorPickerButton listFontColor;
    [Export]
    private ColorPickerButton popupBackgroundColor;
    [Export]
    private ColorPickerButton popupFontColor;

    // --------------------------------
    //		    PROPERTIES	
    // --------------------------------
    public static CustomizationManager Instance { get; private set; }

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
        PopupManager.Instance.IsCustomizationOpen = true;

        //Changed order here to better utilize new arrays
        PopulateColorPickerList();
        AssignPickerColors();
        AttachListeners();
    }

    /// <summary>
    /// Assigns initial colors to designated colior picker buttons
    /// </summary>
    private void AssignPickerColors()
    {
        for (int i = 0; i < colorPickerButtons.Count; i++) 
        {
            GD.Print(i);
            colorPickerButtons[i].Color = GameManager.Instance.currentColors[i];
        }
    }

    /// <summary>
    /// Attaches proper logic to each color picker's ColorChanged event listener
    /// </summary>
    private void AttachListeners()
    {
        for(int i = 0; i < colorPickerButtons.Count; i++) 
        {
            colorPickerButtons[i].ColorChanged += ColorPickerChanged;
        }
    }

    private void ColorPickerChanged(Color c) 
    {
        for (int i = 0; i < colorPickerButtons.Count; i++)
        {
            GameManager.Instance.currentColors[i] = colorPickerButtons[i].Color;
        }

        UpdateColors();
    }

    /// <summary>
    /// Populates the list of color pickers if empty
    /// </summary>
    private void PopulateColorPickerList()
    {
        if(colorPickerButtons.Count > 0)
        {
            return;
        }
        colorPickerButtons =
        [
            generalBackgroundColor,
            secondaryBackgroundColor,
            wheelPrimaryColor,
            wheelSecondaryColor,
            wheelButtonColor,
            wheelButtonTextColor,
            listBackgroundColor,
            listFontColor,
            popupBackgroundColor,
            popupFontColor
        ];
    }

    /// <summary>
    /// Force Color Updates across application based off current color picker button colors
    /// </summary>
    public static void UpdateColors()
    {
        ColorPalletChanged?.Invoke(GameManager.Instance.currentColors);
        Instance?.AssignPickerColors();
    }
}
