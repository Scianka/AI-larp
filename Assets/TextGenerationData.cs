using System;
using UnityEditor.Experimental.GraphView;

[Serializable]
public class TextGenerationData
{
    public string id;
    public string @object;
    public long created;
    public string model;
    public Choices[] choices;
    public Usage usage;
    public string system_fingerprint;

    public string CatWeatherDataParser(int _variableNumber)
    {
        string _generatedText = choices[0].message.content; // Bytom*valid*real*true*other
        string[] _variablesAsStrings = _generatedText.Split('*'); // 0 _locationName 1 _isLocationValid 2 _isLocationReal 3 _canAIAccessWeatherInfo 4 _currentTheme
        return _variablesAsStrings[_variableNumber];
    }

    public string GetLocationName() => CatWeatherDataParser(0);
    public bool? CheckIfLocationIsValid()
    {
        string _rawData = CatWeatherDataParser(1);
        if (_rawData == "invalid") return false;
        else if (_rawData == "valid") return true;
        else return null;
    }
    public bool? CheckIfLocationIsReal()
    {
        string _rawData = CatWeatherDataParser(2);
        if (_rawData == "fictional") return false;
        else if (_rawData == "real") return true;
        else return null;
    }
    public bool? CheckIfAICanAccessWeather()
    {
        string _rawData = CatWeatherDataParser(3);
        if (_rawData == "false") return false;
        else if (_rawData == "true") return true;
        else if (_rawData == "invalid") return false;
        else return null;
    }
    public string GetCurrentThemeAsString() => CatWeatherDataParser(4); // this data needs to be parsed within the ThemeManager script
}

    [Serializable]
    public class Choices
    {
        public int index;
        public Message message;
        public object logprobs;
        public string finish_reason;
    }

        [Serializable]
        public class Message
        {
            public string role;
            public string content;
        }

    [Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }