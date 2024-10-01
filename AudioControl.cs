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

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        // Assigns value to linear version of current Volume
		volumeControl.SetValueNoSignal(Mathf.DbToLinear(AudioServer.GetBusVolumeDb(0)));
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
		AudioServer.SetBusVolumeDb(0, (float)Mathf.LinearToDb(value));
        volumeDisplay.Text = Mathf.RoundToInt((100 * value)).ToString();
	}
}
