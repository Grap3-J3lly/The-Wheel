using Godot;
using Godot.Collections;
using System;
using System.Net.Sockets;
using System.IO;

//setup:
//create oauth here: https://twitchapps.com/tmi/
//place it in user://twitch.config

public partial class TwitchChatFeed : Node
{
	
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    private bool ready = false;

    private Dictionary<string, string> twitchConfigs;

    public delegate void TwitchMessageCallback(string sender, string content);
    public TwitchMessageCallback listener;

    // --------------------------------
    //			PROPERTIES	
    // --------------------------------

    [Export]
    public bool EnableLogging { get; set; } = true;

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        EstablishTwitchConnection();
    }

    public override void _Process(double delta)
    {
        ReadChat();
    }

    public override void _ExitTree()
    {
        reader?.Dispose();
        writer?.Dispose();
        twitchClient?.Dispose();
        base._ExitTree();
    }

    // --------------------------------
    //		TWITCH CONNECTION	
    // --------------------------------

    /// <summary>
    /// If config exists, and is properly filled out, attempts to establish a connection with the Twitch client using TCP
    /// </summary>
    private void EstablishTwitchConnection()
    {
        if (!Godot.FileAccess.FileExists("user://twitch.config"))
        {
            var createdFile = Godot.FileAccess.Open("user://twitch.config", Godot.FileAccess.ModeFlags.Write);
            var templateJsonStr = Json.Stringify(new Dictionary<string, string>() { { "Username", "" }, { "OAuth", "" }, { "Target_Channel", "" } }, "\t");
            createdFile.StoreLine(templateJsonStr);
            createdFile.Close();
            GD.PrintErr("Twitch config is invalid.");
            return;
        }

        var loadedFile = Godot.FileAccess.Open("user://twitch.config", Godot.FileAccess.ModeFlags.Read);
        var loadedJson = loadedFile.GetAsText();
        twitchConfigs = (Dictionary<string, string>)Json.ParseString(loadedJson);

        #region make sure config is valid
        if (twitchConfigs.TryGetValue("Username", out string username))
        {
            if (username == "")
            {
                GD.PrintErr("Twitch config is invalid");
                return;
            }
        }
        else
        {
            GD.PrintErr("Twitch config is invalid");
            return;
        }
        if (twitchConfigs.TryGetValue("OAuth", out string oAuth))
        {
            if (username == "")
            {
                GD.PrintErr("Twitch config is invalid");
                return;
            }
        }
        else
        {
            GD.PrintErr("Twitch config is invalid");
            return;
        }
        if (twitchConfigs.TryGetValue("Target_Channel", out string channel))
        {
            if (username == "")
            {
                GD.PrintErr("Twitch config is invalid");
                return;
            }
            else
            {
                twitchConfigs["Target_Channel"] = channel.ToLower();
            }
        }
        else
        {
            GD.PrintErr("Twitch config is invalid");
            return;
        }
        #endregion

        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        Print("Client Created");

        writer.WriteLine($"PASS {twitchConfigs["OAuth"]}");
        writer.WriteLine($"NICK {twitchConfigs["Username"]}");
        writer.WriteLine($"USER {twitchConfigs["Username"]} 8 * :{twitchConfigs["Username"]}");
        writer.WriteLine($"JOIN #{twitchConfigs["Target_Channel"]}");
        writer.Flush();

        Print("Connection attempt complete");
        ready = true;
    }

    /// <summary>
    /// Communicates with the twitch client to maintain connection, reads in all chat messages and their corresponding sender
    /// </summary>
    private void ReadChat()
    {
        try
        {
            while (ready && twitchClient != null && twitchClient.Available > 0)
            {
                string message;
                if ((message = reader.ReadLine()) != null)
                {
                    if (message.Contains("PING"))
                    {
                        writer.WriteLine("PONG :tmi.twitch.tv\r\n");
                        writer.Flush();
                    }
                    if (message.Contains("PRIVMSG"))
                    {
                        //Print(message);
                        string sender = "";
                        sender = message.Substring(1, message.IndexOf('!') - 1);
                        message = message.Split($"PRIVMSG #{twitchConfigs["Target_Channel"]} :")[1];
                        Print($"Twitch Chat Received: ({sender}): {message}");
                        listener?.Invoke(sender, message);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Print($"{e.Message}\n{e.StackTrace}");
        }
    }

    /// <summary>
    /// Logs connection status to the console
    /// </summary>
    /// <param name="message"></param>
    private void Print(string message)
    {
        if (EnableLogging)
        {
            GD.Print(message);
        }
    }
}
