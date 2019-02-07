using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestRange;
using System;

public class MatchManager : Manager
{
    public event Action MatchSetup;

    private List<PlayerView> players;
    public List<PlayerView> Players { get { return players; } }

    protected override void Start()
    {
        base.Start();
        gameManager.GetLobbyManager.RoomFull += SetupMatch;
    }

    private void SetupMatch()
    {
        if (PhotonNetwork.player.IsMasterClient)
            SetupMatchForPlayerOne();
        else
            SetupMatchForPlayerTwo();
    }

    private void SetupMatchForPlayerOne()
    {
        Transform playerSpawnPoint = gameManager.GetLevelManager.GetLevelData.playerOneSpawnPoint;
        Transform[] characterSpawnPoints = gameManager.GetLevelManager.GetLevelData.teamOneSpawnPoints;
        PhotonNetwork.Instantiate("Player", playerSpawnPoint.position, playerSpawnPoint.rotation, 0);

        for(int i = 0; i < characterSpawnPoints.Length; i++)
        {
            PhotonNetwork.Instantiate("Character", characterSpawnPoints[i].position, characterSpawnPoints[i].rotation, 0);
        }

        MatchSetup();
    }

    private void SetupMatchForPlayerTwo()
    {
        Transform playerSpawnPoint = gameManager.GetLevelManager.GetLevelData.playerTwoSpawnPoint;
        PhotonNetwork.Instantiate("Player", playerSpawnPoint.position, playerSpawnPoint.rotation, 0);

        Transform[] characterSpawnPoints = gameManager.GetLevelManager.GetLevelData.teamTwoSpawnPoints;
        for (int i = 0; i < characterSpawnPoints.Length; i++)
        {
            PhotonNetwork.Instantiate("Character", characterSpawnPoints[i].position, characterSpawnPoints[i].rotation, 0);
        }

        MatchSetup();
    }
}
