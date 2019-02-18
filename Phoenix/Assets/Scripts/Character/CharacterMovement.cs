﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharacterMovement : CharacterAction
{    
    private NavMeshAgent navMeshAgent;
    private NavMeshPath navMeshPath;

    private CharacterMoveHUD characterMoveHUD; 

    #region UnityMethods

    protected override void Start()
    {
        base.Start();        

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
    }

    private void Update()
    {
        if(actionState == ActionState.InProgress)
        {
            SearchForLocationToMoveTo();
            CancelAction();
        }
        if(actionState == ActionState.Started)
        {
            CheckIfPlayerHasReachedTheirDestination();
        }    
    }

    #endregion

    #region CharacterSelectionEvents

    protected override void CharacterSelectedByPlayer()
    {
        base.CharacterSelectedByPlayer();
        //Subscribe to Movement Events
        characterView.GetPlayerView.GetPlayerUI.GetPlayerUIView.GetCharacterActionsUI.PlayerInitiatedCharacterMove += IntiateAction;
    }

    protected override void CharacterDeselectedByPlayer()
    {
        base.CharacterDeselectedByPlayer();
        characterView.GetPlayerView.GetPlayerUI.GetPlayerUIView.GetCharacterActionsUI.PlayerInitiatedCharacterMove -= IntiateAction;
    }

    #endregion

    #region Character Action UI Methods

    /// <summary>
    /// Called from Character Action UI Buttons
    /// </summary>
    protected override void IntiateAction()
    {
        base.IntiateAction();

        if (characterMoveHUD == null)
            characterMoveHUD = characterView.GetPlayerView.GetPlayerInteraction.GetCharacterMoveHUD;

        actionState = ActionState.InProgress;
    }

    public override void CancelAction()
    {
        if(Input.GetMouseButtonDown(1))
        {
            base.CancelAction();

            if (actionState == ActionState.InProgress)
            {
                actionState = ActionState.Finished;
                characterMoveHUD.HideHUDElements();
            }
        }        
    }

    #endregion

    private void SearchForLocationToMoveTo()
    {
        if (hasPerformedAction)
            return;

        if (actionState == ActionState.InProgress)
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

                    //Debug
                    for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
                        Debug.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i + 1], Color.red);

                    //Check if its less than max movement distance
                    if (navMeshPath.status != NavMeshPathStatus.PathInvalid)
                    {
                        foundPath = true;

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
        hasPerformedAction = true;
        characterView.CancelledOrPerformedCharacterAction();

        actionState = ActionState.Started;
        navMeshAgent.destination = _targetDestination;

        MatchManager.SendBattleLogMessage(BattleLogMessage.movement, $"{characterView.GetCharacterData.characterName}: {transform.position} to {_targetDestination}");
        ActionCompleted();
    }

    private void CheckIfPlayerHasReachedTheirDestination()
    {
        if(actionState == ActionState.Started)
        {           
            if(!navMeshAgent.pathPending)
            {
                if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {                    
                    if(!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        actionState = ActionState.Finished;                        
                        characterMoveHUD.HideHUDElements();                       
                    }
                }
            }
        }
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
