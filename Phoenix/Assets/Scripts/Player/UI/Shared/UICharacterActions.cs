using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UICharacterActions : UIPlayerComponent
{
    public event Action PlayerInitiatedCharacterMove;

    [Header("Character Action Elements")]
    [SerializeField] private GameObject moveActionObj;
    [SerializeField] private GameObject shootActionObj;

    protected override void Start()
    {
        base.Start();

        HideCharacterActionButtons();

        uiView.GetPlayerView.GetPlayerInteraction.PlayerSelectedCharacter += CharacterSelected;
        uiView.GetPlayerView.GetPlayerInteraction.PlayerRemovedSelectionCharacter += CharacterDeSelected;
    }

    private void CharacterSelected(CharacterView characterView)
    {
        ShowCharacterActionButtons();
    }

    private void CharacterDeSelected()
    {
        HideCharacterActionButtons();
    }

    private void ShowCharacterActionButtons()
    {
        moveActionObj.SetActive(true);
        shootActionObj.SetActive(true);
    }

    private void HideCharacterActionButtons()
    {
        moveActionObj.SetActive(false);
        shootActionObj.SetActive(false);
    }

    #region ButtonMethods

    public void OnButtonClickMoveCharacter()
    {
        Debug.Log("Intiate Character Move");

        if (PlayerInitiatedCharacterMove != null)
            PlayerInitiatedCharacterMove();
    }

    public void OnButtonClickShootCharacter()
    {
        Debug.Log("Intiate Character Shoot");
    }

    #endregion
}
