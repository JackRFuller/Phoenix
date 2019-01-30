using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PhotonView photonView;

    public PhotonView GetPhotonView { get { return photonView; } }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
}
