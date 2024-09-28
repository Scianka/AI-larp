using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ControlButtons : MonoBehaviour
{
    [Header("Prompt related")]
    public PanelSwitch PanelSwitch_script;
    public PageSwitch PageSwitch_script;
    public TMP_InputField promptIF;
    public TMP_Text contentTF;

    private string contentAI;
    private string prompt_holder;
    private string feedback;
    private string description;
    private string last_state;
    private string last_response;
    private string last_prompt;
    private string session_summary;
    private TextGenerationData _generatedText;
    private Coroutine SendPromptCoroutine;
    private bool buttons_blocked = false;
    private bool invalid_format = false;
    private bool is_spell;

    [Header("Looks related")]
    public float text_transition_time;
    public Image wish_button;
    public Sprite WB_default;
    public Sprite WB_light;
    public Image spell_button;
    public Sprite SB_default;
    public Sprite SB_light;

    private Coroutine FadeImageCoroutine;
    private Coroutine FadeTextCoroutine;
    private Color32 zero_visibility = new Color32(0, 0, 0, 0);
    private Color32 full_visibility = new Color32(0, 0, 0, 255);
    private Color32 text_full_visibility;
    private string text_fade_mode;
    private string img_fade_mode;
    private Image img_to_fade;

    private void Start()
    {
        OpenAI_API_HTTP.GetAPIKey();
        text_full_visibility = contentTF.color;
    }

    private void Update()
    {
        if (!OpenAI_API_HTTP._isAPICallProcessing && !buttons_blocked)
        {
            if (Input.GetKeyDown(KeyCode.F2)) DungeonWish();
            //if (Input.GetKeyDown(KeyCode.F3)) DungeonSpell();
        }
    }

    private void DungeonWish()
    {
        is_spell = false;
        StartCoroutine("WBLight");
        StartCoroutine("ButtonsBlocked");
        if (SendPromptCoroutine != null) StopCoroutine(SendPromptCoroutine);
        SendPromptCoroutine = StartCoroutine("SendPrompt");
    }

    private void DungeonSpell()
    {
        is_spell = true;
        StartCoroutine("SBLight");
        StartCoroutine("ButtonsBlocked");
        if (SendPromptCoroutine != null) StopCoroutine(SendPromptCoroutine);
        SendPromptCoroutine = StartCoroutine("SendPrompt");
    }

    private IEnumerator SendPrompt()
    {
        string _cleanPrompt = promptIF.text.Replace("\t","").Replace("\r","").Replace("\n","").Replace("\"","'");
        if (is_spell) _cleanPrompt = "...";
        prompt_holder = _cleanPrompt;
        text_fade_mode = "out";
        FadeTextCheck();
        yield return new WaitForSeconds(text_transition_time + 0.1f);
        _generatedText = OpenAI_API_HTTP.GenerateText(_cleanPrompt);
        StartCoroutine(WaitForAPICallToEnd());
    }

    private IEnumerator WaitForAPICallToEnd()
    {
        while (OpenAI_API_HTTP._isAPICallProcessing) yield return null;

        if (!OpenAI_API_HTTP._didErrorOccur)
        {
            contentAI = _generatedText.GetContent();
            Debug.Log("Full response:\n" + contentAI);
            //contentTF.text = contentAI;
            ResponseParserForPlayers();
            if (!invalid_format)
            {
                ResponseParserForHistory();
                if (!invalid_format)
                {
                    Debug.Log("Simulation history: \nLast state:\n" + last_state + "\n\n" + "Last response:\n" + last_response + "\n\n" + "Last prompt:\n" + last_prompt + "\n\n" + "Session summary:\n" + session_summary);
                    SaveDataToSimulationHistory();
                    OpenAI_API_HTTP.SaveUpdateTime();
                    OpenAI_API_HTTP.is_first_update = false;
                    contentTF.text = "\"" + feedback + "\"\n\n" + description;
                }
                else
                {
                    Debug.Log("Internal error : invalid response format (second parser)");
                    contentTF.text = "...";
                }
            }
            else
            {
                Debug.Log("Internal error : invalid response format (first parser)");
                contentTF.text = "...";
            }
            invalid_format = false;
        }
        else contentTF.text = "...";
        PageSwitch_script.NewText();
        text_fade_mode = "in";
        FadeTextCheck();
    }

    private IEnumerator WBLight()
    {
        wish_button.sprite = WB_light;
        yield return new WaitForSeconds(0.2f);
        wish_button.sprite = WB_default;
    }

    private IEnumerator SBLight()
    {
        spell_button.sprite = SB_light;
        yield return new WaitForSeconds(0.2f);
        spell_button.sprite = SB_default;
    }

    public void FadeTextCheck()
    {
        if (FadeTextCoroutine != null) StopCoroutine(FadeTextCoroutine);
        FadeTextCoroutine = StartCoroutine("FadeText");
    }

    public IEnumerator FadeText()
    {
        float elapsed_time = 0f;

        if (text_fade_mode == "out")
        {
            contentTF.color = text_full_visibility;
            yield return new WaitForSeconds(text_transition_time);
            while (elapsed_time < 1f)
            {
                elapsed_time += Time.deltaTime;
                contentTF.color = Color.Lerp(text_full_visibility, zero_visibility, elapsed_time / 1f);
                yield return null;
            }
            contentTF.color = zero_visibility;
        }

        else if (text_fade_mode == "in")
        {
            contentTF.color = zero_visibility;
            yield return new WaitForSeconds(text_transition_time);
            while (elapsed_time < 1f)
            {
                elapsed_time += Time.deltaTime;
                contentTF.color = Color.Lerp(zero_visibility, text_full_visibility, elapsed_time / 1f);
                yield return null;
            }
            contentTF.color = text_full_visibility;
        }
    }

    /*public void FadeImageCheck()
    {
        if (FadeImageCoroutine != null) StopCoroutine(FadeImageCoroutine);
        FadeImageCoroutine = StartCoroutine("FadeImage");
    }

    public IEnumerator FadeImage()
    {
        float elapsed_time = 0f;

        if (img_fade_mode == "out")
        {
            img_to_fade.color = full_visibility;
            yield return new WaitForSeconds(0.5f);
            while (elapsed_time < 1f)
            {
                elapsed_time += Time.deltaTime;
                img_to_fade.color = Color.Lerp(full_visibility, zero_visibility, elapsed_time / 1f);
                yield return null;
            }
            img_to_fade.color = zero_visibility;
        }
        else if (img_fade_mode == "in")
        {
            img_to_fade.color = zero_visibility;
            yield return new WaitForSeconds(0.5f);
            while (elapsed_time < 1f)
            {
                elapsed_time += Time.deltaTime;
                img_to_fade.color = Color.Lerp(zero_visibility, full_visibility, elapsed_time / 1f);
                yield return null;
            }
            img_to_fade.color = full_visibility;
        }
    }*/

    private IEnumerator ButtonsBlocked()
    {
        buttons_blocked = true;
        yield return new WaitForSeconds(text_transition_time + 0.1f);
        while (OpenAI_API_HTTP._isAPICallProcessing) yield return null;
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(text_transition_time + 0.2f);
        buttons_blocked = false;
    }

    private void ResponseParserForPlayers()
    {
        string response = contentAI;

        int ufb_from = response.IndexOf("Prompt feedback:") + "Prompt feedback:".Length;
        int ufb_to = response.LastIndexOf("New dungeon’s state description:");
        string unclean_feedback = "";
        if (ResponseValidFormatCheck(ufb_from, ufb_to)) unclean_feedback = response.Substring(ufb_from, ufb_to - ufb_from);
        else { invalid_format = true; return; }
        int cfb_from = unclean_feedback.IndexOf("[");
        int cfb_to = unclean_feedback.LastIndexOf("]");
        string clean_feedback = "";
        if (ResponseValidFormatCheck(cfb_from, cfb_to)) clean_feedback = unclean_feedback.Substring(cfb_from + 1, cfb_to - cfb_from - 1);
        else { invalid_format = true; return; }
        feedback = clean_feedback;

        response = contentAI;

        int udesc_from = response.IndexOf("New dungeon’s state description:") + "New dungeon’s state description:".Length;
        int udesc_to = response.LastIndexOf("Session summary:");
        string unclean_description = "";
        if (ResponseValidFormatCheck(udesc_from, udesc_to)) unclean_description = response.Substring(udesc_from, udesc_to - udesc_from);
        else { invalid_format = true; return; }
        int cdesc_from = unclean_description.IndexOf("[");
        int cdesc_to = unclean_description.LastIndexOf("]");
        string clean_description = "";
        if (ResponseValidFormatCheck(cdesc_from, cdesc_to)) clean_description = unclean_description.Substring(cdesc_from + 1, cdesc_to - cdesc_from - 1);
        else { invalid_format = true; return; }
        description = clean_description;
    }
    
    private void ResponseParserForHistory()
    {
        last_prompt = prompt_holder;
        last_response = "'" + feedback.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("\"", "'") + "' " + description.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("\"", "'");

        string response = contentAI;
        string reformated_response = response.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("\"", "'");

        int uls_from = reformated_response.IndexOf("New dungeon’s state:") + "New dungeon’s state:".Length;
        int uls_to = reformated_response.LastIndexOf("Prompt feedback:");
        string unclean_last_state = "";
        if (ResponseValidFormatCheck(uls_from, uls_to)) unclean_last_state = reformated_response.Substring(uls_from, uls_to - uls_from);
        else { invalid_format = true; return; }
        int cls_from = unclean_last_state.IndexOf("[");
        int cls_to = unclean_last_state.LastIndexOf("]");
        string clean_last_state = "";
        if (ResponseValidFormatCheck(cls_from, cls_to)) clean_last_state = unclean_last_state.Substring(cls_from + 1, cls_to - cls_from - 1);
        else { invalid_format = true; return; }
        last_state = clean_last_state;

        response = contentAI;
        reformated_response = response.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace("\"", "'");

        int uss_from = reformated_response.IndexOf("Session summary:") + "Session summary:".Length;
        string unclean_session_summary = "";
        if (uss_from >= 0) unclean_session_summary = reformated_response.Substring(uss_from);
        else { invalid_format = true; return; }
        int css_from = unclean_session_summary.IndexOf("[");
        int css_to = unclean_session_summary.LastIndexOf("]");
        string clean_session_summary = "";
        if (ResponseValidFormatCheck(css_from, css_to)) clean_session_summary = unclean_session_summary.Substring(css_from + 1, css_to - css_from - 1);
        else { invalid_format = true; return; }
        session_summary = clean_session_summary;
    }

    private bool ResponseValidFormatCheck(int start_index, int end_index)
    {
        if (start_index < 0 || end_index < 0 || start_index >= end_index) return false;
        else return true;
    }

    private void SaveDataToSimulationHistory()
    {
        SimulationHistory.last_state = last_state;
        SimulationHistory.last_response = last_response;
        SimulationHistory.last_prompt = last_prompt;
        SimulationHistory.session_summary = session_summary;
    }
}