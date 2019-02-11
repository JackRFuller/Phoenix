using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : CharacterComponent
{
    private int currentHitPoints;
    public int CurrentHitPoints { get { return currentHitPoints; } }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentHitPoints = characterView.GetCharacterData.hitPoints;
    }

    [PunRPC]
    private void RemoveHealth(int hitPointsToRemove)
    {
        currentHitPoints -= hitPointsToRemove;

        if(currentHitPoints <= 0)
        {
            Debug.Log("Character Dead");
        }
    }
}
