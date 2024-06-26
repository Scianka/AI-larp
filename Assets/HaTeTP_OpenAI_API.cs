using System;
using System.Text;
using System.Net;
using System.IO;
using UnityEngine;

// working with HTTP requests
public static class HaTeTP_OpenAI_API
{
    private static string _secretKey;

    public static void GetAPIKey() => _secretKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);

    public static TextGenerationData GenerateText(string? _text)
    {
        // 0 - _locationName, 1 - _isLocationValid, 2 - _isLocationReal, 3 - _canAIAccessWeatherInfo, 4 - _currentTheme
        string _AISystemSettings = "You are a function in a weather app. You don't interact with the user directly." +
            "Based on given input, you always respond with only 5 key-words and separate them with sign '*'. Never include this sign in your key-words." +
            "The user will always send you a location's name. It can be fictional or real." +
            "If you have any information on it, set first key-word to the interpreted name (keep it short) and the second key-word to 'valid'. " +
            "If you can't find anything, set all the key-words to 'invalid' and don't execute next steps." +
            "Next, determine whether the location is real or fictional. If it is real, set third key-word to 'real'. If it is fictional, set third key-word to 'fictional'. " +
            "Then, if the location is real and you can check its current weather - check the weather and decide whether it is rather rainy or any other kind of weather. " +
            "Based on that decision, set fifth key-word to 'rain' or 'other'. Also set fourth key-word to 'true'. " +
            "If the location is real but you can't check it's current weather - set fourth key-word to 'false' and set fifth key-word to 'none'. " +
            "If the location is fictional, try to guess what kind of weather it could possibly have right now. " +
            "Based on the conclusion, set fifth key-word to 'rain' or 'other' and set fourth key-word to 'invalid'." +
            "For example, your final response could look like this: 'village just outside the dungeon*valid*fictional*invalid*other'.";

        // API request content
        string _jsonData =
        $@"{{
            ""model"": ""gpt-4o"",
            ""messages"":
            [
                {{
                    ""role"": ""system"",
                    ""content"": ""{_AISystemSettings}""
                }},
                {{
                    ""role"": ""user"",
                    ""content"": ""{_text}""
                }}
            ]
        }}";

        // API request
        HttpWebRequest _request = (HttpWebRequest)WebRequest.Create("https://api.openai.com/v1/chat/completions");

        _request.Method = "POST";
        _request.ContentType = "application/json";
        _request.Headers.Add("Authorization", "Bearer " + _secretKey);

        byte[] _jsonDataBytes = Encoding.UTF8.GetBytes(_jsonData);
        _request.ContentLength = _jsonDataBytes.Length;
        _request.GetRequestStream().Write(_jsonDataBytes, 0, _jsonDataBytes.Length);

        // API response
        try
        {
            HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
            StreamReader _responseReader = new StreamReader(_response.GetResponseStream());
            string _responseString = _responseReader.ReadToEnd(); // json data format
            _response.Close(); // not a best place?
            return JsonUtility.FromJson<TextGenerationData>(_responseString);
        }
        // API errors
        catch (WebException _error)
        {
            HttpWebResponse _errorResponse = (HttpWebResponse)_error.Response;
            StreamReader _errorResponseReader = new StreamReader(_errorResponse.GetResponseStream());
            string _errorText = _errorResponseReader.ReadToEnd();
            Debug.Log("Error details: " + _errorResponse.StatusCode + " " + _errorText);
            return JsonUtility.FromJson<TextGenerationData>(null); // this needs a better solution
        }
    }
}