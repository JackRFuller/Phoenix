using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHandler : MonoBehaviour
{
    [Header("Lobby Elements")]
    [SerializeField] private GameObject lobbyCanvasObj;
    [SerializeField] private GameObject lobbyCameraObj;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GetLobbyManager.ClientConnectedToRoom += HideLobbyCanvas;
        GameManager.Instance.GetMatchManager.MatchSetup += TurnOffLobbyCamera;
    }

    private void HideLobbyCanvas()
    {
        lobbyCanvasObj.SetActive(false);
    }

    private void TurnOffLobbyCamera()
    {
        lobbyCameraObj.SetActive(false);
    }

    
}
