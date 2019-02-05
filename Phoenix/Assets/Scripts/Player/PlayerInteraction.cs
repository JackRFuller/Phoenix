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
    [SerializeField] private GameObject characterShootHUDObject;

    private CharacterMoveHUD characterMoveHUD;
    private CharacterShootHUD characterShootHUD;

    public CharacterMoveHUD GetCharacterMoveHUD { get { return characterMoveHUD; } }
    public CharacterShootHUD GetCharacterShootHUD { get { return characterShootHUD; } }

    private PlayerInteractionState playerInteractionState = PlayerInteractionState.Enabled;
    private enum PlayerInteractionState
    {
        Enabled,
        Disabled,
    }

    #region UnityMethods

    protected override void Start()
    {
        base.Start();

        playerCamera = playerView.GetPlayerCamera.PlayerCamera;
        playerView.GetPlayerInput.PlayerCancelled += RemoveSelection;

        SetupSelectionRing();
        SetupCharacterMoveHUD();
        SetupCharacterShootHUD();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInteractionState == PlayerInteractionState.Enabled)
        {
            SendOutRayToFindInteractableObject();           
        }       
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

    private void SetupCharacterShootHUD()
    {
        GameObject characterShoot = Instantiate(characterShootHUDObject);
        characterShootHUD = characterShoot.GetComponent<CharacterShootHUD>();
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
                        AddCharacterToSelection(hit.transform);
                    }
                       
                }
            }            
        } 
    }

    private void AddCharacterToSelection(Transform characterTransform)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.CharacterActionInitiated -= DisablePlayerInteraction;
            selectedCharacter.CharacterActionCancelledOrPerformed -= EnablePlayerInteraction;

            selectedCharacter.CharacterDeselectedByPlayer();
        }

        selectedCharacterTransform = characterTransform;
        selectedCharacter = selectedCharacterTransform.GetComponent<CharacterView>();

        selectedCharacter.CharacterSelectedByPlayer(playerView);

        selectedCharacter.CharacterActionInitiated += DisablePlayerInteraction;
        selectedCharacter.CharacterActionCancelledOrPerformed += EnablePlayerInteraction;

        if (PlayerSelectedCharacter != null)
            PlayerSelectedCharacter(selectedCharacter);        
    }

    private void RemoveSelection()
    {        
        if (selectedCharacterTransform != null)
        {
            selectedCharacter.CharacterDeselectedByPlayer();
            selectedCharacterTransform = null;
            selectedCharacter = null;

            if (PlayerRemovedSelectionCharacter != null)
                PlayerRemovedSelectionCharacter();

            Debug.Log("Removed");
        }             
    }

    private void DisablePlayerInteraction()
    {
        playerInteractionState = PlayerInteractionState.Disabled;
        playerView.GetPlayerInput.PlayerCancelled -= RemoveSelection;
    }

    private void EnablePlayerInteraction()
    {
        playerInteractionState = PlayerInteractionState.Enabled;
        playerView.GetPlayerInput.PlayerCancelled += RemoveSelection;        
    }
}
