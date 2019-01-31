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

    [Header("Selection HUD")]
    [SerializeField] private GameObject selectionHUDObject;

    #region UnityMethods

    protected override void Start()
    {
        base.Start();

        playerCamera = playerView.GetPlayerCamera.PlayerCamera;
        SetupSelectionRing();
    }

    // Update is called once per frame
    void Update()
    {
        SendOutRayToFindInteractableObject();

        RemoveSelection();
    }

    #endregion

    private void SetupSelectionRing()
    {
        GameObject selectionRing = Instantiate(selectionHUDObject);
        selectionRing.GetComponent<SelectionIcon>().SetupRing(this);
    }

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
