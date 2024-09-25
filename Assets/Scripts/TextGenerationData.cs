using System;

[Serializable]
public class TextGenerationData
{
    public string id;
    public string @object;
    public long created;
    public string model;
    public Choices[] choices;
    public Usage usage;
    public string system_fingerprint;

    public string content { set; get; }

    public string GetContent()
    {
        if (choices[0].finish_reason != "")
        {
            content = choices[0].message.content;
            return content;
        }
        else return "...";
    }
}

    [Serializable]
    public class Choices
    {
        public int index;
        public Message message;
        public object logprobs;
        public string finish_reason;
    }

        [Serializable]
        public class Message
        {
            public string role;
            public string content;
        }

    [Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }