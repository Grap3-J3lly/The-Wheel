using Godot;
using System.Collections.Generic;

public partial class ToggleChatInputButton : Button
{
    [Export]
    private TwitchChatFeed feed;
    private OptionManager optionManager;
    private bool toggleChatInput = false;
    List<string> users = new List<string>();

    // --------------------------------
    //		    STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        base._Ready();
        this.Pressed += OnButtonPress;
        feed.listener += OnMessage;

        optionManager = OptionManager.Instance;
    }

    // --------------------------------
    //		    BUTTON LOGIC	
    // --------------------------------

    private void OnButtonPress()
    {
        toggleChatInput = !toggleChatInput;
        GD.Print("Button Toggled to: " + toggleChatInput);

        if(!toggleChatInput)
        {
            users.Clear();
        }
    }

    // --------------------------------
    //		    CHAT LOGIC	
    // --------------------------------

    private void OnMessage(string sender, string message)
    {
        if (!toggleChatInput)
        {
            return;
        }

        foreach (Option option in optionManager.CreatedOptions)
        {
            if(option.OptionName.ToLower() == message.ToLower() && !users.Contains(sender)) 
            {
                users.Add(sender);
                ++option.OptionWeight;
                option.UpdateOptionFields();
                return;
            }
        }
    }
}
