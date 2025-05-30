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

    /// <summary>
    /// Assigns the text value on the winning text of the popup to match the given result of the spin
    /// </summary>
    /// <param name="popup"></param>
    /// <param name="winnerName"></param>
	public void AssignWinningText(Panel popup, string winnerName)
	{
        int rtlCount = 0;
        RichTextLabel winText = null;
        foreach(Variant child in popup.GetChildren())
        {
            try
            {
                if (child.As<RichTextLabel>() != null)
                {
                    ++rtlCount;
                    if (rtlCount > 1)
                    {
                        winText = child.As<RichTextLabel>();
                    }
                }
            }
            catch (Exception ex)
            {
                GD.Print("Continuing Loop");
                continue;
            }
            
            
        }

        if (winText != null)
        {
            winText.Text = "[center]" + winnerName + "[/center]";
        }
        else
        {
            GD.PrintErr("No WinText Found");
        }
	}

    // --------------------------------
    //		GENERAL LOGIC
    // --------------------------------

    /// <summary>
    /// Creates a popup of the given type
    /// </summary>
    /// <param name="popup"></param>
    /// <returns>The panel created</returns>
    public Panel CreatePopup(PackedScene popup)
    {
        this.Visible = true;
        Panel newPopup = (Panel)popup.Instantiate();
        AddChild(newPopup);
        isCustomizationOpen = false;
        return newPopup;
    }

    /// <summary>
    /// Closes the given popup and sets its visibility to false
    /// </summary>
    /// <param name="popupToRemove"></param>
    public void ClosePopup(Control popupToRemove)
    {
        this.Visible = false;
        popupToRemove.QueueFree();
    }
}
