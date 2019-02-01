using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterView : MonoBehaviour
{
    //Actions
    public event Action CharacterSelected;
    public event Action CharacterDeselected;    

    //Character Components
    [SerializeField] private CharacterData characterData;
    private PhotonView photonView;
   

    private PlayerView playerView;

    public CharacterData GetCharacterData { get { return characterData; } }
    public PhotonView GetPhotonView { get { return photonView; } }

    public PlayerView GetPlayerView { get { return playerView; } }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void CharacterSelectedByPlayer(PlayerView _playerView)
    {
        playerView = _playerView;

        if(CharacterSelected != null)
            CharacterSelected();
    }

    public void CharacterDeselectedByPlayer()
    {
        if (CharacterDeselected != null)
            CharacterDeselected();
    }
}
