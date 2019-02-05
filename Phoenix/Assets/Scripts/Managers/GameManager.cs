using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private MatchManager matchManager;

    public MatchManager GetMatchManager { get { return matchManager; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        matchManager = GetComponent<MatchManager>();
    }
}
