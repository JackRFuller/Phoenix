using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIcon : MonoBehaviour
{
    private SpriteRenderer selectionSprite;

    private void Start()
    {
        selectionSprite = GetComponentInChildren<SpriteRenderer>();
        selectionSprite.enabled = false;
    }

    public void SetupRing(PlayerInteraction playerInteraction)
    {
        playerInteraction.PlayerSelectedCharacter += SetToCharacter;
        playerInteraction.PlayerRemovedSelectionCharacter += HideSelectionRing;
    }

    private void SetToCharacter(CharacterView character)
    {
        Vector3 newPosition = character.transform.position;
        transform.position = newPosition;
        selectionSprite.enabled = true;
    }

    private void HideSelectionRing()
    {
        selectionSprite.enabled = false;
    }
}
