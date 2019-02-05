using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootEvent : PlayerCombatEvent
{
    protected override void OnCharacterSelected(CharacterView _characterView)
    {
        base.OnCharacterSelected(_characterView);
        characterView.GetCharacterShooting.CharacterShootingAtTarget += InitiateShootingEvent;
    }

    protected override void OnCharacterDeSelected()
    {
        characterView.GetCharacterShooting.CharacterShootingAtTarget -= InitiateShootingEvent;
        base.OnCharacterDeSelected();
    }

    private void InitiateShootingEvent(Transform playerCharacter, Transform targetCharacter)
    {
        //Set all Player Cameras to Focus on Shooting Event
        //Temp - Need to Redo       
        for (int playerIndex = 0; playerIndex < GameManager.Instance.GetMatchManager.Players.Count; playerIndex++)
        {
            GameManager.Instance.GetMatchManager.Players[playerIndex].GetPhotonView.RPC("ShootingEventStarting", PhotonTargets.All, playerCharacter.position, targetCharacter.position);
        }
    }

    [PunRPC]
    private void ShootingEventStarting(Vector3 playerCharacter, Vector3 targetCharacter, PhotonMessageInfo info)
    {
        CombatBeginning();

        //If Sender is me       
        bool senderIsMe = info.sender.ID == PhotonNetwork.player.ID ? true : false;

        playerCharactersVec3.Add(Vector3.zero);
        targetCharactersVec3.Add(Vector3.zero);

        //If I am the one shooting
        playerCharactersVec3[0]= senderIsMe ? playerCharacter : targetCharacter;
        targetCharactersVec3[0] = senderIsMe ? targetCharacter : playerCharacter;

        FocusCameraOnEvent();
    }

    protected override void FocusCameraOnEvent()
    {
        //Position Camera To Focus On Event
        Vector3 playerCharacter = playerCharactersVec3[0];
        Vector3 targetCharacter = targetCharactersVec3[0];

        Vector3 midPoint = (playerCharacter + targetCharacter) * 0.5f;

        Vector3 newCameraPosition = midPoint;

        newCameraPosition.x = playerCharacter.x <= targetCharacter.x ? -10 : +10;
        newCameraPosition.z = playerCharacter.z <= targetCharacter.z ? -10 : +10;
        newCameraPosition.y = 7;
        transform.position = newCameraPosition;

        midPoint = new Vector3(midPoint.x, this.transform.position.y, midPoint.z);
        transform.LookAt(midPoint);
        transform.eulerAngles = new Vector3(30, transform.eulerAngles.y, 0);

        Debug.Log(gameObject.name + ": Position Camera");
    }

}
