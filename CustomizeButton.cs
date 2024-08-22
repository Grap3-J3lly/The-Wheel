using Godot;
using System.Collections.Generic;

public partial class CustomizeButton : Button
{
	[Export]
	private Panel menuBackground;

	[Export]
	private PackedScene customizationAreaTemplate;

	public override void _Ready()
	{
		base._Ready();
		this.Pressed += OnButtonPressed;
	}


	public void OnButtonPressed()
	{
		if(PopupManager.Instance.IsCustomizationOpen) { return; }
		menuBackground.Size = new Vector2(menuBackground.Size.X * 2, menuBackground.Size.Y);
		menuBackground.Position = new Vector2(menuBackground.Position.X/2, menuBackground.Position.Y);

		Control customizationArea = (Control)customizationAreaTemplate.Instantiate();
		menuBackground.AddChild(customizationArea);
	}
}
