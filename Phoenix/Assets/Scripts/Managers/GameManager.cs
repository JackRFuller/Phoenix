using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private PhotonView photonView;
    private MatchManager matchManager;
    private LobbyManager lobbyManager;
    private LevelManager levelManager;
    private TurnManager turnManager;

    public PhotonView GetPhotonView { get { return photonView; } }
    public MatchManager GetMatchManager { get { return matchManager; } }
    public LobbyManager GetLobbyManager { get { return lobbyManager; } }
    public LevelManager GetLevelManager { get { return levelManager; } }
    public TurnManager GetTurnManager { get { return turnManager; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        photonView = GetComponent<PhotonView>();
        matchManager = GetComponent<MatchManager>();
        lobbyManager = GetComponent<LobbyManager>();
        levelManager = GetComponent<LevelManager>();
        turnManager = GetComponent<TurnManager>();
    }
}
