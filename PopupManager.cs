using Godot;
using System;

public partial class PopupManager : Control
{
	[Export] 
	private PackedScene selectedOptionPopup;

    public static PopupManager Instance { get; private set; }

    public override void _Ready()
	{
		base._Ready();
		Instance = this;
		this.Visible = false;
	}

	public void CreateWinPopup(string winnerName)
	{
		this.Visible = true;
		TextureRect newPopup = (TextureRect)selectedOptionPopup.Instantiate();
		AddChild(newPopup);
		AssignWinningText(newPopup, winnerName);
	}

	public void ClosePopup(Control popupToRemove)
	{
		this.Visible = false;
		popupToRemove.QueueFree();
	}

	public void AssignWinningText(TextureRect popup, string winnerName)
	{
		RichTextLabel winText = popup.GetChild<RichTextLabel>(0);
		winText.Text = "[center]The Winning Choice Is:\n" + winnerName + "[/center]";
	}
}
