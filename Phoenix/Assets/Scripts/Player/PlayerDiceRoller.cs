using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceRoller : PlayerComponent
{
    private List<Dice> playerDice;

    protected override void Start()
    {
        base.Start();

        playerDice = new List<Dice>();

        for(int i = 0; i < 5; i++)
        {
            //GameObject dice = PhotonNetwork.Instantiate("Dice", Vector3.zero, Quaternion.identity, 0);
            //playerDice.Add(dice.GetComponent<Dice>());
        }
    }

    public void RollDice()
    {
        Debug.Log("Roll Dice");
    }
}
