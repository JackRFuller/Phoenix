using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHandler : MonoBehaviour
{
    [Header("Lobby Elements")]
    [SerializeField] private GameObject playerNameInputObj;
    [SerializeField] private GameObject playerConnectButtonObj;
    [SerializeField] private GameObject lobbyCameraObj;
    [SerializeField] private GameObject lobbyWaitingForPlayersObj;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GetLobbyManager.ClientConnectedToRoom += HideLobbyCanvas;
        GameManager.Instance.GetLobbyManager.ClientConnectedToEmptyRoom += ShowWaitingForPlayersMessage;

        GameManager.Instance.GetMatchManager.MatchSetup += TurnOffLobbyCamera;
        GameManager.Instance.GetMatchManager.MatchSetup += HideWaitingForPlayersMessage;

        HideWaitingForPlayersMessage();
    }

    private void HideLobbyCanvas()
    {
        playerNameInputObj.SetActive(false);
        playerConnectButtonObj.SetActive(false);
    }

    private void TurnOffLobbyCamera()
    {
        lobbyCameraObj.SetActive(false);
    }

    private void ShowWaitingForPlayersMessage()
    {
        lobbyWaitingForPlayersObj.SetActive(true);
    }

    private void HideWaitingForPlayersMessage()
    {
        lobbyWaitingForPlayersObj.SetActive(false);
    }
    
}
