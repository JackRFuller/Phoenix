using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestRange;

public class MatchManager : MonoBehaviour
{
    private List<PlayerView> players;
    public List<PlayerView> Players { get { return players; } }

    private void Start()
    {
        players = new List<PlayerView>();
    }

    public void GetAllPlayers()
    {
        PlayerView[] playerViews = FindObjectsOfType<PlayerView>();
        for(int i = 0; i < playerViews.Length; i++)
        {
            players.Add(playerViews[i]);
        }
    }
}
