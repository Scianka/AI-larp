using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using OpenAI.Chat;

// working with openai-dotnet API library 6
public class OpenAI_API : MonoBehaviour
{
    private OpenAIClient _APIClient;
    private ChatClient _chatClient;

    private void Start()
    {
        InitializeClients();
        AskSingleQuestion();
    }

    private void InitializeClients()
    {
        string secret_key = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);
        _APIClient = new OpenAIClient(secret_key);
        _chatClient = new ChatClient("gpt-4o", secret_key);
    }

    private void AskSingleQuestion()
    {
        // nothing works and I tried many code snippets from the official documentation...
    }
}