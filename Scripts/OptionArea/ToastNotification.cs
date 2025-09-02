using Godot;
using System;
using System.Threading.Tasks;

public partial class ToastNotification : Panel
{

    // --------------------------------
    //			VARIABLES
    // --------------------------------
    [Export]
    private Vector2 startLocation;
    [Export]
	private Vector2 finalLocation;

	[Export]
	private RichTextLabel toastMessage;
	[Export]
	private string defaultMessage = " Voted!";

    [Export]
    private float startDuration = 2.0f;
    [Export]
    private float hangDuration = 3.0f;
    [Export]
    private float endDuration = 2.0f;

    // --------------------------------
    //		STANDARD LOGIC
    // --------------------------------

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        
        // PlayToastAnimation();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    
    public void Setup()
    {
        Control parent = GetParent<Control>();

        GD.Print($"ToastNotification.cs: Parent Position: {parent.Position}");
        GD.Print($"ToastNotification.cs: Parent Size: {parent.Size}");

        Vector2 finalPos = new Vector2(parent.Size.X/2 - Size.X/2, parent.Size.Y / 4);
        // Vector2 finalPos = new Vector2(parent.Size.X / 5, parent.Size.Y / 4);
        Vector2 startPos = finalPos + new Vector2(0, 100);

        GD.Print($"ToastNotification.cs: Start Position: {startPos}");
        GD.Print($"ToastNotification.cs: Final Position: {finalPos}");

        finalLocation = finalPos;
        startLocation = startPos;

        Position = startLocation;
    }

    public void ChangeText(string userName)
    {
        toastMessage.Text = userName + defaultMessage;
    }

    public async Task PlayToastAnimation()
    {
        Tween tween = CreateTween().SetParallel(true).SetTrans(Tween.TransitionType.Spring).SetEase(Tween.EaseType.InOut);
        // Tween tween = CreateTween().SetParallel(true).SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(this, "position", finalLocation, startDuration);
        await ToSignal(GetTree().CreateTimer(hangDuration), SceneTreeTimer.SignalName.Timeout);
        tween = CreateTween().SetParallel(true).SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.In);
        tween.TweenProperty(this, "position", startLocation, endDuration);
        // tween.Chain().TweenCallback(Callable.From(QueueFree));
        await ToSignal(GetTree().CreateTimer(endDuration), SceneTreeTimer.SignalName.Timeout);
        this.QueueFree();
    }
}
