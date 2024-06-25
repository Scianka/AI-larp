using System.Net;
using System.IO;
using UnityEngine;

public static class ChuckNorrisJokeAPI
{
    // when this function is called (by another script) it handles an API call and then parses received data into more accessible format
    public static ChuckNorrisJoke GetNewJoke()
    {
        // handling API call
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.chucknorris.io/jokes/random");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        // parsing API call's data
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonString = reader.ReadToEnd();
        return JsonUtility.FromJson<ChuckNorrisJoke>(jsonString);
    }
}