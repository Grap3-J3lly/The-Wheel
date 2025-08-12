using Godot;
using System;

public partial class WSClient : Node
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------

    [Export]
	private string[] handshakeHeaders;
	[Export]
	private string[] supportedProtocols;
	private TlsOptions tlsOptions = null;
	private WebSocketPeer socket = new WebSocketPeer();
	private WebSocketPeer.State lastState = WebSocketPeer.State.Closed;

    // --------------------------------
    //			SIGNALS	
    // --------------------------------

    [Signal]
	public delegate void ConnectedToServerEventHandler();
    [Signal]
    public delegate void ConnectionClosedEventHandler();
	[Signal]
	public delegate void MessageReceivedEventHandler(Variant message);

    // --------------------------------
    //		CONNECTION LOGIC
    // --------------------------------
	private int ConnectToUrl(string url)
	{
		socket.SupportedProtocols = supportedProtocols;
		socket.HandshakeHeaders = handshakeHeaders;
		Error err = socket.ConnectToUrl(url, tlsOptions);
		if (err != Error.Ok)
		{
			return (int)err;
		}

		lastState = socket.GetReadyState();
		return (int)Error.Ok;
	}

	private int Send(string message)
	{
		return (int)socket.SendText(message);
	}

	private Variant GetMessage()
	{
		Variant message = new Variant();
		if(socket.GetAvailablePacketCount() < 1)
		{
			return message;
		}
		byte[] pkt = socket.GetPacket();

		if(socket.WasStringPacket())
		{
			GD.Print(pkt.GetStringFromUtf8());
			return pkt.GetStringFromUtf8();
		}
		return pkt;
	}

	private void Close(int code = 1000, string reason = "")
	{
		socket.Close(code, reason);
		lastState = socket.GetReadyState();
	}

	private void Clear()
	{
		socket = new WebSocketPeer();
		lastState = socket.GetReadyState();
	}

	private WebSocketPeer GetSocket()
	{
		return socket;
	}

	private void Poll()
	{
		if(socket.GetReadyState() != WebSocketPeer.State.Closed)
		{
			socket.Poll();
		}

		WebSocketPeer.State state = socket.GetReadyState();

		if (lastState != state)
		{
			lastState = state;
			if (state == WebSocketPeer.State.Open)
			{
				EmitSignal(SignalName.ConnectedToServer);
				GD.Print($"WSClient.cs: Websocket Connected");
			}
			else if (state == WebSocketPeer.State.Closed)
			{
				EmitSignal(SignalName.ConnectionClosed);
			}
			while (socket.GetReadyState() == WebSocketPeer.State.Open && socket.GetAvailablePacketCount() > 0)
			{
				EmitSignal(SignalName.MessageReceived, GetMessage());
			}
		}
	}

    public override void _Ready()
    {
        if(socket.ConnectToUrl("ws://localhost:8080") != Error.Ok)
		{
			GD.Print($"WSClient.cs: Unable to connect");
			SetProcess(false);
		}
		// Redo the JSON bits, PLEASE
		ConnectedToServer += () => Send(
            """
			{
			\"request\": \"Subscribe\",
				\"id\": \"my-subscribe-id\",
				\"events\": {
					\"raw\": [
						\"Action\",
					]
				},
			}
			"""
    );

    }

	public override void _Process(double delta)
	{
		Poll();
	}
}