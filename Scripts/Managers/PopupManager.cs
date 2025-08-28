using Godot;
using Godot.Collections;
using System;

public partial class PopupManager : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    public enum PopupType
    {
        SelectedOption,
        Menu,
        Toast
    }
    [Export]
    private Dictionary<PopupType, PackedScene> popupTypes = new Dictionary<PopupType, PackedScene> ();

    private bool isCustomizationOpen = false;

    private const int CONST_MaxRTLCount = 1;

    [Signal]
    public delegate void CreateToastEventHandler(string userName);

    // --------------------------------
    //			PROPERTIES
    // --------------------------------
    public bool IsCustomizationOpen { get => isCustomizationOpen; set => isCustomizationOpen = value; }

    public static PopupManager Instance { get; private set; }

    // --------------------------------
    //		STANDARD FUNCTIONS
    // --------------------------------

    public override void _Ready()
	{
		base._Ready();
		Instance = this;
		Visible = false;

        CreateToast += CreateToastNotification;
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
                    if (rtlCount > CONST_MaxRTLCount)
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

    private void CreateToastNotification(string userName)
    {
        ToastNotification newToast = (ToastNotification)CreatePopup(PopupType.Toast);
        newToast.ChangeText(userName);
    }

    /// <summary>
    /// Creates a popup of the given type
    /// </summary>
    /// <param name="popup"></param>
    /// <returns>The panel created</returns>
    public Panel CreatePopup(PopupType typeToSpawn)
    {
        Visible = true;
        Panel newPopup = (Panel)popupTypes[typeToSpawn].Instantiate();
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
        Visible = false;
        popupToRemove.QueueFree();
    }
}
