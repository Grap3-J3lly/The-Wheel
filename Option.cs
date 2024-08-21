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

    public static void DeleteOption(Option option)
    {
        option.optionProgressBar.QueueFree();
        option.QueueFree();
    }

    public static int GetTotalWeight(List<Option> options)
    {
        int result = 0;
        foreach (Option option in options)
        {
            result += option.optionWeight;
        }
        return result;
    }

    public Godot.Collections.Array GetOptionData()
    {
        Godot.Collections.Array result = new Godot.Collections.Array();

        result.Add(optionEnabled);
        result.Add(optionName);
        result.Add(optionWeight);

        return result;
    }

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