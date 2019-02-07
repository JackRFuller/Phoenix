using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private MatchManager matchManager;
    private LobbyManager lobbyManager;
    private LevelManager levelManager;

    public MatchManager GetMatchManager { get { return matchManager; } }
    public LobbyManager GetLobbyManager { get { return lobbyManager; } }
    public LevelManager GetLevelManager { get { return levelManager; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        matchManager = GetComponent<MatchManager>();
        lobbyManager = GetComponent<LobbyManager>();
        levelManager = GetComponent<LevelManager>();
    }
}
