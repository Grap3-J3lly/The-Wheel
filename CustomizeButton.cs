using Godot;
using System.Collections.Generic;

public partial class CustomizeButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    [Export]
	private Panel menuBackground;

	[Export]
	private PackedScene customizationAreaTemplate;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		this.Pressed += OnButtonPressed;
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

	/// <summary>
	/// Resize the menu background to fit the customization elements, instantiate the Customization Area
	/// </summary>
    public void OnButtonPressed()
	{
        GD.Print("Customization Button Pressed");
        if (PopupManager.Instance.IsCustomizationOpen) { return; }
		GD.Print("Creating Customization Area");
		Control customizationArea = (Control)customizationAreaTemplate.Instantiate();
		menuBackground.AddChild(customizationArea);

		menuBackground.Size = new Vector2(menuBackground.Size.X, menuBackground.Size.Y);
		menuBackground.Position = new Vector2(menuBackground.Position.X - (customizationArea.Size.X/4), menuBackground.Position.Y);
        GD.Print("Finished Creating Customization Area");
    }
}
