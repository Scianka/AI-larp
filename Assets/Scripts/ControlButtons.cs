using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlButtons : MonoBehaviour
{
    [Header("Prompt")]
    public PanelSwitch PanelSwitch_script;
    public PageSwitch PageSwitch_script;
    public TMP_InputField promptIF;
    public TMP_Text contentTF;

    public string contentAI { set; get; }

    private string prompt_holder;
    private TextGenerationData _generatedText;
    private Coroutine SendPromptCoroutine;
    private bool buttons_blocked = false;
    private bool is_spell;

    [Header("Looks")]
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
            if (Input.GetKeyDown(KeyCode.F3)) DungeonSpell();
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
        if (is_spell) _cleanPrompt = "This is just a test prompt. Say: 'DLC Spell' and don't change it in any way.";
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
            contentTF.text = contentAI;
        }
        else contentTF.text = "...";
        SimulationHistory.AddPromptToHistory(prompt_holder);
        SimulationHistory.AddResponseToHistory(contentTF.text);
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
        yield return new WaitForSeconds(text_transition_time);
        buttons_blocked = false;
    }
}