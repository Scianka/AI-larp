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
    public AudioSource _backgroundAudio;
    public Image _soundButtonImage;
    public Animator _transitionBlockAnim;
    private bool _themeTransitionCanOccur = true;
    public TMP_InputField _playerText;
    private string _locationName;
    private bool _isLocationValid;
    private bool _isLocationReal;
    private bool _canAIAccessCurrentWeather;
    private TextGenerationData _generatedText;

    private enum WeatherTheme
    {
        none,
        other,
        rain,
    }

    private WeatherTheme? _currentTheme;

    private void Start()
    {
        _catIiconDefaultSource = _catIcon.sprite;
        _bacgroundDefaultColor = _background.color;
        _currentTheme = WeatherTheme.none;
        _rainAudio.Stop();
        HaTeTP_OpenAI_API.GetAPIKey();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && _themeTransitionCanOccur && !HaTeTP_OpenAI_API._isAPICallProcessing)
        {
            string _enteredLocation = _playerText.text;
            _generatedText = HaTeTP_OpenAI_API.GenerateText(_enteredLocation);
            StartCoroutine(WaitForAPICallToEnd());
        }
    }

    private IEnumerator WaitForAPICallToEnd()
    {
        while (HaTeTP_OpenAI_API._isAPICallProcessing) yield return null;
        if (!HaTeTP_OpenAI_API._didErrorOccur)
        {
            ManageVariables();
            MakeThemeTransition();
        }
    }

    private void ManageVariables()
    {
        _locationName = _generatedText.GetLocationName();
        _isLocationValid = _generatedText.CheckIfLocationIsValid();
        _isLocationReal = _generatedText.CheckIfLocationIsReal();
        _canAIAccessCurrentWeather = _generatedText.CheckIfAICanAccessCurrentWeather();
        string _currentThemeAsString = _generatedText.GetCurrentThemeAsString();
        if (_currentThemeAsString == "none") _currentTheme = WeatherTheme.none;
        else if (_currentThemeAsString == "other") _currentTheme = WeatherTheme.other;
        else if (_currentThemeAsString == "rain") _currentTheme = WeatherTheme.rain;
        else _currentTheme = WeatherTheme.none;
        AIDebugLogs();
    }

    private void AIDebugLogs()
    {
        if (_isLocationValid && !_isLocationReal) Debug.Log("AI: The entered location is valid. Its name is " + _locationName + ". This place is fictional. The weather will be guessed.");
        else if (_isLocationValid && _isLocationReal && _canAIAccessCurrentWeather) Debug.Log("AI: The entered location is valid. Its name is " + _locationName + ". This place is real. Relevant data on current weather can be accessed.");
        else if (_isLocationValid && _isLocationReal && !_canAIAccessCurrentWeather) Debug.Log("AI: The entered location is valid. Its name is " + _locationName + ". This place is real. Relevant data on current weather can't be accessed.");
        else Debug.Log("AI: The entered location is invalid. Operations are cancelled.");
        Debug.Log("Current weather theme is " + _currentTheme + ".");
    }

    private void MakeThemeTransition()
    {
        _transitionBlockAnim.Play("BlockTrans", -1, 0f);
        StartCoroutine(ThemeTransitionEnabler());
        if (_currentTheme == WeatherTheme.none) StartCoroutine(NoneTheme());
        else if (_currentTheme == WeatherTheme.other) StartCoroutine(OtherTheme());
        else if (_currentTheme == WeatherTheme.rain) StartCoroutine(RainTheme());
    }

    private IEnumerator ThemeTransitionEnabler()
    {
        _themeTransitionCanOccur = false;
        yield return new WaitForSeconds(2.67f);
        _themeTransitionCanOccur = true;
    }

    private IEnumerator NoneTheme()
    {
        yield return new WaitForSeconds(1.17f);
        _catIcon.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        _catIcon.rectTransform.localRotation = new Quaternion(0, 0, 0, 0);
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

    private IEnumerator OtherTheme()
    {
        yield return new WaitForSeconds(1.17f);
        _catIcon.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        _catIcon.rectTransform.localRotation = new Quaternion(0, 0, 0, 0);
        _catIcon.sprite = _catIconSources[0];
        _catIconAnim.Play("CatCool");
        _background.color = _backgroundColors[0];
        _rainPS.gameObject.SetActive(false);
        _cloud.SetActive(true);
        _cloudB.SetActive(false);
        _sunRays.SetActive(true);
        _rainAudio.Stop();
        _infoText.text = "Current weather in " + _locationName + " is not rainy.";
    }

    private IEnumerator RainTheme()
    {
        yield return new WaitForSeconds(1.17f);
        _catIcon.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        _catIcon.rectTransform.localRotation = new Quaternion(0, 0, 0, 0);
        _catIcon.sprite = _catIconSources[1];
        _catIconAnim.Play("CatCry");
        _background.color = _backgroundColors[1];
        _rainPS.gameObject.SetActive(true);
        _cloud.SetActive(false);
        _cloudB.SetActive(true);
        _sunRays.SetActive(false);
        _rainAudio.Play();
        _infoText.text = "Current weather in " + _locationName + " is rainy...";
    }

    public void ToggleBackgroundSound()
    {
        if (_backgroundAudio.volume == 0)
        {
            _backgroundAudio.volume = 0.7f;
            _soundButtonImage.color = new Color32(255, 255, 255, 245);
        }
        else
        {
            _backgroundAudio.volume = 0;
            _soundButtonImage.color = new Color32(255, 255, 255, 150);
        }
    }
}