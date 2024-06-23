using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Diagnostics.Tracing;

public class ThemeManager : MonoBehaviour
{
    public TMP_Text _infoText;
    public RawImage _background;
    public Color32[] _backgroundColors;
    private Color32 _bacgroundDefaultColor;
    public Image _catIcon;
    public Sprite[] _catIconSources;
    private Sprite _catIiconDefaultSource;
    public Animator _catIconAnim;
    public ParticleSystem _rainPS;
    public GameObject _cloud;

    private enum WeatherTheme
    {
        none,
        other,
        rain,
    }

    private WeatherTheme current_theme;

    private void Start()
    {
        _catIiconDefaultSource = _catIcon.sprite;
        _bacgroundDefaultColor = _background.color;
        current_theme = WeatherTheme.none;

        //curl https://platform.openai.com;
    }

    private void Update()
    {
        bool rainy_theme_on = true;
        if (current_theme != WeatherTheme.rain) rainy_theme_on = false;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            current_theme = rainy_theme_on ? WeatherTheme.other : WeatherTheme.rain;
            _catIcon.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        }

        if (current_theme == WeatherTheme.none)
        {
            _catIcon.sprite = _catIiconDefaultSource;
            _catIconAnim.Play("CatGuitar");
            _background.color = _bacgroundDefaultColor;
            _rainPS.gameObject.SetActive(false);
            _cloud.SetActive(false);
            _infoText.text = "No information on current weather.";
        }
        else if (current_theme == WeatherTheme.other)
        {
            _catIcon.sprite = _catIconSources[0];
            _catIconAnim.Play("CatCool");
            _background.color = _backgroundColors[0];
            _rainPS.gameObject.SetActive(false);
            _cloud.SetActive(true);
            _infoText.text = "Current weather is not rainy";
        }
        else if (current_theme == WeatherTheme.rain)
        {
            _catIcon.sprite = _catIconSources[1];
            _catIconAnim.Play("CatCry");
            _background.color = _backgroundColors[1];
            _rainPS.gameObject.SetActive(true);
            _cloud.SetActive(false);
            _infoText.text = "Current weather is rainy";
        }
    }
}
