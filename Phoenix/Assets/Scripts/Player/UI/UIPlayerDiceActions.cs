using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerDiceActions : UIPlayerComponent
{
    [Header("Roll DICE UI Elements")]
    [SerializeField] private GameObject diceButtonObj;
    [SerializeField] private Button diceButton;
   
    [Header("DICE Instruction Elements")]
    [SerializeField] private GameObject diceInstructionsObj;
    [SerializeField] private TMP_Text diceInstructionText;

    protected override void Start()
    {
        base.Start();

        SetDiceButtonDelegate();
        HideAllUIElements();

        //Subscribe to Event To Hide and Show UI During Combat
        uiView.GetPlayerView.GetPlayerDiceRoller.DiceEventSetup += EnableAllUIElements;
        uiView.GetPlayerView.GetPlayerDiceRoller.PlayerRolledDice += HideAllUIElements;
    } 
   
    private void SetDiceButtonDelegate()
    {        
        diceButton.onClick.AddListener(delegate { uiView.GetPlayerView.GetPlayerDiceRoller.RollDice(); });
    }

    private void EnableAllUIElements()
    {
        diceButtonObj.SetActive(true);
        diceInstructionsObj.SetActive(true);
    }

    private void HideAllUIElements()
    {
        diceButtonObj.SetActive(false);
        diceInstructionsObj.SetActive(false);
    }
}
