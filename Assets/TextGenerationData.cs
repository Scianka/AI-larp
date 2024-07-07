using System;

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

    private string CatWeatherDataParser(int _variableNumber)
    {
        if (choices[0].finish_reason != "")
        {
            string _generatedText = choices[0].message.content; // Bytom*valid*real*true*other
            string[] _variablesAsStrings = _generatedText.Split('*');
            return _variablesAsStrings[_variableNumber];
        }
        else return "";
    }

    public string GetLocationName() => CatWeatherDataParser(0);
    public bool CheckIfLocationIsValid()
    {
        string _rawData = CatWeatherDataParser(1);
        if (_rawData == "valid") return true;
        else return false;
    }
    public bool CheckIfLocationIsReal()
    {
        string _rawData = CatWeatherDataParser(2);
        if (_rawData == "real") return true;
        else return false;
    }
    public bool CheckIfAICanAccessCurrentWeather()
    {
        string _rawData = CatWeatherDataParser(3);
        if (_rawData == "true") return true;
        else return false;
    }
    public string GetCurrentThemeAsString() => CatWeatherDataParser(4);
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