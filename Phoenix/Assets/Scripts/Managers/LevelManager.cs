using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Manager
{
    private Level levelData;
    public Level GetLevelData { get { return levelData; } }


    public void RecieveLevelData(Level _levelData)
    {
        levelData = _levelData;
    }
}
