using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterView : MonoBehaviour
{
    public event Action CharacterSelected;
    public event Action CharacterDeselected;

    private PlayerView playerView;

    //Character Components

    public PlayerView GetPlayerView { get { return playerView; } }

    private void Awake()
    {
       
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
