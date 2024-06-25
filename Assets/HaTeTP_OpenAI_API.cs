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
        using (Stream _requestStream = _request.GetRequestStream()) { _requestStream.Write(_jsonDataBytes, 0, _jsonDataBytes.Length); }

        // API response
        try
        {
            WebResponse _response = (HttpWebResponse)_request.GetResponse();
            using (Stream _responseStream = _response.GetResponseStream())
            {
                StreamReader _responseStreamReader = new StreamReader(_responseStream);
                string _responseString = _responseStreamReader.ReadToEnd(); // json format
                Debug.Log("Response content: " + _responseString);
                /*
                    Response content: {
                      "id": "chatcmpl-9e2DHCXWskuzZiSYFAdYT7QAT61TD",
                      "object": "chat.completion",
                      "created": 1719328655,
                      "model": "gpt-4o-2024-05-13",
                      "choices": [
                        {
                          "index": 0,
                          "message": {
                            "role": "assistant",
                            "content": "Hello! How can I assist you today?"
                          },
                          "logprobs": null,
                          "finish_reason": "stop"
                        }
                      ],
                      "usage": {
                        "prompt_tokens": 20,
                        "completion_tokens": 9,
                        "total_tokens": 29
                      },
                      "system_fingerprint": "fp_3e7d703517"
                    }
                */
            }
            _response.Close();
        }
        // API errors
        catch (WebException _error)
        {
            using (WebResponse _errorResponse = _error.Response)
            {
                HttpWebResponse _errorResponseHTTP = (HttpWebResponse)_errorResponse;
                using (Stream _errorResponseStream = _errorResponse.GetResponseStream())
                using (var _errorResponseStreamReader = new StreamReader(_errorResponseStream))
                {
                    string _errorText = _errorResponseStreamReader.ReadToEnd();
                    Debug.Log("Error details: " + _errorResponseHTTP.StatusCode + " " + _errorText);
                }
            }
        }
    }
}