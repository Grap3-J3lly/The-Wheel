using Godot;
// using Godot.Collections;
using System.Collections.Generic;
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
    private Dictionary<string, Option> userVotes = new Dictionary<string, Option>();

    [Export]
    private ToggleChatInputButton toggleChatInputButton;

    [Export]
    private string wsGlobalVar_TwitchRewardUser = "rewardUser";

    [Export]
    private int priorityUserWeightValue = 10;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        gameManager = GameManager.Instance;
        wsClient.ConnectedToServer += OnConnection;
        gameManager.ToggleTwitch += ToggleInteractions;
        gameManager.ClearWeights += ClearUserVotes;
    }

    // --------------------------------
    //	   TWITCH CONNECTION LOGIC	
    // --------------------------------

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
            userVotes.Clear();
        }
    }

    /// <summary>
    /// Checks the incoming chat logs to determine if a vote was made on an existing option and updates the weight if the user has not already voted previously
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnWebSocketMessage(Variant message)
    {
        string socketMessageString = message.ToString();
        // GD.Print($"TwitchManager.cs: SocketMessageString: {socketMessageString}");
        JsonNode checkText = ParseJson(socketMessageString, "event/type");

        // Need to move Action checks to GameManager, need to create an event for when toggleChatInput becomes TRUE,
        // so Action checks can know when  to subscribe to the MessageReceived event from the websocket.
        if (checkText?.ToString() == "Action")
        {
            HandleAction(socketMessageString);
            return;
        }
        if (checkText?.ToString() == "ChatMessage")
        {
            HandleChatMessage(socketMessageString);
            return;
        }

        checkText = ParseJson(socketMessageString, "variables");
        if(checkText != null)
        {
            // GD.Print($"TwitchManager.cs: CheckText for Variables: {checkText.ToString()}");
            TriggerAction_RemoveVote(socketMessageString);
        }
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

    // --------------------------------
    //		    ACTION LOGIC	
    // --------------------------------

    private void HandleAction(string socketMessageString)
    {
        JsonNode parsedAction = ParseJson(socketMessageString, "data/arguments/actionName");
        GD.Print($"TwitchManager.cs: Action Called: {parsedAction.ToString()}");

        switch (parsedAction.ToString())
        {
            case "Remove Vote":
                wsClient.Send(WSClient.GetGlobal(wsGlobalVar_TwitchRewardUser));
                break;
            default: GD.Print($"Not compatible action");
                break;
        }
    }

    private void TriggerAction_RemoveVote(string socketMessageString)
    {
        JsonNode parsedUser = ParseJson(socketMessageString, "variables/" + wsGlobalVar_TwitchRewardUser + "/value");
                
        GD.Print($"TwitchManager.cs: Action Triggered: Remove Vote - ParsedUser: {parsedUser.ToString()}");
        string previousVoterName = parsedUser.ToString();
        if (userVotes.ContainsKey(previousVoterName))
        {
            Option option = userVotes[previousVoterName];
            option.UpdateOptionFields(option.OptionWeight - priorityUserWeightValue);
            userVotes.Remove(previousVoterName);
        }
        else 
        {
            GD.PushWarning($"TwitchManager.cs: User Not Found in Dictionary, but User attempted to Remove Vote anyway");
        }
    }

    // --------------------------------
    //		    CHAT LOGIC	
    // --------------------------------

    public void ClearUserVotes()
    {
        userVotes.Clear();
    }

    private void HandleChatMessage(string socketMessageString)
    {
        JsonNode parsedText = ParseJson(socketMessageString, "data/text");
        JsonNode parsedSender = ParseJson(socketMessageString, "data/user/login");

        GD.Print($"ToggleChatInputButton.cs: Message Text: {parsedText.ToString()}");

        if (parsedText == null || parsedSender == null) return;
        CheckingForOptionMatch(parsedText.ToString(), parsedSender.ToString());
    }

    private void CheckingForOptionMatch(string chatMessage, string sender)
    {
        if (userVotes.ContainsKey(sender))
        {
            GD.Print($"TwitchManager.cs: User {sender} has already voted");
            return;
        }

        string[] optionNames = new string[gameManager.CreatedOptions.Count];
        foreach (Option option in gameManager.CreatedOptions)
        {
            optionNames[gameManager.CreatedOptions.IndexOf(option)] = option.OptionName;
        }

        int optionResult = OptionPicker(optionNames, chatMessage);
        if (optionResult != -1)
        {
            Option currentOption = gameManager.CreatedOptions[optionResult];
            userVotes.Add(sender, currentOption);
            currentOption.UpdateOptionFields(currentOption.OptionWeight + priorityUserWeightValue);
            // Spawn Toast Notification
            PopupManager.Instance.EmitSignal(PopupManager.SignalName.CreateToast, sender);
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
}
