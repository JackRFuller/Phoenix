using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMatchMessage : MonoBehaviour
{
    private TMP_Text matchMessageText;
    private Animator matchMessageAnim;


    private void Start()
    {
        matchMessageAnim = GetComponent<Animator>();
        matchMessageText = GetComponentInChildren<TMP_Text>();

        GameManager.Instance.GetMatchManager.MatchMessage += TriggerMatchMessage;
    }

    private void TriggerMatchMessage(string matchMessage)
    {
        matchMessageText.text = matchMessage;
        matchMessageAnim.SetTrigger("MatchMessage");
    }
}
