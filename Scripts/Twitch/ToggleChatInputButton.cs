using Godot;
using Godot.Collections;
using System.Text.Json.Nodes;
using System.Text.Json;

public partial class ToggleChatInputButton : Button
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

    [Export]
    private WSClient wsClient;
    private GameManager gameManager;
    private bool toggleChatInput = false;
    private Array<string> users = new Array<string>();

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

        gameManager = GameManager.Instance;
        Disabled = true;
        wsClient.ConnectedToServer += OnConnection;
    }

    // --------------------------------
    //		    BUTTON LOGIC	
    // --------------------------------

    /// <summary>
    /// Toggles the variable to listen to the chat or not
    /// Upon toggling off, clears the tracked list of users
    /// </summary>
    private void OnConnection()
    {
        Disabled = false;
        ToggleCheckbox(isVisible: true);
    }

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
        toggleChatInput = !toggleChatInput;
        gameManager.TwitchInfoArea.Visible = toggleChatInput;

        if(toggleChatInput)
        {
            wsClient.MessageReceived += OnMessage;
            wsClient.Send(WSClient.DoAction("EnableWheelRewards"));
            Theme = menuButtonTheme;
        }
        else
        {
            wsClient.MessageReceived -= OnMessage;
            wsClient.Send(WSClient.DoAction("DisableWheelRewards"));
            Theme = defaultTheme;
        }
    }

    // --------------------------------
    //		    CHAT LOGIC	
    // --------------------------------

    /// <summary>
    /// Checks the incoming chat logs to determine if a vote was made on an existing option and updates the weight if the user has not already voted previously
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnMessage(Variant message)
    {
        string messageText = message.ToString();
        // GD.Print($"ToggleChatInputButton.cs: {messageText}");

        JsonNode checkText = ParseJson(messageText, "event/type");

        if (checkText?.ToString() == "Action")
        {
            JsonNode parsedAction = ParseJson(messageText, "data/arguments/actionName");
            GD.Print($"ToggleChatInputButton.cs: Action Called: {parsedAction.ToString()}");
        }
        if (checkText?.ToString() == "ChatMessage")
        {
            JsonNode parsedText = ParseJson(messageText, "data/text");
            JsonNode parsedSender = ParseJson(messageText, "data/user/login");

            GD.Print($"ToggleChatInputButton.cs: Message Text: {parsedText.ToString()}");
        }

        // Need to split OnMessage up based off type of event coming in (Action vs. ChatMessage)
        // Setup event call to trigger logic in Game Manager


        // Don't care about bottom half rn

        string sender = "";

        foreach (Option option in gameManager.CreatedOptions)
        {
            if((messageText.ToLower()).Contains(option.OptionName.ToLower()) && !users.Contains(sender)) 
            {
                users.Add(sender);
                ++option.OptionWeight;
                option.UpdateOptionFields();
                return;
            }
        }
    }

    private JsonNode ParseJson(string messageToParse, string dataPath)
    {
        JsonNode root = JsonNode.Parse(messageToParse);
        
        if(root == null)
        {
            GD.Print($"ToggleChatInputButton.cs: Failed to Parse");
            return null;
        }

        JsonNode result = root.GetJsonNodeValueByString(dataPath);

        return result;
    }
}
