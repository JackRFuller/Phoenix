using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoveHUD : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private SpriteRenderer destinationSprite;
    [SerializeField] private Transform destinationTransform;
    [SerializeField] private LineRenderer navPath;

    private void Start()
    {
        HideHUDElements();
    }

    public void ShowCharacterNavMeshPath(NavMeshPath navMeshPath,bool pathIsValid, bool foundPath)
    {
        if(foundPath)
        {
            destinationSprite.enabled = true;
            destinationTransform.position = navMeshPath.corners[navMeshPath.corners.Length -1];

            navPath.enabled = true;
            navPath.positionCount = navMeshPath.corners.Length;

            for(int i =0; i < navMeshPath.corners.Length;i++)
            {
                navPath.SetPosition(i, new Vector3(navMeshPath.corners[i].x,
                                                  navMeshPath.corners[i].y + 0.05f,
                                                  navMeshPath.corners[i].z));
            }

            if(!pathIsValid)
            {
                destinationSprite.color = Color.red;
                navPath.startColor = Color.red;
                navPath.endColor = Color.red;
            }
            else
            {
                destinationSprite.color = Color.white;
                navPath.startColor = Color.white;
                navPath.endColor = Color.white;
            }
        }
        else
        {
            HideHUDElements();
        }
    }

    public void CharacterReachedDestination()
    {
        HideHUDElements();
    }

    public void HideHUDElements()
    {
        destinationSprite.enabled = false;
        navPath.enabled = false;
    }
}
