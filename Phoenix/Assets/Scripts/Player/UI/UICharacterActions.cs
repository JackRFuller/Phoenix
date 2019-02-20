using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UICharacterActions : UIPlayerComponent
{   
    public event Action PlayerInitiatedCharacterMove;
    public event Action PlayerInitiatedCharacterShoot;

    private CharacterView characterView;

    [Header("Character Action Elements")]
    [SerializeField] private GameObject moveActionObj;
    [SerializeField] private GameObject shootActionObj;

    [Header("Character Move Action Elements")]
    [SerializeField] private Button moveActionButton;
    [SerializeField] private Image moveActionImage;

    [Header("Character Shoot Action Elements")]
    [SerializeField] private Button shootActionButtons;
    [SerializeField] private Image shootActionImage;

    #region UnityMethods

    protected override void Start()
    {
        base.Start();

        HideCharacterActionButtons();

        //uiView.GetPlayerView.GetPlayerInteraction.PlayerSelectedCharacter += CharacterSelected;
        //uiView.GetPlayerView.GetPlayerInteraction.PlayerRemovedSelectionCharacter += CharacterDeSelected;

        ////Subscribe to Event To Hide and Show UI During Combat
        //uiView.GetPlayerView.GetPlayerShootEvent.CombatEventBeginning += HideCharacterActionButtons;
        //uiView.GetPlayerView.GetPlayerShootEvent.CombatEventEnded += ShowCharacterActionButtons;
    }

    #endregion

    private void CharacterSelected(CharacterView _characterView)
    {
        characterView = _characterView;

        if(characterView.GetPhotonView.isMine)
        {
            //characterView.GetCharacterMovement.CharacterActionPerformed += UpdateCharacterActionButtons;
            //characterView.GetCharacterShooting.CharacterActionPerformed += UpdateCharacterActionButtons;

            ShowCharacterActionButtons();
            UpdateCharacterActionButtons();
        }
        else
        {
            HideCharacterActionButtons();
        }
    }

    private void CharacterDeSelected()
    {
        //characterView.CharacterActionCancelledOrPerformed -= UpdateCharacterActionButtons;
        //characterView.GetCharacterShooting.CharacterActionPerformed -= UpdateCharacterActionButtons;

        HideCharacterActionButtons();
    }

    private void UpdateCharacterActionButtons()
    {
        UpdateCharacterMovementActionButtons();
        UpdateCharacterShootingActionButtons();
    }
    
    private void UpdateCharacterMovementActionButtons()
    {
        if(characterView.GetCharacterMovement.HasPerformedAction || !TurnManager.IsPlayersTurn())
        {
            moveActionButton.enabled = false;
            moveActionImage.color = Color.grey;
        }
        else
        {
            moveActionButton.enabled = true;
            moveActionImage.color = Color.black;
        }
    }

    private void UpdateCharacterShootingActionButtons()
    {
        if (characterView.GetCharacterShooting.HasPerformedAction || !TurnManager.IsPlayersTurn())
        {
            shootActionButtons.enabled = false;
            shootActionImage.color = Color.grey;
        }
        else
        {
            shootActionButtons.enabled = true;
            shootActionImage.color = Color.black;
        }
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
        if (PlayerInitiatedCharacterMove != null)
            PlayerInitiatedCharacterMove();
    }

    public void OnButtonClickShootCharacter()
    {       
        characterView.GetCharacterMovement.CancelAction();

        if (PlayerInitiatedCharacterShoot != null)
            PlayerInitiatedCharacterShoot();
    }

    #endregion
}
