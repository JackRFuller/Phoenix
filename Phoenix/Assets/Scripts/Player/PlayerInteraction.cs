using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInteraction : PlayerComponent
{
    public event Action<CharacterView> PlayerSelectedCharacter;
    public event Action PlayerRemovedSelectionCharacter;

    [SerializeField] private LayerMask interactableLayer;

    private Camera playerCamera;
    private CharacterView selectedCharacter;
    private Transform selectedCharacterTransform;

    [Header("Selection HUD Elements")]
    [SerializeField] private GameObject selectionHUDObject;
    [SerializeField] private GameObject characterMoveHudObject;
    private CharacterMoveHUD characterMoveHUD;

    public CharacterMoveHUD GetCharacterMoveHUD { get { return characterMoveHUD; } }

    #region UnityMethods

    protected override void Start()
    {
        base.Start();

        playerCamera = playerView.GetPlayerCamera.PlayerCamera;

        SetupSelectionRing();
        SetupCharacterMoveHUD();
    }

    // Update is called once per frame
    void Update()
    {
        SendOutRayToFindInteractableObject();

        RemoveSelection();
    }

    #endregion

    #region PlayerInteractionHUDElements

    private void SetupSelectionRing()
    {
        GameObject selectionRing = Instantiate(selectionHUDObject);
        selectionRing.GetComponent<SelectionIcon>().SetupRing(this);
    }

    private void SetupCharacterMoveHUD()
    {
        GameObject characterMove = Instantiate(characterMoveHudObject);
        characterMoveHUD = characterMove.GetComponent<CharacterMoveHUD>();        
    }

    #endregion

    private void SendOutRayToFindInteractableObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, interactableLayer))
        {
            if (hit.collider.CompareTag("Character"))
            {
                if (playerView.GetPlayerInput.SelectInput)
                {
                    if (hit.transform != selectedCharacterTransform || selectedCharacterTransform == null)
                    {
                        if(selectedCharacter != null)
                            selectedCharacter.CharacterDeselectedByPlayer();

                        selectedCharacterTransform = hit.transform;
                        selectedCharacter = selectedCharacterTransform.GetComponent<CharacterView>();

                        selectedCharacter.CharacterSelectedByPlayer(playerView);

                        if (PlayerSelectedCharacter != null)
                            PlayerSelectedCharacter(selectedCharacter);
                    }
                    
                }
            }            
        } 
    }

    private void RemoveSelection()
    {
        if(playerView.GetPlayerInput.DeselectInput)
        {
            if (selectedCharacterTransform != null)
            {
                selectedCharacter.CharacterDeselectedByPlayer();
                selectedCharacterTransform = null;
                selectedCharacter = null;

                if (PlayerRemovedSelectionCharacter != null)
                    PlayerRemovedSelectionCharacter();
            }
        }        
    }
}
