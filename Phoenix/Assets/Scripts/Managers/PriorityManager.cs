using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityManager : Manager
{
    private List<Transform> prioritySpots;

    protected override void Start()
    {
        base.Start();

        gameManager.GetTurnManager.PriorityPhaseInitiated += StartPriorityPhase;

        prioritySpots = new List<Transform>();
    }

    private void StartPriorityPhase()
    {
        if(prioritySpots.Count == 0)
        {
            prioritySpots.Add(gameManager.GetLevelManager.GetLevelData.prioritySpotOne);
            prioritySpots.Add(gameManager.GetLevelManager.GetLevelData.prioritySpotTwo);
        }

        //Determine PrioritySpot
        Transform priorityPosition = PhotonNetwork.isMasterClient ? prioritySpots[0] : prioritySpots[1];

        //Trigger players into priority spots
        gameManager.GetMatchManager.LocalPlayer.GetPlayerCamera.LockCameraToSpecificPosition(priorityPosition.position, priorityPosition.eulerAngles);
        gameManager.GetMatchManager.TriggerMatchMessage("Roll For Priority");

        gameManager.GetMatchManager.LocalPlayer.GetPlayerDiceRoller.SetupDiceRoll(1);
        gameManager.GetMatchManager.LocalPlayer.GetPlayerDiceRoller.DiceRolled += CompareDiceResults;
    }

    private void CompareDiceResults(List<int> localPlayerDiceRolls, List<int> opponentDiceRolls)
    {
        string winnerName = null;

        int winnerIndex = 0;

        if(localPlayerDiceRolls[0] > opponentDiceRolls[0])
        {
            winnerName = PhotonNetwork.player.NickName;            
        }
        else if(localPlayerDiceRolls[0] < opponentDiceRolls[0])
        {
            winnerIndex = 1;

            for (int i = 0; i < PhotonNetwork.playerList.Length;i++)
            {
                if(PhotonNetwork.playerList[i] != PhotonNetwork.player)
                {
                    winnerName = PhotonNetwork.playerList[i].NickName;
                    break;
                }
            }
        }

        string winnerMessage = $"{winnerName} Wins Priority. {winnerName} Goes First";
        gameManager.GetMatchManager.TriggerMatchMessage(winnerMessage);

        gameManager.GetTurnManager.SetPlayerWhoWinsPriority(winnerIndex);

        gameManager.GetMatchManager.LocalPlayer.GetPlayerDiceRoller.ResetDice();
    }

    
}
