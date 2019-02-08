using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon;

public class UIMatchCanvas : Photon.MonoBehaviour
{
    [Header("Player Names")]
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text opponentNameText;

    private void Start()
    {
        GameManager.Instance.GetLobbyManager.ClientConnectedToRoom += DisplayPlayerNames;
        GameManager.Instance.GetLobbyManager.OpponentConnectedToRoom += DisplayPlayerNames;

        playerNameText.text = "";
        opponentNameText.text = "";
    }

    private void DisplayPlayerNames()
    {
        string playerName = PhotonNetwork.player.NickName;

        if (string.IsNullOrEmpty(playerName))
            playerName = "Player One";

        playerNameText.text = playerName;


        if(PhotonNetwork.playerList.Length == 2)
        {
            for(int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                if(PhotonNetwork.playerList[i] != PhotonNetwork.player)
                {
                    string opponentName = PhotonNetwork.playerList[i].NickName;

                    if (string.IsNullOrEmpty(opponentName))
                        playerName = "Player Two";

                    opponentNameText.text = opponentName;
                    break;
                }
            }
        }            
    }




}
