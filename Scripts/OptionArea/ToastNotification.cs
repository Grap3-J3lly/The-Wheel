using Godot;
using System;

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
		Position = startLocation;
        PlayToastAnimation();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _ExitTree()
    {
        base._ExitTree();
        PopupManager.Instance.Visible = false;
    }

    public void ChangeText(string userName)
    {
        toastMessage.Text = userName + defaultMessage;
    }

    private void PlayToastAnimation()
    {
        Tween tween = CreateTween().SetParallel(true);
        tween.TweenProperty(this, "position", finalLocation, startDuration);
        tween.Chain().TweenProperty(this, "position", Position, hangDuration);
        tween.Chain().TweenProperty(this, "position", startLocation, endDuration);
        tween.Chain().TweenCallback(Callable.From(QueueFree));
    }
}
