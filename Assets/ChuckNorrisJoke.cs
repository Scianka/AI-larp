[System.Serializable]
public class ChuckNorrisJoke
{
    // here goes an API call's parsed data,
    // so it can be accessed by using fields of this class,
    // when the function handling the API is called
    public string[] categories;
    public string created_at;
    public string icon_url;
    public string id;
    public string updated_at;
    public string url;
    public string value;
}