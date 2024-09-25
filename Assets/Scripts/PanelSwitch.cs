using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Drawing;

public class PanelSwitch : MonoBehaviour
{
    public PageSwitch PageSwitch_script;
    public GameObject device_lights;
    public Image light_bulb;
    private TMP_InputField promptIF;
    private bool prompt_activated = false;

    public bool can_switch { get; set; } = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        promptIF = GetComponent<TMP_InputField>();
        promptIF.enabled = false;

        device_lights.SetActive(false);
        light_bulb.enabled = false;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1) && can_switch)
        {
            if (!prompt_activated)
            {
                promptIF.enabled = true;
                promptIF.ActivateInputField();
                PageSwitch_script.can_switch = false;
                light_bulb.enabled = true;
                device_lights.SetActive(false);
                prompt_activated = true;

            }
            else if (prompt_activated)
            {
                promptIF.enabled = false;
                PageSwitch_script.can_switch = true;
                light_bulb.enabled = false;
                device_lights.SetActive(true);
                prompt_activated = false;
            }
        }
    }
}
