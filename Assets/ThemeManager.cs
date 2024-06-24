using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject _cloudB;
    public GameObject _sunRays;
    public AudioSource _rainAudio;
    public Animator _transitionBlockAnim;
    private bool _transitionCanOccur = true;
    private Coroutine _eatherCoroutine;

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
        _rainAudio.Stop();

        // calling APIs:
        CallOpenAI_API();
        //JokingTime();
    }

    private void Update()
    {
        bool rainy_theme_on = true;
        if (current_theme != WeatherTheme.rain) rainy_theme_on = false;
        if (Input.GetKeyDown(KeyCode.Return) && _transitionCanOccur)
        {
            StartCoroutine(TransitionEnabler());
            _transitionBlockAnim.Play("BlockTrans", -1, 0f);
            current_theme = rainy_theme_on ? WeatherTheme.other : WeatherTheme.rain;
            if (current_theme == WeatherTheme.none) StartCoroutine(NoneWeather());
            else if (current_theme == WeatherTheme.other) StartCoroutine(OtherWeather());
            else if (current_theme == WeatherTheme.rain) StartCoroutine(RainWeather());
        }
    }

    private IEnumerator NoneWeather()
    {
        yield return new WaitForSeconds(1.17f);
        _catIcon.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        _catIcon.sprite = _catIiconDefaultSource;
        _catIconAnim.Play("CatGuitar");
        _background.color = _bacgroundDefaultColor;
        _rainPS.gameObject.SetActive(false);
        _cloud.SetActive(false);
        _cloudB.SetActive(false);
        _sunRays.SetActive(false);
        _rainAudio.Stop();
        _infoText.text = "No information on current weather.";
    }

    private IEnumerator OtherWeather()
    {
        yield return new WaitForSeconds(1.17f);
        _catIcon.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        _catIcon.sprite = _catIconSources[0];
        _catIconAnim.Play("CatCool");
        _background.color = _backgroundColors[0];
        _rainPS.gameObject.SetActive(false);
        _cloud.SetActive(true);
        _cloudB.SetActive(false);
        _sunRays.SetActive(true);
        _rainAudio.Stop();
        _infoText.text = "Current weather is not rainy.";
    }

    private IEnumerator RainWeather()
    {
        yield return new WaitForSeconds(1.17f);
        _catIcon.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        _catIcon.sprite = _catIconSources[1];
        _catIconAnim.Play("CatCry");
        _background.color = _backgroundColors[1];
        _rainPS.gameObject.SetActive(true);
        _cloud.SetActive(false);
        _cloudB.SetActive(true);
        _sunRays.SetActive(false);
        _rainAudio.Play();
        _infoText.text = "Current weather is rainy...";
    }

    private IEnumerator TransitionEnabler()
    {
        _transitionCanOccur = false;
        yield return new WaitForSeconds(2.67f);
        _transitionCanOccur = true;
    }

    private void CallOpenAI_API() => HaTeTPLowQualityAPI.InitializeNewAPICall();

    // function made for first API testing and learning
    private void JokingTime() => Debug.Log(ChuckNorrisJokeAPI.GetNewJoke().value);

    private void DebugLogs()
    {
        Debug.Log(current_theme);
    }
}