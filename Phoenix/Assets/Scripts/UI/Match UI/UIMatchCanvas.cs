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

    [Header("Turn Button Elements")]
    [SerializeField] private Image endTurnButtonImage;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TMP_Text endTurnButtonText;

    private string localPlayerName;
    private string opponentName;

    private void Start()
    {
        GameManager.Instance.GetLobbyManager.ClientConnectedToRoom += DisplayPlayerNames;
        GameManager.Instance.GetLobbyManager.OpponentConnectedToRoom += DisplayPlayerNames;

        GameManager.Instance.GetTurnManager.PriorityPhaseInitiated += PriorityPhase;
        GameManager.Instance.GetTurnManager.UpdateToPlayerTurn += PlayerTurnUpdate;
        GameManager.Instance.GetTurnManager.CombatPhaseInitiated += CombatPhaseInitiated;

        playerNameText.text = "";
        opponentNameText.text = "";

        localPlayerTurnImage.enabled = false;
        opponentPlayerTurnImage.enabled = false;

        endTurnButton.enabled = false;
        endTurnButtonImage.enabled = false;
        endTurnButtonText.text = "";

        SetEndTurnButtonDelegate();
    }

    private void SetEndTurnButtonDelegate()
    {
        endTurnButton.onClick.AddListener(delegate { GameManager.Instance.GetTurnManager.EndPlayersTurn(); });
    }

    private void PriorityPhase()
    {
        endTurnButtonText.color = Color.white;
        endTurnButton.enabled = false;
        endTurnButtonImage.enabled = false;
        endTurnButtonText.text = "Priority Phase";
    }

    private void DisplayPlayerNames()
    {
         localPlayerName = PhotonNetwork.player.NickName;

        if (string.IsNullOrEmpty(localPlayerName))
            localPlayerName = "Player One";

        playerNameText.text = localPlayerName;

        if(PhotonNetwork.playerList.Length == 2)
        {
            for(int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                if(PhotonNetwork.playerList[i] != PhotonNetwork.player)
                {
                    opponentName = PhotonNetwork.playerList[i].NickName;

                    if (string.IsNullOrEmpty(opponentName))
                        opponentName = "Player Two";

                    opponentNameText.text = opponentName;
                    break;
                }
            }
        }            
    }

    private void PlayerTurnUpdate(int playerTurnID)
    {
        if(TurnManager.GetTurnPhase == TurnManager.TurnPhase.Shooting || TurnManager.GetTurnPhase == TurnManager.TurnPhase.Movement)
        {
            string phaseType = TurnManager.GetTurnPhase == TurnManager.TurnPhase.Movement ? "Movement" : "Shooting";

            if (playerTurnID == 0)
            {
                localPlayerTurnImage.enabled = true;
                opponentPlayerTurnImage.enabled = false;

                endTurnButton.enabled = true;
                endTurnButtonImage.enabled = true;               

                endTurnButtonText.text = $"End Your {phaseType} Turn";
                endTurnButtonText.color = Color.black;
            }
            else
            {
                localPlayerTurnImage.enabled = false;
                opponentPlayerTurnImage.enabled = true;

                endTurnButtonText.color = Color.white;
                endTurnButton.enabled = false;
                endTurnButtonImage.enabled = false;
                endTurnButtonText.text = $"{phaseType} Phase - {opponentName}";
            }
        }
    }

    private void CombatPhaseInitiated()
    {
        endTurnButtonText.color = Color.white;
        endTurnButton.enabled = false;
        endTurnButtonImage.enabled = false;
        endTurnButtonText.text = "Combat Phase";
    }




}
