using System.Net;
using System.IO;
using UnityEngine;

public static class ChuckNorrisJokeAPI
{
    public static ChuckNorrisJoke GetNewJoke()
    {
        // calling API
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.chucknorris.io/jokes/random");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        // parsing called API's content is going on here
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonString = reader.ReadToEnd();
        return JsonUtility.FromJson<ChuckNorrisJoke>(jsonString);
    }
}