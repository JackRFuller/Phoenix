using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharacterMovement : CharacterAction
{    
    private NavMeshAgent navMeshAgent;
    private NavMeshPath navMeshPath;

    private CharacterMoveHUD characterMoveHUD;

    private Vector3 oldPosition;
    private Vector3 originalPosition;

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
            SearchForLocation();           
            CancelAction();
        }        
    }

    #endregion

    #region CharacterSelectionEvents

    protected override void CharacterSelectedByPlayer()
    {
        base.CharacterSelectedByPlayer();

        if(characterView.GetPhotonView.isMine)
        {
            if (TurnManager.GetTurnPhase == TurnManager.TurnPhase.Movement && TurnManager.IsPlayersTurn())
            {
                if(TurnManager.IsPlayersTurn())
                {
                    StartCoroutine(CooldownInputBeforeStartingMovement());
                }                
            }
        }        
    }

    private IEnumerator CooldownInputBeforeStartingMovement()
    {
        yield return new WaitForEndOfFrame();

        if (!hasPerformedAction)
            InitiateCharacterMovement();

        if (characterMoveHUD == null)
            characterMoveHUD = characterView.GetPlayerView.GetPlayerInteraction.GetCharacterMoveHUD;       
    }

    protected override void CharacterDeselectedByPlayer()
    {
        base.CharacterDeselectedByPlayer();        
    }

    #endregion

    #region Character Action UI Methods    

    public override void CancelAction()
    {
        if(Input.GetMouseButtonDown(1))
        {
            base.CancelAction();

            if (actionState == ActionState.InProgress)
            {
                actionState = ActionState.Started;
                ResetCharacterPosition();
                characterMoveHUD.HideHUDElements();
            }
        }        
    }

    #endregion

    private void InitiateCharacterMovement()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        actionState = ActionState.InProgress;

        originalPosition = transform.position;
    }

    private void ResetCharacterPosition()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        transform.position = originalPosition;
        characterMoveHUD.HideHUDElements();
    }

    private void SearchForLocation()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool foundPath = false;
        bool validPath = false;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if(hit.collider.CompareTag("Terrain"))
            {
                oldPosition = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);

                //Calculate Path
                NavMesh.CalculatePath(originalPosition, hit.point, NavMesh.AllAreas, navMeshPath);

                //Check if its less than max movement distance
                if (navMeshPath.status != NavMeshPathStatus.PathInvalid)
                {
                    foundPath = true;

                    if (ReturnIfNavMeshPathIsLessThanMaxMovementDistance())
                    {
                        validPath = true;

                        Collider[] hitColliders = Physics.OverlapSphere(hit.point,1);

                        for(int i = 0; i < hitColliders.Length;i++)
                        {

                        }
                            
                        if (Input.GetMouseButtonDown(0))
                        {
                            SetCharacterToPosition(hit.point);
                        }                        
                    }
                }
            }
        }

        //Used to hide character HUD after character has moved
        if(!hasPerformedAction)
        {
            transform.position = oldPosition;

            if (characterMoveHUD)
                characterMoveHUD.ShowCharacterNavMeshPath(navMeshPath, validPath, foundPath);
        }
    }

    private void SetCharacterToPosition(Vector3 _targetPosition)
    {
        hasPerformedAction = true;
        //characterView.CancelledOrPerformedCharacterAction();

        actionState = ActionState.Finished;
        MatchManager.SendBattleLogMessage(BattleLogMessage.movement, $"{characterView.GetCharacterData.characterName}: {originalPosition} to {_targetPosition}");

        
        characterMoveHUD.HideHUDElements();
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        ActionCompleted();
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
