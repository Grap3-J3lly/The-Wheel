using Godot;
using System;

public partial class OpenPopupButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += OnButtonPress;
	}

	private void OnButtonPress()
	{
		PopupManager.Instance.CreatePopup(PopupManager.Instance.MenuPopup);
	}
}
