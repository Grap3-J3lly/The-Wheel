using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Text.Json.Nodes;

public partial class ToggleChatInputButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    [Export]
    private TwitchManager twitchManager;

    [Export]
    private TextureRect checkmark;

    [Export]
    private Theme defaultTheme;
    [Export]
    private Theme menuButtonTheme;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        if(OS.GetName() == "Android")
        {
            Visible = false;
        }

        base._Ready();
        Pressed += OnPress;

        Disabled = true;
        
    }

    // --------------------------------
    //		    BUTTON LOGIC	
    // --------------------------------

    public void ToggleCheckbox(bool isVisible)
    {
        if (!isVisible)
        {
            checkmark.Modulate = new Color(255, 255, 255, 0);
        }
        else
        {
            checkmark.Modulate = new Color(255, 255, 255, 1);
        }
    }

    public void OnPress()
    {
        if (Disabled) return;
        twitchManager.ToggleInteractions();
    }

    public void ToggleTheme(bool useDefault)
    {
        Theme = useDefault ? defaultTheme : menuButtonTheme;
    }

}
