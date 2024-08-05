using Godot;
using System.Collections.Generic;

public partial class OptionManager : Node
{
    // --------------------------------
	//			VARIABLES	
    // --------------------------------

	private List<Control> createdOptions = new List<Control>();

    [Export]
    private Color primaryColor;
    [Export]
    private Color secondaryColor;

	private bool wheelSpinning;

    // --------------------------------
    //			PROPERTIES	
    // --------------------------------

    public static OptionManager Instance { get; private set; }
    public List<Control> CreatedOptions {  get { return createdOptions; } }

	public Color PrimaryColor 
	{
		get => primaryColor; set => primaryColor = value;
	}
	public Color SecondaryColor 
	{ 
		get => secondaryColor; set => secondaryColor = value;
	}

	public bool WheelSpinning { get => wheelSpinning; set => wheelSpinning = value; }

    // --------------------------------
    //		STANDARD FUNCTIONS
    // --------------------------------

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
