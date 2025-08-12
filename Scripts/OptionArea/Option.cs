using Godot;
using Godot.Collections;

public partial class Option : Control
{
    // --------------------------------
    //			VARIABLES	
    // --------------------------------
    [Export]
    private DisableButton optionEnabledField;
    [Export]
    private OptionName optionNameField;
    [Export]
    private OptionWeight optionWeightField;

    private bool optionEnabled = true;
	private string optionName = "";
	private int optionWeight = 1;
	private ProgressBar optionProgressBar;

    // Const Values
    private const int CONST_DefaultWeight = 1;

    private const int CONST_Index_OptionName = 0;
    private const int CONST_Index_OptionEnabled = 1;
    private const int CONST_Index_OptionWeight = 2;

    // --------------------------------
    //			PROPERTIES	
    // --------------------------------

    public DisableButton OptionEnabledField { get => optionEnabledField; }
    public OptionName OptionNameField {  get => optionNameField; }
    public OptionWeight OptionWeightField { get => optionWeightField; }
    public bool OptionEnabled { get => optionEnabled; set => optionEnabled = value; }
    public string OptionName { get => optionName; set => optionName = value; }
    public int OptionWeight { get => optionWeight; set => optionWeight = value; }
    public ProgressBar OptionProgressBar { get => optionProgressBar; set => optionProgressBar = value; }

    // --------------------------------
    //		STANDARD FUNCTIONS	
    // --------------------------------

    public override void _Ready()
    {
        base._Ready();
    }

    // --------------------------------
    //		OPTION FUNCTIONS	
    // --------------------------------

    /// <summary>
    /// Assigns the weight of the option back to 1, updates the UI fields
    /// </summary>
    public void ResetDefaultWeight()
    {
        OptionWeight = CONST_DefaultWeight;
        UpdateOptionFields();
    }

    /// <summary>
    /// Updates teh weight and name fields to reflect the variables behind the scenes
    /// </summary>
    public void UpdateOptionFields()
    {
        optionWeightField.UpdateOptionWeightField();
        optionNameField.UpdateOptionNameField();
    }

    /// <summary>
    /// Used in the Save System, this returns all data necessary for saving the current list of options
    /// </summary>
    /// <returns>An array of objects (a bool, a string, and an int)</returns>
    public Godot.Collections.Array GetOptionData()
    {
        Godot.Collections.Array result = new Godot.Collections.Array();

        result.Add(optionName);
        result.Add(optionEnabled);
        result.Add(optionWeight);

        return result;
    }

    /// <summary>
    /// Verifies if any option is missing its designated progress bar
    /// </summary>
    /// <param name="options"></param>
    /// <returns>True if any assigned optionProgressBar value is null</returns>
    public static bool CheckForMissingProgressBar(Array<Option> options)
    {
        foreach (Option option in options)
        {
            if (option.optionProgressBar == null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Loops through each option in a given list, and destroys their optionProgressBar values before assigning the value to null
    /// </summary>
    /// <param name="options"></param>
    public static void RemoveAllProgressBars(Array<Option> options)
    {
        foreach (Option option in options)
        {
            if (option.optionProgressBar != null)
            {
                option.optionProgressBar.QueueFree();
            }
            option.optionProgressBar = null;
        }
    }

    /// <summary>
    /// Deletes an options ProgressBar before deleting the option itself
    /// </summary>
    /// <param name="option"></param>
    public static void DeleteOption(Option option)
    {
        option.optionProgressBar.QueueFree();
        option.QueueFree();
    }

    /// <summary>
    /// Returns the sum weight of all options in a given list
    /// </summary>
    /// <param name="options"></param>
    /// <returns>The sum weight of the given list</returns>
    public static int GetTotalWeight(Array<Option> options)
    {
        int result = 0;
        foreach (Option option in options)
        {
            result += option.optionWeight;
        }
        return result;
    }

    /// <summary>
    /// Creates an option object based off given data, a template, and a parent value
    /// </summary>
    /// <param name="optionData"></param>
    /// <param name="optionTemplate"></param>
    /// <param name="optionParent"></param>
    /// <returns>The newly created option</returns>
    public static Option CreateOptions(Array optionData, PackedScene optionTemplate, Control optionParent)
    {
        Option newOption = (Option)optionTemplate.Instantiate();
        optionParent.AddChild(newOption);
        newOption.optionName = (string)optionData[CONST_Index_OptionName];
        newOption.optionNameField.Text = newOption.OptionName;

        newOption.optionEnabled = (bool)optionData[CONST_Index_OptionEnabled];
        newOption.optionEnabledField.Enabled = newOption.OptionEnabled;
        newOption.optionEnabledField.ButtonPressed = newOption.OptionEnabled;
        newOption.GetChild<DisableButton>(0).ToggleCheckmark(newOption.OptionEnabled);

        newOption.optionWeight = (int)optionData[CONST_Index_OptionWeight];
        newOption.optionWeightField.Text = newOption.OptionWeight.ToString();

        // Assign Focus
        newOption.AssignFocus(optionParent);

        return newOption;
    }

    // --------------------------------
    //		    FOCUS LOGIC
    // --------------------------------

    public void AssignFocus(Control optionParent)
    {
        GameManager gameManager = GameManager.Instance;
        Array<Node> allOptions = optionParent.GetChildren();
        RemoveOptionButton removeButton = GetChild<RemoveOptionButton>(3);

        FocusMode = FocusModeEnum.All;

        optionEnabledField.FocusNeighborRight = optionNameField.GetPath();
        optionEnabledField.FocusNext = optionNameField.GetPath();
        optionEnabledField.FocusNeighborBottom = gameManager.Option_BottomEnd.GetPath();

        optionNameField.FocusNeighborLeft = optionEnabledField.GetPath();
        optionNameField.FocusPrevious = optionEnabledField.GetPath();
        optionNameField.FocusNeighborRight = optionWeightField.GetPath();
        optionNameField.FocusNext = optionWeightField.GetPath();
        optionNameField.FocusNeighborBottom = gameManager.Option_BottomEnd.GetPath();

        optionWeightField.FocusNeighborLeft = optionNameField.GetPath();
        optionWeightField.FocusPrevious = optionNameField.GetPath();
        optionWeightField.FocusNeighborRight = removeButton.GetPath();
        optionWeightField.FocusNext = removeButton.GetPath();
        optionWeightField.FocusNeighborBottom = gameManager.Option_BottomEnd.GetPath();

        removeButton.FocusNeighborLeft = optionWeightField.GetPath();
        removeButton.FocusPrevious = optionWeightField.GetPath();
        removeButton.FocusNeighborRight = gameManager.Option_RightEnd.GetPath();
        removeButton.FocusNext = gameManager.Option_RightEnd.GetPath();
        removeButton.FocusNeighborBottom = gameManager.Option_BottomEnd.GetPath();

        // If Option is First Option
        if (allOptions.Count == 0)
        {
            // use leftBegin
            optionEnabledField.FocusNeighborLeft = gameManager.Option_LeftBegin.GetPath();
            optionEnabledField.FocusPrevious = gameManager.Option_LeftBegin.GetPath();

            // use topBegin
            optionEnabledField.FocusNeighborTop = gameManager.Option_TopBegin.GetPath();
            optionNameField.FocusNeighborTop = gameManager.Option_TopBegin.GetPath();
            optionWeightField.FocusNeighborTop = gameManager.Option_TopBegin.GetPath();
            removeButton.FocusNeighborTop = gameManager.Option_TopBegin.GetPath();
            return;
        }

        Option previousOption = (Option)allOptions[allOptions.Count - 1];
        RemoveOptionButton prevRemoveButton = previousOption.GetChild<RemoveOptionButton>(3);

        // Update previous option to point to new option
        previousOption.OptionEnabledField.FocusNeighborBottom = optionWeightField.GetPath();
        previousOption.OptionNameField.FocusNeighborBottom = optionNameField.GetPath();
        previousOption.OptionWeightField.FocusNeighborBottom = optionWeightField.GetPath();
        prevRemoveButton.FocusNeighborBottom = removeButton.GetPath();

        prevRemoveButton.FocusNeighborRight = optionEnabledField.GetPath();
        prevRemoveButton.FocusNext = optionEnabledField.GetPath();

        // Update new option to point to previous option
        optionEnabledField.FocusNeighborTop = previousOption.OptionEnabledField.GetPath();
        optionNameField.FocusNeighborTop = previousOption.OptionNameField.GetPath();
        optionWeightField.FocusNeighborTop = previousOption.OptionWeightField.GetPath();
        removeButton.FocusNeighborTop = prevRemoveButton.GetPath();

        optionEnabledField.FocusNeighborLeft = prevRemoveButton.GetPath();
        optionEnabledField.FocusPrevious = prevRemoveButton.GetPath();

    }
}