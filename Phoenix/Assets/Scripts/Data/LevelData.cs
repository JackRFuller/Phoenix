using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data",menuName = "Data/Level",order = 2)]
public class LevelData : ScriptableObject
{
    [Header("Team One")]
    public Transform playerOneSpawnPoint;
    public Transform[] teamOneSpawnPoints;

    [Header("Team Two")]
    public Transform playerTwoSpawnPoint;
    public Transform[] teamTwoSpawnPoints;
}
