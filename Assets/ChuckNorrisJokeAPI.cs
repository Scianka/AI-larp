using System.Net;
using System.IO;
using UnityEngine;

public static class ChuckNorrisJokeAPI
{
    // when this function is called (by another script) it creates and handles API calls and then parses their data into more accessible format (class instance)
    public static ChuckNorrisJoke GetNewJoke()
    {
        // creating&handling API calls
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.chucknorris.io/jokes/random");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        // parsing API call's data
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonString = reader.ReadToEnd();
        return JsonUtility.FromJson<ChuckNorrisJoke>(jsonString);
    }
}