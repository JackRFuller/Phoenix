using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : Manager
{
    public event Action PriorityPhaseInitiated;
    public event Action<int> UpdateToPlayerTurn;

    public event Action ActionPhaseInitiated;

    private static int playerTurnID; //0 = localplayer 1 = opponent
    private int numberOfActionTurnsCompleted = 0;

    private static TurnPhase turnPhase;
    public static TurnPhase GetTurnPhase { get { return turnPhase;}}
    public enum TurnPhase
    {
        Priority,
        Action,
        Combat
    }

    protected override void Start()
    {
        base.Start();
        gameManager.GetMatchManager.MatchSetup += SetTurnPhaseToPriority;
    }

    #region Priority

    private void SetTurnPhaseToPriority()
    {
        turnPhase = TurnPhase.Priority;
        PriorityPhaseInitiated();
    }
    
    public void SetPlayerWhoWinsPriority(int winnerIndex)
    {
        playerTurnID = winnerIndex;
        turnPhase = TurnPhase.Action;
        numberOfActionTurnsCompleted = 0;

        UpdateToPlayerTurn(playerTurnID);
    }

    #endregion

    #region Action Phase

    public void EndPlayersActionTurn()
    {
        gameManager.GetPhotonView.RPC("UpdateTurnAndPhase", PhotonTargets.All);
        Debug.Log("End Turn");
    }

    [PunRPC]
    private void UpdateTurnAndPhase()
    {
        if(turnPhase == TurnPhase.Action)
        {
            numberOfActionTurnsCompleted++;
            if(numberOfActionTurnsCompleted > 2)
            {
                Debug.Log("Combat Phase");                
                numberOfActionTurnsCompleted = 0;
                InitiateCombatPhase();
            }
            else
            {
                string endTurnMessage = "";              
                
                playerTurnID = playerTurnID == 0 ? 1 : 0; 
                
                if(playerTurnID == 0)
                {
                    endTurnMessage = "Your Action Turn!";
                }
                else
                {
                    endTurnMessage = $"Action Turn - {gameManager.GetMatchManager.OpponentName}";
                }

                gameManager.GetMatchManager.TriggerMatchMessage(endTurnMessage);
                UpdateToPlayerTurn(playerTurnID);
            }
        }

        if(turnPhase == TurnPhase.Combat)
        {

        }

        Debug.Log("Turn Updated");
    }

    #endregion

    #region Combat Phase

    private void InitiateCombatPhase()
    {
        turnPhase = TurnPhase.Combat;
        gameManager.GetMatchManager.TriggerMatchMessage("Combat Phase!");
    }

    #endregion

    public static bool IsPlayersTurn()
    {
        if (playerTurnID == 0)
            return true;
        else
            return false;
    }        
}
