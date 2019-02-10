using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootEvent : PlayerCombatEvent
{
    private CharacterView localCharacter;
    private CharacterView opponentCharacter;

    private string localCharacterName;
    private string opponentCharacterName;

    private ShootingPhase shootingPhase;
    private enum ShootingPhase
    {
        ShootingToHit,
        ShootingToWound,
    }

    protected override void OnCharacterSelected(CharacterView _characterView)
    {
        base.OnCharacterSelected(_characterView);
        characterView.GetCharacterShooting.CharacterShootingAtTarget += StartShooingEvent;
    }

    protected override void OnCharacterDeSelected()
    {
        characterView.GetCharacterShooting.CharacterShootingAtTarget -= StartShooingEvent;
        base.OnCharacterDeSelected();
    }

    private void StartShooingEvent(CharacterView _localCharacter, CharacterView _opponentCHaracter)
    {
        localCharacter = _localCharacter;
        opponentCharacter = _opponentCHaracter;

        //Signal Events
        localCharacterName = localCharacter.GetCharacterData.characterName;
        opponentCharacterName = opponentCharacter.GetCharacterData.characterName;

        GameManager.Instance.GetMatchManager.TriggerMatchMessageForBothPlayers($"{localCharacterName} Rolling to Hit {opponentCharacterName}");
                
        //Setup Event
        playerView.GetPlayerDiceRoller.DiceRolled += GetDiceResults;

        //Setup Dice
        playerView.GetPlayerDiceRoller.SetupDiceRoll(1,0);
    }  

    private void GetDiceResults(List<int> playerDiceRolls, List<int> opponentDiceRolls)
    {
        if(shootingPhase == ShootingPhase.ShootingToHit)
        {
            ShootingHitPhase(playerDiceRolls);
        }
        else if(shootingPhase == ShootingPhase.ShootingToWound)
        {
            if(DidCharacterCauseAWound(playerDiceRolls))
            {

            }
            else
            {

            }
        }
    }  

    private void ShootingHitPhase(List<int> playerDiceRolls)
    {
        //Calculate if we hit
        if (DidCharacterHit(playerDiceRolls))
        {
            SetupShootingToWoundPhase();
        }
        else
        {
            GameManager.Instance.GetMatchManager.TriggerMatchMessageForBothPlayers($"{localCharacterName} Failed To Hit");
        }
    }

    private void SetupShootingToWoundPhase()
    {
        int diceRollNeeded = WoundChart.ValueNeededToCauseAWound(opponentCharacter.GetCharacterData.defense, localCharacter.GetCharacterData.strength);
        GameManager.Instance.GetMatchManager.TriggerMatchMessageForBothPlayers($"HIT! {diceRollNeeded}+ Needed to Wound");

        shootingPhase = ShootingPhase.ShootingToWound;
        playerView.GetPlayerDiceRoller.SetupDiceRoll(1, 0);
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

    #region Dice Roll Calculations

    private bool DidCharacterHit(List<int> playerDiceRolls)
    {
        if(playerDiceRolls[0] >= 6 - localCharacter.GetCharacterData.shootingSkill)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool DidCharacterCauseAWound(List<int> playerDiceRolls)
    {
        int valueNeededToWound = WoundChart.ValueNeededToCauseAWound(opponentCharacter.GetCharacterData.defense, localCharacter.GetCharacterData.strength);

        if (playerDiceRolls[0] >= valueNeededToWound)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

}
