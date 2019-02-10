using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDiceRoller : PlayerComponent
{
    public event Action DiceEventSetup;
    public event Action PlayerRolledDice;
    public event Action<List<int>, List<int>> DiceRolled;

    private List<Dice> playerDice;
    private int numberOfDiceToRoll;

    //Dice Rolls
    private List<int> localPlayerDiceRolls;
    private List<int> opponentDiceRoll;

    protected override void Start()
    {
        base.Start();
        SpawnInDice();

        localPlayerDiceRolls = new List<int>();
        opponentDiceRoll = new List<int>();
    }

    private void SpawnInDice()
    {
        if (playerView.GetPhotonView.isMine)
        {
            playerDice = new List<Dice>();

            for (int i = 0; i < 5; i++)
            {
                GameObject dice = PhotonNetwork.Instantiate("Dice", Vector3.zero, Quaternion.identity, 0);
                playerDice.Add(dice.GetComponent<Dice>());
            }
        }
    }

    public void SetupDiceRoll(int _numberOfDiceToRoll)
    {
        localPlayerDiceRolls.Clear();
        opponentDiceRoll.Clear();

        numberOfDiceToRoll = _numberOfDiceToRoll;

        if(DiceEventSetup != null)
            DiceEventSetup();
    }

    public void ResetDice()
    {
        StartCoroutine(WaitToCleanUpDice());
    }

    private IEnumerator WaitToCleanUpDice()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < playerDice.Count; i++)
        {
            playerDice[i].GetPhotonView.RPC("ResetDice", PhotonTargets.All);
        }
    }

    public void RollDice()
    {        
        Vector2 screenSpawnPosition = new Vector2(0.6f,0.3f);

        for(int i = 0; i < numberOfDiceToRoll; i++)
        {
            //Determine SpawnPoint
            Ray ray = playerView.GetPlayerCamera.PlayerCamera.ViewportPointToRay(screenSpawnPosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    Vector3 diceSpawnPosition = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);

                    playerDice[i].GetPhotonView.RPC("RollDice", PhotonTargets.All, diceSpawnPosition);                   
                }
            }
            screenSpawnPosition = new Vector2(screenSpawnPosition.x, screenSpawnPosition.y + 0.075f);
        }

        if (PlayerRolledDice != null)
            PlayerRolledDice();
    }

    [PunRPC]
    public void RecieveDiceRolls(int diceRoll, PhotonMessageInfo info)
    {
        if(info.sender == PhotonNetwork.player)
        {
            localPlayerDiceRolls.Add(diceRoll);
        }
        else
        {
            opponentDiceRoll.Add(diceRoll);
        }
        
        if(localPlayerDiceRolls.Count == numberOfDiceToRoll && opponentDiceRoll.Count == numberOfDiceToRoll)
        {
            if (DiceRolled != null)
            {
                DiceRolled(localPlayerDiceRolls, opponentDiceRoll);
            }
        }

    }
}
