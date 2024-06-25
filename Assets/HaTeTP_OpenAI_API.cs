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

    public static void GenerateText(string _text)
    {
        // API request content
        string _jsonData =
        $@"{{
            ""model"": ""gpt-4o"",
            ""messages"":
            [
                {{
                    ""role"": ""system"",
                    ""content"": ""Be the usual AI assistant.""
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
            Debug.Log("Response content: " + _responseString);
            _response.Close();
        }
        // API errors
        catch (WebException _error)
        {
            HttpWebResponse _errorResponse = (HttpWebResponse)_error.Response;
            StreamReader _errorResponseReader = new StreamReader(_errorResponse.GetResponseStream());
            string _errorText = _errorResponseReader.ReadToEnd();
            Debug.Log("Error details: " + _errorResponse.StatusCode + " " + _errorText);
        }
    }
}