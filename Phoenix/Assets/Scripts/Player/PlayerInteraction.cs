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
    private CharacterView selectedCharacterView;
    private Transform selectedCharacterTransform;

    [Header("Selection HUD Elements")] 
    [SerializeField] private GameObject characterMoveHudObject;
    [SerializeField] private GameObject characterShootHUDObject;

    private CharacterMoveHUD characterMoveHUD;
    private CharacterShootHUD characterShootHUD;

    public CharacterMoveHUD GetCharacterMoveHUD { get { return characterMoveHUD; } }
    public CharacterShootHUD GetCharacterShootHUD { get { return characterShootHUD; } }

    private CharacterSelectionState characterSelectionState = CharacterSelectionState.Enabled;
    private enum CharacterSelectionState
    {
        Enabled,
        Disabled,
    }

    #region UnityMethods

    protected override void Start()
    {
        base.Start();

        playerCamera = playerView.GetPlayerCamera.PlayerCamera;        
      
        SetupCharacterMoveHUD();
        SetupCharacterShootHUD();
    }

    // Update is called once per frame
    void Update()
    {
        if(characterSelectionState == CharacterSelectionState.Enabled)
        {
            SelectCharacterInput();            
        }

        RemoveCharacterInput();
    }

    #endregion

    #region PlayerInteractionHUDElements   

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

    private void SelectCharacterInput()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, interactableLayer))
        {
            if (hit.collider.CompareTag("Character"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform != selectedCharacterTransform || selectedCharacterTransform == null)
                    {
                        AddCharacterToSelection(hit.transform);
                    }                       
                }
            }            
        } 
    }

    private void RemoveCharacterInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(selectedCharacterView != null)
            {
                RemoveSelectedCharacter();
            }
        }
    }

    private void AddCharacterToSelection(Transform characterTransform)
    {
        if (selectedCharacterView != null)
        {
            RemoveSelectedCharacter();
        }

        selectedCharacterTransform = characterTransform;
        selectedCharacterView = selectedCharacterTransform.GetComponent<CharacterView>();

        selectedCharacterView.CharacterActionPerformed += CharacterCompletedAction;

        selectedCharacterView.CharacterSelectedByPlayer(playerView); 
        

        if (PlayerSelectedCharacter != null)
            PlayerSelectedCharacter(selectedCharacterView);

        characterSelectionState = CharacterSelectionState.Disabled;
    }

    private void RemoveSelectedCharacter()
    { 
        selectedCharacterView.CharacterDeselectedByPlayer();

        selectedCharacterView.CharacterActionPerformed -= CharacterCompletedAction;

        selectedCharacterTransform = null;
        selectedCharacterView = null;

        if (PlayerRemovedSelectionCharacter != null)
            PlayerRemovedSelectionCharacter();

        characterSelectionState = CharacterSelectionState.Enabled;
    }   

    private void CharacterCompletedAction()
    {
        RemoveSelectedCharacter();
    }
}
