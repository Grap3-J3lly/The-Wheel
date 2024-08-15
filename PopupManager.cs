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

    private bool isCustomizationOpen = false;

    // --------------------------------
    //			PROPERTIES
    // --------------------------------

    public PackedScene SelectedOptionPopup { get => selectedOptionPopup; }
    public PackedScene MenuPopup { get => menuPopup; }

    public bool IsCustomizationOpen { get => isCustomizationOpen; set => isCustomizationOpen = value; }

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

	public void AssignWinningText(ColorRect popup, string winnerName)
	{
		RichTextLabel winText = popup.GetChild<RichTextLabel>(0);
		winText.Text = "[center]The Winning Choice Is:\n" + winnerName + "[/center]";
	}

    // --------------------------------
    //		GENERAL LOGIC
    // --------------------------------

    public ColorRect CreatePopup(PackedScene popup)
    {
        this.Visible = true;
        ColorRect newPopup = (ColorRect)popup.Instantiate();
        AddChild(newPopup);
        isCustomizationOpen = false;
        return newPopup;
    }

    public void ClosePopup(Control popupToRemove)
    {
        this.Visible = false;
        popupToRemove.QueueFree();
    }
}
