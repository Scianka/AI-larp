using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;
using UnityEngine.UI;

public class PageSwitch : MonoBehaviour
{
    public TextMeshProUGUI what_page;
    private TextMeshProUGUI content_text;

    public Sprite button_default;
    public Sprite button_light;
    public Image LB_img;
    public Image RB_img;

    [HideInInspector]
    public bool can_switch = true;

    private void Start()
    {
        content_text = GetComponent<TextMeshProUGUI>();
        NewText();
    }

    private void Update()
    {
        Debug.Log(can_switch);
        if (can_switch)
        {
            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.PageDown)) NextPage();
            if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.PageUp)) PreviousPage();
        }
        WhatPage();
    }

    public void NextPage()
    {
        if (content_text.pageToDisplay < content_text.textInfo.pageCount) content_text.pageToDisplay++;
        StartCoroutine("RBLight");
    }

    public void PreviousPage()
    {
        if (content_text.pageToDisplay > 1) content_text.pageToDisplay--;
        StartCoroutine("LBLight");
    }


    public void NewText() => content_text.pageToDisplay = 1;

    private void WhatPage() => what_page.text = content_text.pageToDisplay.ToString() + " / " + content_text.textInfo.pageCount.ToString();

    private IEnumerator LBLight()
    {
        LB_img.sprite = button_light;
        yield return new WaitForSeconds(0.2f);
        LB_img.sprite = button_default;
    }

    private IEnumerator RBLight()
    {
        RB_img.sprite = button_light;
        yield return new WaitForSeconds(0.2f);
        RB_img.sprite = button_default;
    }
}
