using System;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

// working with HTTP requests
public static class OpenAI_API_HTTP
{
    public static bool _isAPICallProcessing = false;
    public static bool _didErrorOccur = false; // is set to true when a response is defective or error happened
    private static string _secretKey;

    public static void GetAPIKey() => _secretKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);

    public static TextGenerationData GenerateText(string _text)
    {
        _isAPICallProcessing = true;
        _didErrorOccur = false;

        string _defaultResponse =
        @"{
            ""id"": """",
            ""object"": """",
            ""created"": 0,
            ""model"": """",
            ""choices"":
            [{
                ""index"": 0,
                ""message"":
                {
                    ""role"": """",
                    ""content"": """"
                },
                ""logprobs"": null,
                ""finish_reason"": """"
            }],
            ""usage"":
            {
                ""prompt_tokens"": 0,
                ""completion_tokens"": 0,
                ""total_tokens"": 0
            },
            ""system_fingerprint"": """"
        }";

        string _AImodel = "gpt-4o";
        string _promptWithHistory = SimulationHistory.history.Replace("\n"," ") + " " + SimulationHistory.GetNewEntryNumber().ToString() + ". New entry: [" + _text + "]";
        string _AISystemSettings =
        "You are a discord kitty currently playing Dark Souls 3 game. Don't ever use special characters, symbols, headers, boldings or markdowns.";

        // API request content
        string _jsonData =
        $@"{{
            ""model"": ""{_AImodel}"",
            ""messages"":
            [
                {{
                    ""role"": ""system"",
                    ""content"": ""{_AISystemSettings}""
                }},
                {{
                    ""role"": ""user"",
                    ""content"": ""{_promptWithHistory}""
                }}
            ]
        }}";

        // API request
        HttpWebRequest _request = (HttpWebRequest)WebRequest.Create("https://api.openai.com/v1/chat/completions");
        Debug.Log(SimulationHistory.history);
        _request.Method = "POST";
        _request.ContentType = "application/json";
        _request.Headers.Add("Authorization", "Bearer " + _secretKey);

        byte[] _jsonDataBytes = Encoding.UTF8.GetBytes(_jsonData);
        _request.ContentLength = _jsonDataBytes.Length;
        _request.GetRequestStream().Write(_jsonDataBytes, 0, _jsonDataBytes.Length);

        // API response
        HttpWebResponse _response = null;
        StreamReader _responseReader = null;
        try
        {
            _response = (HttpWebResponse)_request.GetResponse();
            _responseReader = new StreamReader(_response.GetResponseStream());
            string _responseString = _responseReader.ReadToEnd();
            TextGenerationData _generatedText = JsonUtility.FromJson<TextGenerationData>(_responseString);
            if (_generatedText != null)
            {
                if (_generatedText.choices[0].finish_reason != "stop" && _generatedText.choices[0].finish_reason != "")
                {
                    _didErrorOccur = true;
                    Debug.Log("API error details : " + _generatedText.choices[0].finish_reason);
                }
            }
            return _generatedText;
        }
        // API errors
        catch (WebException _error)
        {
            // BEWARE A BUGGY SECTION
            _didErrorOccur = true;
            HttpWebResponse _errorResponse = (HttpWebResponse)_error.Response;
            Debug.Log("Web error details : " + _errorResponse.StatusCode);
            return JsonUtility.FromJson<TextGenerationData>(_defaultResponse);
        }
        catch (Exception _anotherError)
        {
            // BEWARE ANOTHER BUGGY SECTION
            _didErrorOccur = true;
            Debug.Log("Other error details : " + _anotherError.Message);
            return JsonUtility.FromJson<TextGenerationData>(_defaultResponse);
        }
        finally
        {
            if (_responseReader != null) _responseReader.Close();
            if (_response != null) _response.Close();
            _isAPICallProcessing = false;
        }
    }
}