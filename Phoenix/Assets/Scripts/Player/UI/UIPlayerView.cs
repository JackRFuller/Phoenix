using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerView : MonoBehaviour
{
    private PlayerView playerView;
    private UICharacterActions uICharacterActions;
   
    public PlayerView GetPlayerView { get { return playerView; } }
    public UICharacterActions GetCharacterActionsUI { get { return uICharacterActions; } }

    public void SetupPlayerUI(PlayerView _playerView)
    {
        playerView = _playerView;

        uICharacterActions = GetComponentInChildren<UICharacterActions>();
    }
}
