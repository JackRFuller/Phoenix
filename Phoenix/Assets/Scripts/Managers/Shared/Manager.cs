using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class Manager : Photon.MonoBehaviour
{
    protected GameManager gameManager;

    protected virtual void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    
}
