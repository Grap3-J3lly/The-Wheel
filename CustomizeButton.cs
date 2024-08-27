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
		if(PopupManager.Instance.IsCustomizationOpen) { return; }
		menuBackground.Size = new Vector2(menuBackground.Size.X * 2, menuBackground.Size.Y);
		menuBackground.Position = new Vector2(menuBackground.Position.X/2, menuBackground.Position.Y);

		Control customizationArea = (Control)customizationAreaTemplate.Instantiate();
		menuBackground.AddChild(customizationArea);
	}
}
