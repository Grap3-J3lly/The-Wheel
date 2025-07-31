using Godot;
using System;

public partial class AudioControl : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Export]
	private HSlider volumeControl;
    [Export]
    private RichTextLabel volumeDisplay;

    private const int CONST_BusID = 0;
    private const float CONST_DecimalToPercentMultiplier = 100f;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        // Assigns value to linear version of current Volume
		volumeControl.SetValueNoSignal(Mathf.DbToLinear(AudioServer.GetBusVolumeDb(CONST_BusID)));
        volumeControl.ValueChanged += OnVolumeChange;
	}

    // --------------------------------
    //		SLIDER FUNCTIONS	
    // --------------------------------

    /// <summary>
    /// Assigns the volume of the application based on the value the slider has changed to
    /// </summary>
    /// <param name="value"></param>
    private void OnVolumeChange(double value)
	{
		AudioServer.SetBusVolumeDb(CONST_BusID, (float)Mathf.LinearToDb(value));
        volumeDisplay.Text = Mathf.RoundToInt((CONST_DecimalToPercentMultiplier * value)).ToString();
	}
}
