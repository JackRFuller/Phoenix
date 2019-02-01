using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharacterMovement : CharacterComponent
{
    private Camera playerCamera;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath navMeshPath;

    private CharacterMoveHUD characterMoveHUD;

    private MovementState movementState = MovementState.Still;
    private enum MovementState
    {
        Still,
        Searching,
        Moving,
    }

    #region UnityMethods

    protected override void Start()
    {
        base.Start();

        characterView.CharacterSelected += CharacterSelectedByPlayer;
        characterView.CharacterDeselected += CharacterDeselectedByPlayer;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
    }

    private void Update()
    {
        SearchForLocationToMoveTo();
    }

    #endregion

    #region CharacterSelectionEvents

    private void CharacterSelectedByPlayer()
    {
        playerCamera = characterView.GetPlayerView.GetPlayerCamera.PlayerCamera;

        //Subscribe to Events
        characterView.GetPlayerView.GetPlayerUI.GetPlayerUIView.GetCharacterActionsUI.PlayerInitiatedCharacterMove += MovementIntiated;
    }

    private void CharacterDeselectedByPlayer()
    {
        characterView.GetPlayerView.GetPlayerUI.GetPlayerUIView.GetCharacterActionsUI.PlayerInitiatedCharacterMove -= MovementIntiated;
    }

    #endregion

    private void MovementIntiated()
    {
        if (characterMoveHUD == null)
            characterMoveHUD = characterView.GetPlayerView.GetPlayerInteraction.GetCharacterMoveHUD;

        movementState = MovementState.Searching;
    }

    private void SearchForLocationToMoveTo()
    {
        if (movementState == MovementState.Searching)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool foundPath = false;
            bool validPath = false;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    //Calculate Path
                    NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, navMeshPath);

                    foundPath = true;

                    //Debug
                    for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
                        Debug.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i + 1], Color.red);

                    //Check if its less than max movement distance
                    if (navMeshPath.status != NavMeshPathStatus.PathInvalid)
                    {
                        if (ReturnIfNavMeshPathIsLessThanMaxMovementDistance())
                        {
                            validPath = true;
                            if (!EventSystem.current.IsPointerOverGameObject())
                            {
                                if (characterView.GetPlayerView.GetPlayerInput.SelectInput)
                                {
                                   
                                    StartCharacterMovement(hit.point);
                                }
                            }
                        }
                    }
                }
            }

            if(characterMoveHUD)
                characterMoveHUD.ShowCharacterNavMeshPath(navMeshPath, validPath, foundPath);
        }
    }

    private void StartCharacterMovement(Vector3 _targetDestination)
    {
        movementState = MovementState.Moving;
        navMeshAgent.destination = _targetDestination;
    }

    private bool ReturnIfNavMeshPathIsLessThanMaxMovementDistance()
    {
        float pathDistance = 0.0f;

        for(int pathCorners = 1; pathCorners < navMeshPath.corners.Length; pathCorners++)
        {
            pathDistance += Vector3.Distance(navMeshPath.corners[pathCorners - 1], navMeshPath.corners[pathCorners]);
        }

        if (pathDistance <= characterView.GetCharacterData.maxMovementDistance)
            return true;
        else return false;
    }
}
