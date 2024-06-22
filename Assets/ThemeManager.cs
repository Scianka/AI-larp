using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class ThemeManager : MonoBehaviour
{
    public TMP_Text infoText;
    public RawImage background;
    public Color32[] backgroundColors;
    private Color32 bacgroundDefaultColor;
    public Image catIcon;
    public Sprite[] catIconSources;
    private Sprite catIiconDefaultSource;

    private enum WeatherTheme
    {
        none,
        other,
        rain,
    }

    private WeatherTheme current_theme;

    private void Start()
    {
        catIiconDefaultSource = catIcon.sprite;
        bacgroundDefaultColor = background.color;
        catIcon.sprite = catIconSources[0];
        current_theme = WeatherTheme.none;

        //curl https://platform.openai.com;
    }

    private void Update()
    {
        bool rainy_theme_on = true;
        if (current_theme != WeatherTheme.rain) { rainy_theme_on = false; }

        if (Input.GetKeyDown(KeyCode.Return)) { current_theme = rainy_theme_on ? WeatherTheme.other : WeatherTheme.rain; }

        if (current_theme == WeatherTheme.none)
        {
            catIcon.sprite = catIiconDefaultSource;
            background.color = bacgroundDefaultColor;
            infoText.text = "No information on current weather.";
        }
        else if (current_theme == WeatherTheme.other)
        {
            catIcon.sprite = catIconSources[0];
            background.color = backgroundColors[0];
            infoText.text = "Current weather: Not rainy";
        }
        else if (current_theme == WeatherTheme.rain)
        {
            catIcon.sprite = catIconSources[1];
            background.color = backgroundColors[1];
            infoText.text = "Current weather: Rainy";
        }
    }
}
