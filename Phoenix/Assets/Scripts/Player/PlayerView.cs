using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_Cam;

public class PlayerView : MonoBehaviour
{
    private PhotonView photonView;  
    private PlayerInteraction playerInteraction;
    private PlayerDiceRoller playerDiceRoller;
    private PlayerShootEvent playerShootEvent;
    private PlayerUI playerUI;
    private RTS_Camera playerCamera;

    public PhotonView GetPhotonView { get { return photonView; } }  
    public PlayerInteraction GetPlayerInteraction { get { return playerInteraction; } }
    public PlayerShootEvent GetPlayerShootEvent { get { return playerShootEvent; } }
    public PlayerDiceRoller GetPlayerDiceRoller { get { return playerDiceRoller; } }
    public PlayerUI GetPlayerUI { get { return playerUI; } }
    public RTS_Camera GetPlayerCamera { get { return playerCamera; } }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();      
        playerCamera = GetComponent<RTS_Camera>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerDiceRoller = GetComponent<PlayerDiceRoller>();
        playerShootEvent = GetComponent<PlayerShootEvent>();
        playerUI = GetComponent<PlayerUI>();       
    }

    private void Start()
    {
        GameManager.Instance.GetMatchManager.RecievePlayers(this);
    }
}
