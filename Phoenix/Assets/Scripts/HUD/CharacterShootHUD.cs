using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShootHUD : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private SpriteRenderer targetSprite;
    [SerializeField] private SpriteRenderer pathSprite;

    private void Start()
    {
        TurnOffHUDElements();
    }

    public void SetShootHUDState(Vector3 hitPosition,bool foundValidPoint, bool isValidTarget)
    {
        if(!foundValidPoint)
        {
            TurnOffHUDElements();
            return;
        }
        else
        {
            TurnOnHUDElements();
        }

        transform.position = hitPosition;

        if (!isValidTarget)
        {
            targetSprite.color = Color.red;
            pathSprite.color = Color.red;
        }
        else
        {
            targetSprite.color = Color.green;
            pathSprite.color = Color.green;
        }
    }

    public void TurnOnHUDElements()
    {
        if(!targetSprite.enabled)
        {
            targetSprite.enabled = true;
            pathSprite.enabled = true;
        }
    }

    public void TurnOffHUDElements()
    {
        targetSprite.enabled = false;
        pathSprite.enabled = false;
    }
}
