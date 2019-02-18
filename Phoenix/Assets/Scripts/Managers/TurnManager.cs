using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : Manager
{
    [Header("Debug Controls")]
    [SerializeField] private bool startWithPriority;

    public event Action PriorityPhaseInitiated;
    public event Action<int> UpdateToPlayerTurn;
    public event Action CombatPhaseInitiated;


    private static int playerTurnID; //0 = localplayer 1 = opponent
    private int numberOfActionTurnsCompleted = 0;

    private static TurnPhase turnPhase;
    public static TurnPhase GetTurnPhase { get { return turnPhase;}}
    public enum TurnPhase
    {
        Priority,
        Movement,
        Shooting,       
        Combat
    }

    protected override void Start()
    {
        base.Start();

        if(startWithPriority)
        {
            gameManager.GetMatchManager.MatchSetup += SetTurnPhaseToPriority;
        }
        else
        {
            turnPhase = TurnPhase.Movement;
            SetPlayerWhoWinsPriority(0);
        }

        
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
        turnPhase = TurnPhase.Movement;
        numberOfActionTurnsCompleted = 0;

        if(UpdateToPlayerTurn != null)
            UpdateToPlayerTurn(playerTurnID);
    }

    #endregion

    #region Action Phase

    public void EndPlayersTurn()
    {
        gameManager.GetPhotonView.RPC("UpdateTurnAndPhase", PhotonTargets.All);
        Debug.Log("End Turn");
    }

    [PunRPC]
    private void UpdateTurnAndPhase()
    {
        if(turnPhase == TurnPhase.Movement || turnPhase == TurnPhase.Shooting)
        {
            numberOfActionTurnsCompleted++;

            if(numberOfActionTurnsCompleted == 2)
            {
                DetermineTheStateOfMovementAndShootingPhases();             
            }
            else
            {
                ShiftTurnIntoPlayersControl();
            }
        }
        else if(turnPhase == TurnPhase.Combat)
        {
            
        }       
    }

    private void DetermineTheStateOfMovementAndShootingPhases()
    {
        if (turnPhase == TurnPhase.Movement)
        {
            InitiateShootingPhase();
            ShiftTurnIntoPlayersControl();
        }
        else if (turnPhase == TurnPhase.Shooting)
        {
            InitiateCombatPhase();
        }

        numberOfActionTurnsCompleted = 0;
    }

    private void ShiftTurnIntoPlayersControl()
    {
        string endTurnMessage = "";
        string phaseType = turnPhase == TurnPhase.Movement ? "Movement" : "Shooting";

        playerTurnID = playerTurnID == 0 ? 1 : 0;

        if (playerTurnID == 0)
        {
            endTurnMessage = $"Your {phaseType} Turn!";
        }
        else
        {
            endTurnMessage = $"{phaseType} Turn - {gameManager.GetMatchManager.OpponentName}";
        }

        gameManager.GetMatchManager.TriggerMatchMessage(endTurnMessage);
        UpdateToPlayerTurn(playerTurnID);
    }


    #endregion

    #region ShootingPhase

    private void InitiateShootingPhase()
    {
        turnPhase = TurnPhase.Shooting;      
    }

    #endregion

    #region Combat Phase

    private void InitiateCombatPhase()
    {
        turnPhase = TurnPhase.Combat;
        CombatPhaseInitiated();
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
