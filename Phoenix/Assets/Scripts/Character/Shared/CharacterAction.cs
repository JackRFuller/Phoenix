using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAction : CharacterComponent
{
    public event Action CharacterActionPerformed;

    protected Camera playerCamera;

    protected bool hasPerformedAction;
    public bool HasPerformedAction { get { return hasPerformedAction; } }

    protected ActionState actionState = ActionState.Finished;
    protected enum ActionState
    {
        Finished,
        InProgress,
        Started,
    }

    protected override void Start()
    {
        base.Start();

        characterView.CharacterSelected += CharacterSelectedByPlayer;
        characterView.CharacterDeselected += CharacterDeselectedByPlayer;
    }

    protected virtual void CharacterSelectedByPlayer()
    {
        playerCamera = characterView.GetPlayerView.GetPlayerCamera.PlayerCamera;     
    }

    protected virtual void CharacterDeselectedByPlayer()
    {
        characterView.GetPlayerView.GetPlayerInput.PlayerCancelled -= CancelAction;
    }
    
    protected virtual void IntiateAction()
    {
        characterView.InitiatedCharacterAction();
    }

    public virtual void CancelAction()
    {
        if(actionState == ActionState.InProgress)
        {
            characterView.CancelledOrPerformedCharacterAction();
        }        
    }

    protected virtual void ActionCompleted()
    {
        hasPerformedAction = true;

        if (CharacterActionPerformed != null)
            CharacterActionPerformed();
    }
}
