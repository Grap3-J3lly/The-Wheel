using Godot;
using Godot.Collections;

public partial class CustomizationManager : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private GameManager gameManager;
    private Array<ColorPickerButton> colorPickerButtons = new Array<ColorPickerButton>();

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

    // Various Themes
    [Export]
    private Theme popupBackgroundTheme;
    [Export]
    private Theme popupFontTheme;
    [Export]
    private Theme listFontTheme;
    [Export]
    private Theme wheelButtonTheme;

    // Const Values
    private const int CONST_Index_GeneralBackgroundColor = 0;
    private const int CONST_Index_SecondaryBackgroundColor = 1;
    private const int CONST_Index_WheelPrimaryColor = 2;
    private const int CONST_Index_WheelSecondaryColor = 3;
    private const int CONST_Index_WheelButtonColor = 4;
    private const int CONST_Index_WheelButtonTextColor = 5;
    private const int CONST_Index_ListBackgroundColor = 6;
    private const int CONST_Index_ListFontColor = 7;
    private const int CONST_Index_PopupBackgroundColor = 8;
    private const int CONST_Index_PopupFontColor = 9;

    private const float CONST_ColorDarken_Quarter = .25f;
    private const float CONST_ColorDarken_Half = .5f;

    private const float CONST_ColorFadeAmount = .6f;

    // --------------------------------
    //		    PROPERTIES	
    // --------------------------------

    public static CustomizationManager Instance { get; private set; }
    public Array<ColorPickerButton> ColorPickerButtons { get => colorPickerButtons; }

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
        gameManager = GameManager.Instance;
        PopupManager.Instance.IsCustomizationOpen = true;
        PopulateColorPickerList();
        PopulateColorList();
        AssignInitialPickerColors();
        AttachListeners();
    }

    /// <summary>
    /// Assigns initial colors to designated colior picker buttons
    /// </summary>
    private void AssignInitialPickerColors()
    {
        generalBackgroundColor.Color = gameManager.ApplicationBackground.Color;
        secondaryBackgroundColor.Color = gameManager.BackgroundTexture.Modulate;
        wheelPrimaryColor.Color = gameManager.PrimaryColor;
        wheelSecondaryColor.Color = gameManager.SecondaryColor;
        wheelButtonColor.Color = GetWheelButtonColor();
        wheelButtonTextColor.Color = GetWheelButtonTextColor();
        listBackgroundColor.Color = gameManager.ListBackground.Color;
        listFontColor.Color = GetListFontColor();
        popupBackgroundColor.Color = GetPopupBackgroundColor();
        popupFontColor.Color = GetPopupFontColor();
    }

    /// <summary>
    /// Attaches proper logic to each color picker's ColorChanged event listener
    /// </summary>
    private void AttachListeners()
    {
        generalBackgroundColor.ColorChanged += ChangeGeneralBackgroundColor;
        secondaryBackgroundColor.ColorChanged += ChangeSecondaryBackgroundColor;
        wheelPrimaryColor.ColorChanged += ChangeWheelPrimaryColor;
        wheelSecondaryColor.ColorChanged += ChangeWheelSecondaryColor;
        wheelButtonColor.ColorChanged += ChangeWheelButtonColor;
        wheelButtonTextColor.ColorChanged += ChangeWheelButtonTextColor;
        listBackgroundColor.ColorChanged += ChangeListBackgroundColor;
        listFontColor.ColorChanged += ChangeListFontColor;
        popupBackgroundColor.ColorChanged += ChangePopupBackgroundColor;
        popupFontColor.ColorChanged += ChangePopupFontColor;
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
    /// Assigns current colors to each color picker button
    /// </summary>
    private void PopulateColorList()
    {
        if (gameManager.Colors.Count == 0) return;

        for(int index = 0; index < colorPickerButtons.Count; index++)
        {
            gameManager.Colors[index] = colorPickerButtons[index].Color;
        }
    }

    /// <summary>
    /// Given a list of colors of the same size as the color picker list, assigns color to each color picker button and updates the corresponding application element
    /// </summary>
    /// <param name="assignmentColors"></param>
    public void AssignColorsToList(Array<Color> assignmentColors)
    {
        if(assignmentColors.Count < colorPickerButtons.Count)
        {
            return;
        }

        for (int i = 0; i < assignmentColors.Count; i++)
        {
            colorPickerButtons[i].Color = assignmentColors[i];
        }
        ManuallyRunListeners(assignmentColors);
    }

    /// <summary>
    /// Updates all potential application elements with the given list of colors
    /// </summary>
    /// <param name="newColors"></param>
    private void ManuallyRunListeners(Array<Color> newColors)
    {
        ChangeGeneralBackgroundColor(newColors[CONST_Index_GeneralBackgroundColor]);
        ChangeSecondaryBackgroundColor(newColors[CONST_Index_SecondaryBackgroundColor]);
        ChangeWheelPrimaryColor(newColors[CONST_Index_WheelPrimaryColor]);
        ChangeWheelSecondaryColor(newColors[CONST_Index_WheelSecondaryColor]);
        ChangeWheelButtonColor(newColors[CONST_Index_WheelButtonColor]);
        ChangeWheelButtonTextColor(newColors[CONST_Index_WheelButtonTextColor]);
        ChangeListBackgroundColor(newColors[CONST_Index_ListBackgroundColor]);
        ChangeListFontColor(newColors[CONST_Index_ListFontColor]);
        ChangePopupBackgroundColor(newColors[CONST_Index_PopupBackgroundColor]);
        ChangePopupFontColor(newColors[CONST_Index_PopupFontColor]);
    }

    /// <summary>
    /// Force Color Updates across application based off current color picker button colors
    /// </summary>
    public void UpdateColors()
    {
        ChangeGeneralBackgroundColor(colorPickerButtons[CONST_Index_GeneralBackgroundColor].Color);
        ChangeSecondaryBackgroundColor(colorPickerButtons[CONST_Index_SecondaryBackgroundColor].Color);
        ChangeWheelPrimaryColor(colorPickerButtons[CONST_Index_WheelPrimaryColor].Color);
        ChangeWheelSecondaryColor(colorPickerButtons[CONST_Index_WheelSecondaryColor].Color);
        ChangeWheelButtonColor(colorPickerButtons[CONST_Index_WheelButtonColor].Color);
        ChangeWheelButtonTextColor(colorPickerButtons[CONST_Index_WheelButtonTextColor].Color);
        ChangeListBackgroundColor(colorPickerButtons[CONST_Index_ListBackgroundColor].Color);
        ChangeListFontColor(colorPickerButtons[CONST_Index_ListFontColor].Color);
        ChangePopupBackgroundColor(colorPickerButtons[CONST_Index_PopupBackgroundColor].Color);
        ChangePopupFontColor(colorPickerButtons[CONST_Index_PopupFontColor].Color);
    }

    // --------------------------------
    //		COLOR PICKER LOGIC
    // --------------------------------

    /// <summary>
    /// Assigns the application background color and the corresponding Color list element for future save data
    /// </summary>
    /// <param name="color"></param>
    private void ChangeGeneralBackgroundColor(Color color)
    {
        gameManager.ApplicationBackground.Color = color;
        gameManager.Colors[CONST_Index_GeneralBackgroundColor] = color;
    }

    private void ChangeSecondaryBackgroundColor(Color color)
    {
        gameManager.BackgroundTexture.Modulate = color;
        gameManager.Colors[CONST_Index_SecondaryBackgroundColor] = color;
    }

    /// <summary>
    /// Assigns the new primary color value, refreshes the wheel, and updates the corresponding Color list element for future save data
    /// </summary>
    /// <param name="color"></param>
    private void ChangeWheelPrimaryColor(Color color)
    {
        gameManager.PrimaryColor = color;
        gameManager.UpdateWheelColors();
        gameManager.Colors[CONST_Index_WheelPrimaryColor] = color;
    }

    /// <summary>
    /// Assigns the new secondary color value, refreshes the wheel, and updates the corresponding Color list element for future save data
    /// </summary>
    /// <param name="color"></param>
    private void ChangeWheelSecondaryColor(Color color)
    {
        gameManager.SecondaryColor = color;
        gameManager.UpdateWheelColors();
        gameManager.Colors[CONST_Index_WheelSecondaryColor] = color;
    }

    /// <summary>
    /// Assigns the wheel button color value and updates the corresponding Color list element for future save data
    /// </summary>
    /// <param name="color"></param>
    private void ChangeWheelButtonColor(Color color)
    {
        SetWheelButtonColor(color);
        gameManager.Colors[CONST_Index_WheelButtonColor] = color;
    }

    private void ChangeWheelButtonTextColor(Color color)
    {
        SetWheelButtonTextColor(color);
        gameManager.Colors[CONST_Index_WheelButtonTextColor] = color;
    }

    /// <summary>
    /// Assigns the list background color value and updates the corresponding Color list element for future save data
    /// </summary>
    /// <param name="color"></param>
    private void ChangeListBackgroundColor(Color color)
    {
        gameManager.ListBackground.Color = color;
        gameManager.Colors[CONST_Index_ListBackgroundColor] = color;
    }

    /// <summary>
    /// Assigns the list font color value and updates the corresponding Color list element for future save data
    /// </summary>
    /// <param name="color"></param>
    private void ChangeListFontColor(Color color)
    {
        SetListFontColor(color);
        gameManager.Colors[CONST_Index_ListFontColor] = color;
    }

    /// <summary>
    /// Assigns the popup background color value and updates the corresponding Color list element for future save data
    /// </summary>
    /// <param name="color"></param>
    private void ChangePopupBackgroundColor(Color color)
    {
        SetPopupBackgroundColor(color);
        gameManager.Colors[CONST_Index_PopupBackgroundColor] = color;
    }

    /// <summary>
    /// Assigns the popup font color value and updates the corresponding Color list element for future save data
    /// </summary>
    /// <param name="color"></param>
    private void ChangePopupFontColor(Color color)
    {
        SetPopupFontColor(color);
        gameManager.Colors[CONST_Index_PopupFontColor] = color;
    }

    // --------------------------------
    //			THEME LOGIC
    // --------------------------------

    /// <summary>
    /// Grabs and returns the desired color from the wheel button theme
    /// </summary>
    /// <returns></returns>
    private Color GetWheelButtonColor()
    {
        StyleBoxTexture styleBoxTexture = (StyleBoxTexture)wheelButtonTheme.Get("Button/styles/normal");
        return (Color)styleBoxTexture.Get("modulate_color");
    }

    /// <summary>
    /// Assigns the given color to all elements of the wheel button theme
    /// </summary>
    /// <param name="color"></param>
    private void SetWheelButtonColor(Color color)
    {
        StyleBoxTexture normalResult = (StyleBoxTexture)wheelButtonTheme.Get("Button/styles/normal");
        normalResult.Set("modulate_color", color);

        StyleBoxTexture hoverResult = (StyleBoxTexture)wheelButtonTheme.Get("Button/styles/hover");
        hoverResult.Set("modulate_color", color.Darkened(CONST_ColorDarken_Quarter));

        StyleBoxTexture pressedResult = (StyleBoxTexture)wheelButtonTheme.Get("Button/styles/pressed");
        pressedResult.Set("modulate_color", color.Darkened(CONST_ColorDarken_Half));
    }

    private Color GetWheelButtonTextColor()
    {
        Color wheelButtonTextColor = (Color)wheelButtonTheme.Get("RichTextLabel/colors/default_color");
        return wheelButtonTextColor;
    }

    private void SetWheelButtonTextColor(Color color)
    {
        GD.Print($"CustomizationManager.cs: Assigning Wheel Button Text Color");
        wheelButtonTheme.Set("RichTextLabel/colors/default_color", color);
    }

    /// <summary>
    /// Grabs and returns the desired color from the list font theme
    /// </summary>
    /// <returns></returns>
    private Color GetListFontColor()
    {
        Color fontColor = (Color)listFontTheme.Get("LineEdit/colors/font_color");
        return fontColor;
    }

    /// <summary>
    /// Assigns the given color to all elements of the list font theme
    /// </summary>
    /// <param name="color"></param>
    private void SetListFontColor(Color color)
    {
        listFontTheme.Set("LineEdit/colors/font_color", color);
        listFontTheme.Set("TextEdit/colors/font_color", color);
        listFontTheme.Set("TextEdit/colors/font_placeholder_color", new Color(color, color.A * CONST_ColorFadeAmount));
        listFontTheme.Set("RichTextLabel/colors/default_color", color);
    }

    /// <summary>
    /// Grabs and returns the desired color from the popup background theme
    /// </summary>
    /// <returns></returns>
    private Color GetPopupBackgroundColor()
    {
        StyleBoxFlat styleBoxFlat = (StyleBoxFlat)popupBackgroundTheme.Get("Panel/styles/panel");
        Color color = (Color)styleBoxFlat.Get("bg_color");
        return color;
    }
    /// <summary>
    /// Assigns the given color to the popup background theme
    /// </summary>
    /// <param name="color"></param>
    private void SetPopupBackgroundColor(Color color)
    {
        StyleBoxFlat styleBoxFlat = (StyleBoxFlat)popupBackgroundTheme.Get("Panel/styles/panel");
        styleBoxFlat.Set("bg_color", color);
    }

    /// <summary>
    /// Grabs and returns the desired color from the popup font theme
    /// </summary>
    /// <returns></returns>
    private Color GetPopupFontColor()
    {
        return (Color)popupFontTheme.Get("Button/colors/font_color");
    }

    /// <summary>
    /// Assigns the given color to all elements of the popup font theme
    /// </summary>
    /// <param name="color"></param>
    private void SetPopupFontColor(Color color)
    {
        popupFontTheme.Set("RichTextLabel/colors/default_color", color);

        popupFontTheme.Set("Button/colors/font_color", color);
        popupFontTheme.Set("Button/colors/font_hover_color", color.Darkened(CONST_ColorDarken_Quarter));
        popupFontTheme.Set("Button/colors/font_pressed_color", color.Darkened(CONST_ColorDarken_Half));
    }
}
