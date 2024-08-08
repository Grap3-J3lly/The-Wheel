using Godot;
using System;

public partial class ClosePopupButton : Button
{
	[Export]
	private Control popupArea;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += OnButtonPress;
	}

	private void OnButtonPress()
	{
		popupArea.Visible = false;
	}
}
