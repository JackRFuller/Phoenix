using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterView : MonoBehaviour
{
    //Actions
    public event Action CharacterSelected;
    public event Action CharacterDeselected;

    public event Action CharacterActionInitiated;
    public event Action CharacterActionCancelledOrPerformed;

    //Character Components
    [SerializeField] private CharacterData characterData;
    private PhotonView photonView;
    private CharacterMovement characterMovement;
    private CharacterShooting characterShooting;
   

    private PlayerView playerView;

    public CharacterData GetCharacterData { get { return characterData; } }
    public PhotonView GetPhotonView { get { return photonView; } }
    public CharacterMovement GetCharacterMovement { get { return characterMovement; } }
    public CharacterShooting GetCharacterShooting { get { return characterShooting; } }

    public PlayerView GetPlayerView { get { return playerView; } }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    public void CharacterSelectedByPlayer(PlayerView _playerView)
    {
        playerView = _playerView;

        if(CharacterSelected != null)
            CharacterSelected();
    }

    #region Selection Events

    public void CharacterDeselectedByPlayer()
    {
        if (CharacterDeselected != null)
            CharacterDeselected();
    }   

    #endregion

    public void InitiatedCharacterAction()
    {
        if (CharacterActionInitiated != null)
            CharacterActionInitiated();
    }

    public void CancelledOrPerformedCharacterAction()
    {
        if (CharacterActionCancelledOrPerformed != null)
            CharacterActionCancelledOrPerformed();
    }
}
