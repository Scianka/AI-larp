using System;
using System.Text;
using System.Net;
using System.IO;
using UnityEngine;

// working with HTTP requests
public static class HaTeTPLowQualityAPI
{
    private static string _secretKey;

    public static void InitializeNewAPICall()
    {
        _secretKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);
        NewAPICall();
    }

    private static void NewAPICall()
    {
        string apiKey = _secretKey;
        string url = "https://api.openai.com/v1/chat/completions";
        string jsonData =
        @"{
            ""model"": ""gpt-4o"",
            ""messages"": [
                {
                    ""role"": ""system"",
                    ""content"": ""You are a funny bot.""
                },
                {
                    ""role"": ""user"",
                    ""content"": ""Say 'Test completed.'. And add a short, random joke.""
                }
            ]
        }";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Bearer " + apiKey);

        byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
        request.ContentLength = byteArray.Length;

        using (Stream dataStream = request.GetRequestStream())
        {
            dataStream.Write(byteArray, 0, byteArray.Length);
        }

        try
        {
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                string responseText = reader.ReadToEnd();
                Debug.Log("Response text: " + responseText);
                // responseText ^ NEEDS TO BE PARSED INTO VARIABLES IN A CLASS
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
            response.Close();
        }
        catch (WebException e)
        {
            using (WebResponse response = e.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                Debug.Log("Error code: " + httpResponse.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    Debug.Log("Text: " + text);
                }
            }
        }
    }
}