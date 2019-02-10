using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestRange;
using System;

public class MatchManager : Manager
{
    public event Action MatchSetup;
    public event Action<string> MatchMessage;

    private PlayerView localPlayer;
    public PlayerView LocalPlayer { get { return localPlayer; } }

    private List<PlayerView> players;
    public List<PlayerView> Players { get { return players; } }

    private string localPlayerName;
    private string opponentName;

    public string LocalPlayerName { get { return localPlayerName; } }
    public string OpponentName { get { return opponentName; } }

    protected override void Start()
    {
        base.Start();
        gameManager.GetLobbyManager.RoomFull += SetupMatch;

        players = new List<PlayerView>();
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
    }

    public void TriggerMatchMessage(string matchMessage)
    {
        if (MatchMessage != null)
            MatchMessage(matchMessage);
    }

    public void RecievePlayers(PlayerView player)
    {
        players.Add(player);

        if(player.GetPhotonView.isMine)
        {
            localPlayer = player;            
        }            

        //Check if we have both players
        if (players.Count == gameManager.GetLobbyManager.RequiredNumberOfPlayers)
        {
            //Set Player Names
            for(int i =0; i < players.Count; i++)
            {
                if (PhotonNetwork.playerList[i].IsLocal)
                {
                    localPlayerName = PhotonNetwork.playerList[i].NickName;
                }
                else
                {
                    opponentName = PhotonNetwork.playerList[i].NickName;
                }
            }

            StartCoroutine(WaitToStartMatch());
        }
    }

    //Temp / find a better way to sort out / used to make sure all components have been cached
    IEnumerator WaitToStartMatch()
    {
        yield return new WaitForSeconds(2.0f);

        if (MatchSetup != null)
            MatchSetup();
    }
}
