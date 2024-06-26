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
    private bool _themeTransitionCanOccur = true;
    public TMP_InputField _playerText;

    private string _locationName = "(...)";
    private bool _isAPICallProcessing = false;
    private bool _isLocationValid;
    private bool _isLocationReal;
    private bool _canAIAccessWeatherInfo;
    // private WeatherTheme _currentTheme

    private enum WeatherTheme
    {
        none,
        other,
        rain,
    }

    private WeatherTheme _currentTheme;

    private void Start()
    {
        _catIiconDefaultSource = _catIcon.sprite;
        _bacgroundDefaultColor = _background.color;
        _currentTheme = WeatherTheme.none;
        _rainAudio.Stop();
        Debug.Log("Hi! In this weather app you can check current weather in both real and fictional places. Although, it will only tell you whether the weather is rainy or not.");

        // accessing API
        HaTeTP_OpenAI_API.GetAPIKey();

        // calling APIs:
        //Debug.Log(HaTeTP_OpenAI_API.GenerateText("Say in your own words that Chuck Norris jokes aren't funny.").choices[0].message.content);
        //Debug.Log(ChuckNorrisJokeAPI.GetNewJoke().value);
    }

    private void Update()
    {
        // DON'T LET THE FUNCTION MAKE API CALLS WHEN THERE IS ONE ALREADY UNDERGOING PROCESSING
        if (Input.GetKeyDown(KeyCode.Return) && _themeTransitionCanOccur && !_isAPICallProcessing)
        {
            string _enteredLocation = _playerText.text;
            Debug.Log("Current weather is " + HaTeTP_OpenAI_API.GenerateText(_enteredLocation).GetCurrentThemeAsString() + "!");

            // LET AI EXTRACT RELATED VARIABLES' VALUES FROM IT

            // AND IF THERE IS AN API CALL ERROR - DISPLAY AN APPROPRIATE DEBUG LOG AND STOP EXECUTING THE REST OF THIS CODE (CHECK FOR ERRORS BEFORE MANAGING VARIABLES)

            // IF THE LOCATION IS VALID BUT IT IS NOT REAL - LET AI DECIDE BETWEEN rain OR other WeatherTheme (BASED ON THE OVERALL LOCATION'S CHARACTERISTICS)
            // IF THE LOCATION IS VALID AND REAL AND AI HAS INFO ON IT'S WEATHER - CHECK THE ACTUAL WEATHER AND BASED THAT LET AI DECIDE BETWEEN rain OR other WeatherTheme
            // IF THE LOCATION IS VAILD AND REAL BUT AI HAS NO INFO ON IT'S WEATHER -  RESULT IS none WeatherTheme
            // IF THE LOCATION IS NOT VALID - RESULT IS none WeatherTheme

            // ADD DEBUG LOGS FOR ALL THE ABOVE CASES

            // WAIT FOR THE API CALL TO END TO MAKE THEME TRANSITION

            MakeThemeTransition();
        }
    }

    private IEnumerator NoneTheme()
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

    private IEnumerator OtherTheme()
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
        _infoText.text = "Current weather in " + _locationName + " is not rainy.";
    }

    private IEnumerator RainTheme()
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
        _infoText.text = "Current weather in " + _locationName + " is rainy...";
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
}