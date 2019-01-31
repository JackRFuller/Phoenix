using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : CharacterComponent
{
    private Camera playerCamera;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath navMeshPath;

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
        movementState = MovementState.Searching;
    }

    private void SearchForLocationToMoveTo()
    {
        if (movementState == MovementState.Searching)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    Debug.Log("Found Terrain");
                }
            }
        }
    }
}
