using Godot;
using System;

public partial class CustomizationMenu : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    private System.Collections.Generic.List<ColorPickerButton> colorPickerButtons = new System.Collections.Generic.List<ColorPickerButton>();

    private CustomizationManager customizationManager;

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
    //		STANDARD FUNCTIONS	
    // --------------------------------

    // Called when the node enters the scene tree for the first time.
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
        customizationManager = CustomizationManager.Instance;
        PopupManager.Instance.IsCustomizationOpen = true;

        //Changed order here to better utilize new arrays
        PopulateColorPickerList();
        AssignPickerColors();
        AttachListeners();
    }

    // --------------------------------
    //		COLOR PICKER LOGIC
    // --------------------------------

    /// <summary>
    /// Assigns initial colors to designated colior picker buttons
    /// </summary>
    public void AssignPickerColors()
    {
        for (int i = 0; i < colorPickerButtons.Count; i++)
        {
            GD.Print(i);
            colorPickerButtons[i].Color = customizationManager.CurrentColors[i];
        }
    }

    /// <summary>
    /// Attaches proper logic to each color picker's ColorChanged event listener
    /// </summary>
    private void AttachListeners()
    {
        for (int i = 0; i < colorPickerButtons.Count; i++)
        {
            colorPickerButtons[i].ColorChanged += ColorPickerChanged;
        }
    }

    private void ColorPickerChanged(Color c)
    {
        for (int i = 0; i < colorPickerButtons.Count; i++)
        {
            customizationManager.CurrentColors[i] = colorPickerButtons[i].Color;
        }

        CustomizationManager.UpdateColors();
        AssignPickerColors();
    }

    /// <summary>
    /// Populates the list of color pickers if empty
    /// </summary>
    private void PopulateColorPickerList()
    {
        if (colorPickerButtons.Count > 0)
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
}
