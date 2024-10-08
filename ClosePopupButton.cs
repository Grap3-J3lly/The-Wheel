using Godot;
using System;

public partial class ClosePopupButton : Button
{
    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
	{
		this.Pressed += OnButtonPress;
	}

    // --------------------------------
    //		    BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Closes the current popup
    /// </summary>
    private void OnButtonPress()
	{
		PopupManager.Instance.ClosePopup(GetParent<Control>());
        GameManager.Instance.SpinButton.idleSpinning = true;
	}
}
