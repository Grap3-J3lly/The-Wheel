using Godot;

public partial class ClosePopupButton : Button
{
    private const bool CONST_DefaultIdleSpinning = true;

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
        GameManager.Instance.SpinButton.idleSpinning = CONST_DefaultIdleSpinning;
	}
}
