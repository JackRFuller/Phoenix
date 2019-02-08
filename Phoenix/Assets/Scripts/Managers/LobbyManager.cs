using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using TMPro;
using System;


public class LobbyManager : Manager
{
    public event Action ClientConnectedToRoom;
    public event Action ClientConnectedToEmptyRoom;
    public event Action OpponentConnectedToRoom;
    public event Action RoomFull;

    [SerializeField] private int requiredNumberOfPlayers = 2;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        PhotonNetwork.ConnectUsingSettings("Pre-Alpha");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToMaster");
    }

    public void OnButtonClickJoinRoomOrCreateRoom()
    {
        JoinOrCreateRoom();
    }

    public void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("newRoom", roomOptions, null);        
    }

    public void OnJoinedRoom()
    {   
        if (ClientConnectedToRoom != null)
            ClientConnectedToRoom();

        if (ReturnIfRoomIsFullWithPlayers())
            RoomFull();
        else
        {
            if (ClientConnectedToEmptyRoom != null)
                ClientConnectedToEmptyRoom();
        }
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Second player connected");
        if (OpponentConnectedToRoom != null)
            OpponentConnectedToRoom();

        if (ReturnIfRoomIsFullWithPlayers())
            RoomFull();
    }

    public void SetPlayerName(TMP_Text playerNameText)
    {       
        PhotonNetwork.player.NickName = playerNameText.text;
    }

    private bool ReturnIfRoomIsFullWithPlayers()
    {
        if (PhotonNetwork.playerList.Length == requiredNumberOfPlayers)
            return true;
        else
            return false;
    }
}
