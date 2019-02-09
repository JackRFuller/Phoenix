using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon;
using UnityEngine.UI;

public class UIMatchCanvas : Photon.MonoBehaviour
{
    [Header("Player Names")]
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text opponentNameText;

    [Header("Player Turn IDs")]
    [SerializeField] private Image localPlayerTurnImage;
    [SerializeField] private Image opponentPlayerTurnImage;

    private void Start()
    {
        GameManager.Instance.GetLobbyManager.ClientConnectedToRoom += DisplayPlayerNames;
        GameManager.Instance.GetLobbyManager.OpponentConnectedToRoom += DisplayPlayerNames;

        GameManager.Instance.GetTurnManager.UpdateToPlayerTurn += PlayerTurnUpdate;

        playerNameText.text = "";
        opponentNameText.text = "";

        localPlayerTurnImage.enabled = false;
        opponentPlayerTurnImage.enabled = false;
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

    private void PlayerTurnUpdate(int playerTurnID)
    {
        if(playerTurnID == 0)
        {
            localPlayerTurnImage.enabled = true;
            opponentPlayerTurnImage.enabled = false;
        }
        else
        {
            localPlayerTurnImage.enabled = false;
            opponentPlayerTurnImage.enabled = true;
        }
    }




}
