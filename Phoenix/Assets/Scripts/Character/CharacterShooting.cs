using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterShooting : CharacterAction
{
    public event Action<CharacterView,CharacterView> CharacterShootingAtTarget;

    private RangedWeaponData rangedWeapon;
    private CharacterShootHUD characterShootHUD;

    //Target
    private Transform targetCharacterTransform;
    private CharacterView targetCharacterView;

    private bool hasShot = false;

    #region UnityMethods

    protected override void Start()
    {
        base.Start();
        rangedWeapon = characterView.GetCharacterData.rangedWeapon;
    }

    private void Update()
    {
        SearchForTargets();
    }

    #endregion

    protected override void CharacterSelectedByPlayer()
    {
        base.CharacterSelectedByPlayer();
        characterView.GetPlayerView.GetPlayerUI.GetPlayerUIView.GetCharacterActionsUI.PlayerInitiatedCharacterShoot += IntiateAction;

        if (characterShootHUD == null)
            characterShootHUD = characterView.GetPlayerView.GetPlayerInteraction.GetCharacterShootHUD;
    }

    protected override void CharacterDeselectedByPlayer()
    {
        base.CharacterDeselectedByPlayer();
        characterView.GetPlayerView.GetPlayerUI.GetPlayerUIView.GetCharacterActionsUI.PlayerInitiatedCharacterMove -= IntiateAction;
    }

    protected override void IntiateAction()
    {
        base.IntiateAction();
        actionState = ActionState.InProgress;        
    }

    public override void CancelAction()
    {
        base.CancelAction();
        if(actionState == ActionState.InProgress)
        {
            actionState = ActionState.Finished;
            characterShootHUD.TurnOffHUDElements();
        }       
    }

    private void SearchForTargets()
    {
        if (hasShot)
            return;

        if (actionState != ActionState.InProgress)
            return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 hitPosition = Vector3.zero;
        bool foundValidPoint = false;
        bool isValidTarget = false;

        if(Physics.Raycast(ray, out hit, 100.0f))
        {
            hitPosition = hit.point;
            foundValidPoint = true;
            
            if(hit.collider.CompareTag("Character"))
            {
                if (targetCharacterTransform == null || targetCharacterTransform != hit.transform)
                {
                    targetCharacterTransform = hit.transform;
                    targetCharacterView = targetCharacterTransform.GetComponent<CharacterView>();
                }

                hitPosition = targetCharacterTransform.position;

                //Check if character is mine
                if(!targetCharacterView.GetPhotonView.isMine)
                {                   
                    //Calculate if character has line of sight to target
                    if (ReturnIfCharacterHasLineOfSightOnTarget())
                    {
                        //Calculate if character is in range
                        if (ReturnIfCharacterIsInRange())
                        {                           
                            isValidTarget = true;

                            if (characterView.GetPlayerView.GetPlayerInput.SelectInput)
                            {                                
                                TurnToFaceTarget();

                                if (CharacterShootingAtTarget != null)
                                    CharacterShootingAtTarget(characterView, targetCharacterView);

                                hasShot = true;

                                ActionCompleted();
                            }
                        }
                    }
                }                
            }
        }

        characterShootHUD.SetShootHUDState(hitPosition, foundValidPoint, isValidTarget);
    }

    private void PlayerPerformedAction()
    {
        hasPerformedAction = true;
    }

    private void TurnToFaceTarget()
    {
        Vector3 lookAtTarget = new Vector3(targetCharacterTransform.position.x,
                                            transform.position.y,
                                            targetCharacterTransform.position.z);

        transform.LookAt(lookAtTarget);
    }

    private bool ReturnIfCharacterHasLineOfSightOnTarget()
    {
        Vector3 characterEyeHeight = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Vector3 targetEyeHeight = new Vector3(targetCharacterTransform.position.x, targetCharacterTransform.position.y + 1, targetCharacterTransform.position.z);

        RaycastHit hit;
        Ray ray = new Ray(characterEyeHeight, targetEyeHeight - characterEyeHeight);
        Debug.DrawRay(ray.origin, ray.direction, Color.red,100);
        
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.transform == targetCharacterTransform)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private bool ReturnIfCharacterIsInRange()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetCharacterTransform.position);

        if(distanceToTarget < rangedWeapon.weaponRange)
        {
            return true;
        }

        return false;
    }
}
