﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

namespace TestRange
{
    public class TestRangeManager : Photon.MonoBehaviour
    {
        [Header("Character Spawn Elements")]
        [SerializeField] private string[] charactersToSpawn;
        [SerializeField] private Transform[] playerOnecharacterSpawnPoints;
        [SerializeField] private Transform[] playerTwocharacterSpawnPoints;

        #region UnityMethods

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings("Pre-Alpha");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        #endregion

        #region PhotonMethods

        public virtual void OnConnectedToMaster()
        {
            Debug.Log("ConnectedToMaster");
            JoinOrCreateRoom();
        }

        public virtual void JoinOrCreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom("newRoom", roomOptions, null);
        }

        public virtual void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            SpawnInPlayer();            
        }

        #endregion

        private void SpawnInPlayer()
        {
            Vector3 spawnPosition = new Vector3(-10, 7, -10);
            Quaternion spawnRotation = Quaternion.Euler(new Vector3(30, 45, 0));
            PhotonNetwork.Instantiate("Player", spawnPosition, spawnRotation, 0);
        }


    }
}


