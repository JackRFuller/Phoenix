﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterView : MonoBehaviour
{
    //Actions
    public event Action CharacterSelected;
    public event Action CharacterDeselected;
    
    public event Action CharacterActionPerformed;

    //Character Components
    [SerializeField] private CharacterData characterData;
    private PhotonView photonView;
    private CharacterMovement characterMovement;
    private CharacterShooting characterShooting;
    private CharacterHealth characterHealth;
    private CharacterAnimation characterAnimation;

    private PlayerView playerView;

    public CharacterData GetCharacterData { get { return characterData; } }
    public PhotonView GetPhotonView { get { return photonView; } }
    public CharacterMovement GetCharacterMovement { get { return characterMovement; } }
    public CharacterShooting GetCharacterShooting { get { return characterShooting; } }
    public CharacterHealth GetCharacterHealth { get { return characterHealth; } }
    public CharacterAnimation GetCharacterAnimation { get { return characterAnimation; } }

    public PlayerView GetPlayerView { get { return playerView; } }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        characterMovement = gameObject.AddComponent<CharacterMovement>();
        characterShooting = gameObject.AddComponent<CharacterShooting>();
        characterHealth = gameObject.AddComponent<CharacterHealth>();
        characterAnimation = gameObject.AddComponent<CharacterAnimation>();
        
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

    public void CharacterActionPerformedByPlayer()
    {
        if (CharacterActionPerformed != null)
            CharacterActionPerformed();
    }
}
