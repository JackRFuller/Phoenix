using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoveHUD : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private SpriteRenderer destinationSprite;
    [SerializeField] private Transform destinationTransform;

    private void Start()
    {
        HideHUDElements();
    }

    public void ShowCharacterNavMeshPath(NavMeshPath navMeshPath,bool pathIsValid, bool foundPath)
    {
        if (foundPath)
        {
            if(!destinationSprite.enabled)
            {
                destinationSprite.enabled = true;
            }           

            if(pathIsValid)
            {
                destinationSprite.color = Color.white;
            }
            else
            {
                destinationSprite.color = Color.red;
            }

            destinationTransform.position = navMeshPath.corners[navMeshPath.corners.Length -1];
        }
        else
        {
            destinationSprite.enabled = false;
        }
    }

    public void CharacterReachedDestination()
    {
        HideHUDElements();
    }

    public void HideHUDElements()
    {
        destinationSprite.enabled = false;
    }
}
