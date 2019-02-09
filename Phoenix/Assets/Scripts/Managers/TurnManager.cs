using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : Manager
{
    public event Action PriorityPhaseInitiated;
    public event Action<int> UpdateToPlayerTurn;

    private static int playerTurnID;

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

    private void SetTurnPhaseToPriority()
    {
        turnPhase = TurnPhase.Priority;

        PriorityPhaseInitiated();
    }
    
    public void SetPlayerWhoWinsPriority(int winnerIndex)
    {
        playerTurnID = winnerIndex;
        turnPhase = TurnPhase.Action;

        UpdateToPlayerTurn(playerTurnID);
    }

    public static bool IsPlayersTurn()
    {
        if (playerTurnID == 0)
            return true;
        else
            return false;
    }        
}
