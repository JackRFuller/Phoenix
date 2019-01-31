using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_Cam;

public class PlayerView : MonoBehaviour
{
    private PhotonView photonView;
    private PlayerInput playerInput;
    private PlayerInteraction playerInteraction;
    private PlayerUI playerUI;
    private RTS_Camera playerCamera;

    public PhotonView GetPhotonView { get { return photonView; } }
    public PlayerInput GetPlayerInput { get { return playerInput; } }
    public PlayerInteraction GetPlayerInteraction { get { return playerInteraction; } }
    public PlayerUI GetPlayerUI { get { return playerUI; } }
    public RTS_Camera GetPlayerCamera { get { return playerCamera; } }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerInput = GetComponent<PlayerInput>();
        playerCamera = GetComponent<RTS_Camera>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerUI = GetComponent<PlayerUI>();
    }
}
