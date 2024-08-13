using Godot;
using System;

public partial class PopupManager : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Export] 
	private PackedScene selectedOptionPopup;
    [Export]
    private PackedScene menuPopup;
    [Export]
    private TextureRect fadedBackground;

    // --------------------------------
    //			PROPERTIES
    // --------------------------------

    public PackedScene SelectedOptionPopup { get => selectedOptionPopup; }
    public PackedScene MenuPopup { get => menuPopup; }

    public static PopupManager Instance { get; private set; }

    // --------------------------------
    //		STANDARD FUNCTIONS
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		Instance = this;
		this.Visible = false;
	}

    // --------------------------------
    //		WIN POPUP LOGIC
    // --------------------------------

	public void AssignWinningText(TextureRect popup, string winnerName)
	{
		RichTextLabel winText = popup.GetChild<RichTextLabel>(0);
		winText.Text = "[center]The Winning Choice Is:\n" + winnerName + "[/center]";
	}

    // --------------------------------
    //		GENERAL LOGIC
    // --------------------------------

    public TextureRect CreatePopup(PackedScene popup)
    {
        this.Visible = true;
        TextureRect newPopup = (TextureRect)popup.Instantiate();
        AddChild(newPopup);
        return newPopup;
    }

    public void ClosePopup(Control popupToRemove)
    {
        this.Visible = false;
        popupToRemove.QueueFree();
    }
}
