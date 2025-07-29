using Godot;

public partial class OpenPopupButton : Button
{
    // --------------------------------
    //		STANDARD FUNCTIONS
    // --------------------------------

    public override void _Ready()
	{
		this.Pressed += OnButtonPress;
	}

    // --------------------------------
    //			BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Creates a Menu Popup
    /// </summary>
    private void OnButtonPress()
	{
		PopupManager.Instance.CreatePopup(PopupManager.Instance.MenuPopup);
	}
}
