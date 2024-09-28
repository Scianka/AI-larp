using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationData : MonoBehaviour
{
    public int _howManyPlayersAttendingSession = 1;

    private void Start()
    {
        OpenAI_API_HTTP.players_info = GetPlayersInfo();
    }

    private string GetPlayersInfo()
    {
        if (_howManyPlayersAttendingSession == 1) return "You will be interacting with only one player during this session.";
        else return "You will be interacting with " + _howManyPlayersAttendingSession.ToString() + " players during this session.";
    }
}