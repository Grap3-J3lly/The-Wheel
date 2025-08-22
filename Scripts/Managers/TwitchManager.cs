using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.Json.Nodes;

public partial class TwitchManager : Node
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    [Export]
    private WSClient wsClient;
    private GameManager gameManager;
    private bool toggleChatInput = false;
    private Array<string> users = new Array<string>();

    [Export]
    private ToggleChatInputButton toggleChatInputButton;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        gameManager = GameManager.Instance;
        wsClient.ConnectedToServer += OnConnection;
        gameManager.ToggleTwitch += ToggleInteractions;
    }

    /// <summary>
    /// Toggles the variable to listen to the chat or not
    /// Upon toggling off, clears the tracked list of users
    /// </summary>
    private void OnConnection()
    {
        toggleChatInputButton.Disabled = false;
        toggleChatInputButton.ToggleCheckbox(isVisible: true);
    }

    public void ToggleInteractions()
    {
        ToggleInteractions(!toggleChatInput);
    }

    public void ToggleInteractions(bool isActive)
    {
        toggleChatInput = isActive;
        gameManager.TwitchInfoArea.Visible = toggleChatInput;

        toggleChatInputButton.ToggleTheme(!toggleChatInput);
        if (toggleChatInput)
        {
            wsClient.MessageReceived += OnWebSocketMessage;
            wsClient.Send(WSClient.DoAction("EnableWheelRewards"));
        }
        else
        {
            wsClient.MessageReceived -= OnWebSocketMessage;
            wsClient.Send(WSClient.DoAction("DisableWheelRewards"));
            users.Clear();
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
    private void OnWebSocketMessage(Variant message)
    {
        string socketMessageString = message.ToString();
        // GD.Print($"ToggleChatInputButton.cs: {messageText}");

        JsonNode checkText = ParseJson(socketMessageString, "event/type");


        // Need to move Action checks to GameManager, need to create an event for when toggleChatInput becomes TRUE,
        // so Action checks can know when  to subscribe to the MessageReceived event from the websocket.
        if (checkText?.ToString() == "Action")
        {
            JsonNode parsedAction = ParseJson(socketMessageString, "data/arguments/actionName");
            GD.Print($"ToggleChatInputButton.cs: Action Called: {parsedAction.ToString()}");
            HandleAction();
        }
        if (checkText?.ToString() == "ChatMessage")
        {
            JsonNode parsedText = ParseJson(socketMessageString, "data/text");
            JsonNode parsedSender = ParseJson(socketMessageString, "data/user/login");

            GD.Print($"ToggleChatInputButton.cs: Message Text: {parsedText.ToString()}");
            HandleChatMessage(parsedText, parsedSender);
        }

        
    }

    private void HandleAction()
    {

    }

    private void HandleChatMessage(JsonNode parsedText, JsonNode parsedSender)
    {
        if (parsedText == null || parsedSender == null) return;
        CheckingForOptionMatch(parsedText.ToString(), parsedSender.ToString());
    }

    private void CheckingForOptionMatch(string chatMessage, string sender)
    {
        if (users.Contains(sender))
        {
            GD.Print($"ToggleChatInputButton.cs: User {sender} has already voted");
            return;
        }
        users.Add(sender);

        string[] optionNames = new string[gameManager.CreatedOptions.Count];
        foreach (Option option in gameManager.CreatedOptions)
        {
            optionNames[gameManager.CreatedOptions.IndexOf(option)] = option.OptionName;
        }

        int optionResult = OptionPicker(optionNames, chatMessage);
        if (optionResult != -1)
        {
            Option currentOption = gameManager.CreatedOptions[optionResult];
            users.Add(sender);
            ++currentOption.OptionWeight;
            currentOption.UpdateOptionFields();
        }
    }

    public int OptionPicker(string[] optionNames, string searchKey)
    {
        searchKey = searchKey.ToLower();
        for (int i = 0; i < optionNames.Length; i++)
        {
            optionNames[i] = optionNames[i].ToLower();
        }

        List<int> candidates = new List<int>();

        for (int i = 0; i < optionNames.Length; i++)
        {
            if (searchKey.Contains(optionNames[i]))
            {
                candidates.Add(i);
            }
        }

        if (candidates.Count == 0)
        {
            return -1;
        }

        int finalCandidate = candidates[0];
        int finalCandidateLength = optionNames[candidates[0]].Length;
        for (int i = 1; i < candidates.Count; i++)
        {
            int curCandidateLength = optionNames[candidates[i]].Length;
            if (curCandidateLength > finalCandidateLength)
            {
                finalCandidate = candidates[i];
                finalCandidateLength = curCandidateLength;
            }
        }


        return finalCandidate;
    }

    private JsonNode ParseJson(string messageToParse, string dataPath)
    {
        JsonNode root = JsonNode.Parse(messageToParse);

        if (root == null)
        {
            GD.Print($"ToggleChatInputButton.cs: Failed to Parse");
            return null;
        }

        JsonNode result = root.GetJsonNodeValueByString(dataPath);

        return result;
    }
}
