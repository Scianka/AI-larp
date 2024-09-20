using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PanelSwitch : MonoBehaviour
{
    public PageSwitch PageSwitch_script;
    private TMP_InputField promptIF;
    public bool is_promptIF_active = false;

    private void Start()
    {
        promptIF = GetComponent<TMP_InputField>();
        promptIF.DeactivateInputField();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete)) // need to choose other key
        {
            // need to add a behaviour in case of clicking the IF instead of pressing a key
            // can_switch at PageSwitch works weridly at start
            // a better input block is required for IF - or I can just leave it
            is_promptIF_active = !is_promptIF_active;
            if (is_promptIF_active)
            {
                promptIF.ActivateInputField();
                PageSwitch_script.can_switch = false;
            }
            else
            {
                promptIF.DeactivateInputField();
                PageSwitch_script.can_switch = true;
            }
        }
    }
}
