[System.Serializable]
public class TextGenerationData
{
    /*public string id;
    //public string object;
    public int created;
    public string model;
    public string[] choices;
        public int index;
        public string message;
            public string role;
            public string content;
        public string logprobs;
        public string finish_reason;
    public string usage;
        public string prompt_tokens;
        public string completion_tokens;
        public string total_tokens;
    public string system_fingerprint;*/
}


/*
 * THIS WORKED IN CMD !
    curl https://api.openai.com/v1/chat/completions -H "Content-Type: application/json" -H "Authorization: Bearer sk-proj-9EXnzTCuLOWBMOJGzTMrT3BlbkFJXV5tdQ5VXCpYqlRn5mAW" -d "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"system\", \"content\": \"You are a poetic assistant, skilled in explaining complex programming concepts with creative flair.\"}, {\"role\": \"user\", \"content\": \"Compose a poem that explains the concept of recursion in programming.\"}]}"
*/


/*
 * THE CURL'S RESPONSE !
    {
      "id": "chatcmpl-9dfMRkv2ykYKXJMcw02oUiMa8AxPr",
      "object": "chat.completion",
      "created": 1719240811,
      "model": "gpt-3.5-turbo-0125",
      "choices": [
        {
          "index": 0,
          "message": {
            "role": "assistant",
            "content": "In the realm of coding's deep esoteric art,\nThere lies a concept that sets minds apart.\nRecursion, a loop of mystical grace,\nA method so elegant, in a programming race.\n\nLike a mirror reflecting its own reflection,\nRecursion calls upon its own collection.\nA function that calls itself again and again,\nTackling problems with a recursive refrain.\n\nWith each iteration, it peels back the veil,\nSolving puzzles with a recursive tale.\nA fractal pattern, endlessly repeating,\nIn a dance of logic, ever completing.\n\nFrom Fibonacci sequences to tree traversal's maze,\nRecursion shines in its complex ways.\nDivide and conquer, conquer and divide,\nRecursive magic in code does reside.\n\nSo embrace recursion, let your code unfold,\nIn its poetry of loops, mysterious and bold.\nA looping cycle, a dance of delight,\nIn the symphony of code, recursion takes flight."
          },
          "logprobs": null,
          "finish_reason": "stop"
        }
      ],
      "usage": {
        "prompt_tokens": 39,
        "completion_tokens": 181,
        "total_tokens": 220
      },
      "system_fingerprint": null
    }
*/