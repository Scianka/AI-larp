using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SimulationHistory
{
    public static string history = "";
    public static int entries_counter = 0;
    public static int new_entry_number;
    private static bool is_first_entry = true;

    public static void AddPromptToHistory(string new_prompt)
    {
        string new_entry = "";
        entries_counter++;
        if (is_first_entry)
        {
            new_entry = entries_counter.ToString() + ". User: [" + new_prompt + "]\n";
            is_first_entry = false;
        }
        else new_entry = "\n" + entries_counter.ToString() + ". User: [" + new_prompt + "]\n";
        history += new_entry;
    }

    public static void AddResponseToHistory(string new_response)
    {
        string new_entry = "";
        entries_counter++;
        new_entry = entries_counter.ToString() + ". AI: [" + new_response + "]";
        history += new_entry;
    }

    public static int GetNewEntryNumber()
    {
        new_entry_number = ++entries_counter;
        entries_counter--; Debug.Log(new_entry_number);
        return new_entry_number;
    }
}