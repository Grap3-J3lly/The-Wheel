using Godot;
using System.Collections.Generic;

public partial class CustomizationManager : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private List<ColorPickerButton> colorPickers = new List<ColorPickerButton>();

    // Color Picker Buttons
    [Export]
    private ColorPickerButton generalBackgroundColor;
    [Export]
    private ColorPickerButton wheelPrimaryColor;
    [Export]
    private ColorPickerButton wheelSecondaryColor;
    [Export]
    private ColorPickerButton wheelButtonColor;
    [Export]
    private ColorPickerButton listBackgroundColor;
    [Export]
    private ColorPickerButton listFontColor;
    [Export]
    private ColorPickerButton popupBackgroundColor;
    [Export]
    private ColorPickerButton popupFontColor;

    [Export]
    private Theme popupBackgroundTheme;
    [Export]
    private Theme popupFontTheme;
    [Export]
    private Theme listFontTheme;
    [Export]
    private Theme wheelButtonTheme;

    private OptionManager optionManager;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
        base._Ready();
        optionManager = OptionManager.Instance;
        Setup();
    }

    // --------------------------------
    //		    SETUP LOGIC
    // --------------------------------

    private void Setup()
    {
        PopupManager.Instance.IsCustomizationOpen = true;
        AssignInitialPickerColors();
        AttachListeners();
        PopulateColorPickerList();
        PopulateColorList();
    }

    private void AssignInitialPickerColors()
    {
        generalBackgroundColor.Color = optionManager.ApplicationBackground.Color;
        wheelPrimaryColor.Color = optionManager.PrimaryColor;
        wheelSecondaryColor.Color = optionManager.SecondaryColor;
        wheelButtonColor.Color = GetWheelButtonColor();
        listBackgroundColor.Color = optionManager.ListBackground.Color;
        listFontColor.Color = GetListFontColor();
        popupBackgroundColor.Color = GetPopupBackgroundColor();
        popupFontColor.Color = GetPopupFontColor();
    }

    private void AttachListeners()
    {
        generalBackgroundColor.ColorChanged += ChangeGeneralBackgroundColor;
        wheelPrimaryColor.ColorChanged += ChangeWheelPrimaryColor;
        wheelSecondaryColor.ColorChanged += ChangeWheelSecondaryColor;
        wheelButtonColor.ColorChanged += ChangeWheelButtonColor;
        listBackgroundColor.ColorChanged += ChangeListBackgroundColor;
        listFontColor.ColorChanged += ChangeListFontColor;
        popupBackgroundColor.ColorChanged += ChangePopupBackgroundColor;
        popupFontColor.ColorChanged += ChangePopupFontColor;
    }

    private void PopulateColorPickerList()
    {
        colorPickers.Add(generalBackgroundColor);
        colorPickers.Add(wheelPrimaryColor);
        colorPickers.Add(wheelSecondaryColor);
        colorPickers.Add(wheelButtonColor);
        colorPickers.Add(listBackgroundColor);
        colorPickers.Add(listFontColor);
        colorPickers.Add(popupBackgroundColor);
        colorPickers.Add(popupFontColor);
    }

    private void PopulateColorList()
    {
        optionManager.Colors.Add(generalBackgroundColor.Color);
        optionManager.Colors.Add(wheelPrimaryColor.Color);
        optionManager.Colors.Add(wheelSecondaryColor.Color);
        optionManager.Colors.Add(wheelButtonColor.Color);
        optionManager.Colors.Add(listBackgroundColor.Color);
        optionManager.Colors.Add(listFontColor.Color);
        optionManager.Colors.Add(popupBackgroundColor.Color);
        optionManager.Colors.Add(popupFontColor.Color);
    }

    // --------------------------------
    //		COLOR PICKER LOGIC
    // --------------------------------

    private void ChangeGeneralBackgroundColor(Color color)
    {
        optionManager.ApplicationBackground.Color = color;
    }

    private void ChangeWheelPrimaryColor(Color color)
    {
        optionManager.PrimaryColor = color;
        optionManager.UpdateWheelColors();
    }

    private void ChangeWheelSecondaryColor(Color color)
    {
        optionManager.SecondaryColor = color;
        optionManager.UpdateWheelColors();
    }

    private void ChangeWheelButtonColor(Color color)
    {
        SetWheelButtonColor(color);
    }

    private void ChangeListBackgroundColor(Color color)
    {
        optionManager.ListBackground.Color = color;
    }

    private void ChangeListFontColor(Color color)
    {
        SetListFontColor(color);
    }

    private void ChangePopupBackgroundColor(Color color)
    {
        SetPopupBackgroundColor(color);
    }

    private void ChangePopupFontColor(Color color)
    {
        SetPopupFontColor(color);
    }

    // --------------------------------
    //			THEME LOGIC
    // --------------------------------

    private Color GetPopupBackgroundColor()
    {
        StyleBoxFlat styleBoxFlat = (StyleBoxFlat)popupBackgroundTheme.Get("Panel/styles/panel");
        Color color = (Color)styleBoxFlat.Get("bg_color");
        return color;
    }

    private void SetPopupBackgroundColor(Color color)
    {
        StyleBoxFlat styleBoxFlat = (StyleBoxFlat)popupBackgroundTheme.Get("Panel/styles/panel");
        styleBoxFlat.Set("bg_color", color);
    }

    private Color GetPopupFontColor()
    {
        return (Color)popupFontTheme.Get("Button/colors/font_color");
    }

    private void SetPopupFontColor(Color color)
    {
        popupFontTheme.Set("RichTextLabel/colors/default_color", color);
        
        popupFontTheme.Set("Button/colors/font_color", color);
        popupFontTheme.Set("Button/colors/font_hover_color", color.Darkened(.25f));
        popupFontTheme.Set("Button/colors/font_pressed_color", color.Darkened(.5f));
    }

    public Color GetListFontColor()
    {
        Color fontColor = (Color)listFontTheme.Get("LineEdit/colors/font_color");
        return fontColor;
    }

    public void SetListFontColor(Color color)
    {
        listFontTheme.Set("LineEdit/colors/font_color", color);
        listFontTheme.Set("TextEdit/colors/font_color", color);
        listFontTheme.Set("TextEdit/colors/font_placeholder_color", new Color(color, color.A * .6f));
        listFontTheme.Set("RichTextLabel/colors/default_color", color);
    }

    public Color GetWheelButtonColor()
    {
        StyleBoxFlat styleBoxFlat = (StyleBoxFlat)wheelButtonTheme.Get("Button/styles/normal");
        return (Color)styleBoxFlat.Get("bg_color");
    }

    public void SetWheelButtonColor(Color color)
    {
        StyleBoxFlat normalResult = (StyleBoxFlat)wheelButtonTheme.Get("Button/styles/normal");
        normalResult.Set("bg_color", color);

        StyleBoxFlat hoverResult = (StyleBoxFlat)wheelButtonTheme.Get("Button/styles/hover");
        hoverResult.Set("bg_color", color.Darkened(.25f));

        StyleBoxFlat pressedResult = (StyleBoxFlat)wheelButtonTheme.Get("Button/styles/pressed");
        pressedResult.Set("bg_color", color.Darkened(.5f));
    }
}
