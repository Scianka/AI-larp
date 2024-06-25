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
                    Response text: {
                    "id": "chatcmpl-9dgAXUK9WTawwRpcChsrRSYOp94m6",
                    "object": "chat.completion",
                    "created": 1719243917,
                    "model": "gpt-3.5-turbo-0125",
                    "choices": [
                    {
                    "index": 0,
                    "message": {
                    "role": "assistant",
                    "content": "In the realm of code, a concept profound,\nRecursion reigns, whimsically bound.\nA function calling itself, without a frown,\nA dance of elegance, up and down.\n\nLike a mirror reflecting its own reflection,\nRecursion dives into a deep connection.\nBreaking problems into smaller parts,\nIt unwraps mysteries, revealing hearts.\n\nEach recursive call a journey anew,\nSolving puzzles with a mystical view.\nStack frames stacking, like a tower tall,\nUntil the base case answers the call.\n\nA cycle of repetition, in patterns delight,\nRecursive dreams take flight.\nInfinite loops or beauty divine,\nRecursion dances in a sublime design.\n\nSo embrace the recursion, with courage and zest,\nIn the world of programming, it truly is the best.\nA poetic loop, a magical song,\nIn the vast universe of code, where we all belong."
                    },
                    "logprobs": null,
                    "finish_reason": "stop"
                    }
                    ],
                    "usage": {
                    "prompt_tokens": 39,
                    "completion_tokens": 172,
                    "total_tokens": 211
                    },
                    "system_fingerprint": null
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