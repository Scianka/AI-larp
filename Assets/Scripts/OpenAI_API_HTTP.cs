using System;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

// this script can handle only one API call at a time
public static class OpenAI_API_HTTP
{
    public static bool _isAPICallProcessing = false;
    public static bool _didErrorOccur = false; // is set to true when a response is defective or an error happened
    private static string _secretKey; // this needs to be set manually for the tests' sake

    public static string players_info;
    public static bool is_first_update = true;
    private static int last_update_hour;
    private static int last_update_minute;
    private static int last_update_second;

    public static void GetAPIKey() => _secretKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);

    public static TextGenerationData GenerateText(string new_prompt)
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
        string _AISystemSettings = SystemPrompt._systemPromptRole + players_info + SystemPrompt._systemPromptRest;
        string time_info = "The real time is " + GetRealTime() + ". The time that has passed in the simulation since the last dungeon's update is " + GetSimulationTime() + ".";
        string full_prompt =
        "Is first update: [" + is_first_update.ToString() + "] " +
        "New user prompt: [" + new_prompt + "] " +
        "Time calculations: [" + time_info + "] " +
        "Subtle twist occurrence: [" + DrawTwist() + "] " + 
        "Last dungeon's state: [" + SimulationHistory.last_state + "] " +
        "Last system response: [" + SimulationHistory.last_response + "] " +
        "Last user prompt: [" + SimulationHistory.last_prompt + "] " +
        "Session summary: [" + SimulationHistory.session_summary + "]";
        Debug.Log("Full prompt:\n" + full_prompt);

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
                    ""content"": ""{full_prompt}""
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

    public static string GetRealTime() { return DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString(); }

    public static string GetSimulationTime()
    {
        int hours_passed = DateTime.Now.Hour - last_update_hour;
        int minutes_passed = DateTime.Now.Minute - last_update_minute;
        int seconds_passed = DateTime.Now.Second - last_update_second;
        if (is_first_update) return 0 + ":" + 0 + ":" + 0;
        else return hours_passed + ":" + minutes_passed + ":" + seconds_passed;
    }

    public static void SaveUpdateTime()
    {
        last_update_hour = DateTime.Now.Hour;
        last_update_minute = DateTime.Now.Minute;
        last_update_second = DateTime.Now.Second;
    }

    public static string DrawTwist()
    {
        int d100_roll = UnityEngine.Random.Range(1, 101);
        if (d100_roll <= 30) return "Introduce the subtle twist to this update. ";
        else return "Don't introduce the subtle twist to this update. ";
    }
}