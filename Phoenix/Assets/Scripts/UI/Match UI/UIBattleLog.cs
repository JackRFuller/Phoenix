using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBattleLog : MonoBehaviour
{
    [Header("Battle Log Elements")]
    [SerializeField] private List<BattleLog> battleLog;

    [Header("Battle Log Message Icons")]
    [SerializeField] private Sprite[] battleLogIcons;
    [SerializeField] private Image battleLogBGImage;

    private int numberOfRecievedMessages;

    private void Start()
    {
        TurnOffBattleLog();
        GameManager.Instance.GetMatchManager.BattleLogMessage += UpdateBattleLog;
    }

    private void TurnOffBattleLog()
    {
        for (int battleLogEntry = 0; battleLogEntry < battleLog.Count; battleLogEntry++)
        {
            battleLog[battleLogEntry].battleLogMessageIconImage.enabled = false;
            battleLog[battleLogEntry].battleLogMessageText.enabled = false;
        }

        battleLogBGImage.enabled = false;
    }

    private void UpdateBattleLog(int messageType, string message)
    {   
        if(!battleLogBGImage.enabled)
            battleLogBGImage.enabled = true;

        for (int battleLogEntry = battleLog.Count -1; battleLogEntry > 0; battleLogEntry--)
        {
            battleLog[battleLogEntry].battleLogMessageIconImage.sprite = battleLog[battleLogEntry - 1].battleLogMessageIconImage.sprite;
            battleLog[battleLogEntry].battleLogMessageText.text = battleLog[battleLogEntry - 1].battleLogMessageText.text;
        }

        battleLog[0].battleLogMessageText.text = message;
        battleLog[0].battleLogMessageIconImage.sprite = battleLogIcons[messageType];        

        if(numberOfRecievedMessages < 5)
        {
            battleLog[numberOfRecievedMessages].battleLogMessageText.enabled = true;
            battleLog[numberOfRecievedMessages].battleLogMessageIconImage.enabled = true;
            numberOfRecievedMessages++;
        }

       
    }
}

[System.Serializable]
public class BattleLog
{
    public Image battleLogMessageIconImage;
    public TMP_Text battleLogMessageText;
}

