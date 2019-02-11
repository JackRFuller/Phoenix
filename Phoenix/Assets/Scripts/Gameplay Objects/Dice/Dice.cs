using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private DICEState diceState = DICEState.Static;

    private PhotonView photonView;

    private Transform diceTransform;
    private Rigidbody diceRB;
    private MeshRenderer diceMesh;
    private Collider diceCollider;

    [Header("Dice Materials")]
    [SerializeField] private Material clientDiceMaterial;
    [SerializeField] private Material opponentDiceMaterial;
    
    public PhotonView GetPhotonView { get { return photonView; } }

    private enum DICEState
    {
        Rolled,
        Static,
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        diceCollider = GetComponent<Collider>();
        diceTransform = GetComponent<Transform>();
        diceRB = GetComponent<Rigidbody>();
        diceMesh = GetComponentInChildren<MeshRenderer>();

        DisableDice();
        SetupDice();
    }

    private void SetupDice()
    {        
        diceMesh.material = photonView.isMine ? clientDiceMaterial : opponentDiceMaterial;
    }

    private void FixedUpdate()
    {
        if(photonView.isMine)
        {
            if (diceState == DICEState.Rolled)
            {
                GetDiceValue();
            }
        }       
    }

    private void GetDiceValue()
    {
        if (diceRB.velocity == Vector3.zero)
        {
            int diceValue = ReturnDiceValue();
            diceState = DICEState.Static;

            MatchManager.SendBattleLogMessage(BattleLogMessage.diceRoll, $"{GameManager.Instance.GetMatchManager.LocalPlayerName} Rolled a {diceValue}");

            for(int i =0; i < GameManager.Instance.GetMatchManager.Players.Count; i++)
            {
                GameManager.Instance.GetMatchManager.Players[i].GetPhotonView.RPC("RecieveDiceRolls",PhotonTargets.All, diceValue);
            }
        }
    }

    [PunRPC]
    public void RollDice(Vector3 spawnPosition)
    {
        ShowDice();

        if(photonView.isMine)
        {
            transform.position = spawnPosition;

            EnableDice();

            Vector3 rollDirection = PhotonNetwork.isMasterClient ? -transform.right : transform.right;

            //Generate Random Speed
            diceRB.AddForce(rollDirection * Random.Range(3, 12), ForceMode.Impulse);
            diceRB.AddTorque(rollDirection * Random.Range(3, 12), ForceMode.Impulse);

            StartCoroutine(WaitForHalfASecondBeforeCheckingDICERoll());
        }
    }

    IEnumerator WaitForHalfASecondBeforeCheckingDICERoll()
    {
        yield return new WaitForSeconds(0.5f);
        diceState = DICEState.Rolled;
    }

    private int ReturnDiceValue()
    {
        int diceValue = 0;

        if (Vector3.Dot(diceTransform.forward, Vector3.up) > 0.6f)
            diceValue = 1;
        if (Vector3.Dot(-diceTransform.forward, Vector3.up) > 0.6f)
            diceValue = 6;
        if (Vector3.Dot(diceTransform.up, Vector3.up) > 0.6f)
            diceValue = 5;
        if (Vector3.Dot(-diceTransform.up, Vector3.up) > 0.6f)
            diceValue = 2;
        if (Vector3.Dot(diceTransform.right, Vector3.up) > 0.6f)
            diceValue = 4;
        if (Vector3.Dot(-diceTransform.right, Vector3.up) > 0.6f)
            diceValue = 3;

        return diceValue;
    }

    [PunRPC]
    public void ResetDice()
    {
        DisableDice();
    }

    private void EnableDice()
    {
        diceCollider.enabled = true;
        diceRB.useGravity = true;
        ShowDice();    
    }

    private void DisableDice()
    {
        diceRB.useGravity = false;
        diceRB.velocity = Vector3.zero;
        diceRB.angularVelocity = Vector3.zero;
        diceCollider.enabled = false;
        HideDice();
    }

    private void ShowDice()
    {
        diceMesh.enabled = true;
    }

    private void HideDice()
    {
        diceMesh.enabled = false;
    }
}
