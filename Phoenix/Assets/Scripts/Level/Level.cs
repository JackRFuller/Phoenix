using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    [Header("Team One")]
    public Transform playerOneSpawnPoint;
    public Transform[] teamOneSpawnPoints;

    [Header("Team Two")]
    public Transform playerTwoSpawnPoint;
    public Transform[] teamTwoSpawnPoints;

    private void Start()
    {
        GameManager.Instance.GetLevelManager.RecieveLevelData(this);
    }
}
