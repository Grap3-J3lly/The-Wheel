using Godot;
using Godot.Collections;
using System.Collections.Generic;

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

    // --------------------------------
    //			PROPERTIES	
    // --------------------------------

    public bool OptionEnabled { get => optionEnabled; set => optionEnabled = value; }
    public string OptionName { get => optionName; set => optionName = value; }
    public int OptionWeight { get => optionWeight; set => optionWeight = value; }
    public ProgressBar OptionProgressBar { get => optionProgressBar; set => optionProgressBar = value; }

    // --------------------------------
    //		OPTION FUNCTIONS	
    // --------------------------------

    /// <summary>
    /// Assigns the weight of the option back to 1, updates the UI fields
    /// </summary>
    public void ResetDefaultWeight()
    {
        OptionWeight = 1;
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

        result.Add(optionEnabled);
        result.Add(optionName);
        result.Add(optionWeight);

        return result;
    }

    /// <summary>
    /// Verifies if any option is missing its designated progress bar
    /// </summary>
    /// <param name="options"></param>
    /// <returns>True if any assigned optionProgressBar value is null</returns>
    public static bool CheckForMissingProgressBar(List<Option> options)
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
    public static void RemoveAllProgressBars(List<Option> options)
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
    public static int GetTotalWeight(List<Option> options)
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
        newOption.OptionEnabled = (bool)optionData[0];
        newOption.optionEnabledField.Enabled = newOption.OptionEnabled;
        newOption.OptionName = (string)optionData[1];
        newOption.optionNameField.Text = newOption.OptionName;
        newOption.OptionWeight = (int)optionData[2];
        newOption.optionWeightField.Text = newOption.OptionWeight.ToString();

        return newOption;
    }
}