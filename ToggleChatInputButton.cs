using Godot;
using System.Collections.Generic;

public partial class ToggleChatInputButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Export]
    private TwitchChatFeed feed;
    private OptionManager optionManager;
    private bool toggleChatInput = false;
    private List<string> users = new List<string>();

    // --------------------------------
    //		STANDARD FUNCTIONS	
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

    /// <summary>
    /// Toggles the variable to listen to the chat or not
    /// Upon toggling off, clears the tracked list of users
    /// </summary>
    private void OnButtonPress()
    {
        toggleChatInput = !toggleChatInput;

        if(!toggleChatInput)
        {
            users.Clear();
        }
        optionManager.TwitchInfoArea.Visible = toggleChatInput;
    }

    // --------------------------------
    //		    CHAT LOGIC	
    // --------------------------------

    /// <summary>
    /// Checks the incoming chat logs to determine if a vote was made on an existing option and updates the weight if the user has not already voted previously
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnMessage(string sender, string message)
    {
        if (!toggleChatInput)
        {
            return;
        }

        foreach (Option option in optionManager.CreatedOptions)
        {
            if((message.ToLower()).Contains(option.OptionName.ToLower()) && !users.Contains(sender)) 
            {
                users.Add(sender);
                ++option.OptionWeight;
                option.UpdateOptionFields();
                return;
            }
        }
    }
}
