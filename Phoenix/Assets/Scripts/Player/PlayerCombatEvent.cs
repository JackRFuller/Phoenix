using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombatEvent : PlayerComponent
{
    public event Action CombatEventBeginning;
    public event Action CombatEventEnded;

    protected CharacterView characterView;

    protected List<Vector3> playerCharactersVec3;
    protected List<Vector3> targetCharactersVec3;

    protected override void Start()
    {
        base.Start();

        playerView.GetPlayerInteraction.PlayerSelectedCharacter += OnCharacterSelected;
        playerView.GetPlayerInteraction.PlayerRemovedSelectionCharacter += OnCharacterDeSelected;

        playerCharactersVec3 = new List<Vector3>();
        targetCharactersVec3 = new List<Vector3>();
    }

    #region SelectionEvents

    protected virtual void OnCharacterSelected(CharacterView _characterView)
    {
        characterView = _characterView;
    }

    protected virtual void OnCharacterDeSelected()
    {
        characterView = null;
    }

    #endregion

    protected virtual void FocusCameraOnEvent()
    {
        
    }

    protected void CombatBeginning()
    {
        if (CombatEventBeginning != null)
            CombatEventBeginning();

    }
}
