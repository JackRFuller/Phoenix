using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : CharacterComponent
{
    [Header("Character Base Elements")]
    [SerializeField] private Material playerOwnedMaterial;
    [SerializeField] private Material opponentOwnedMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private MeshRenderer characterBase;

    private Material defaultMaterial;

    protected override void Start()
    {
        base.Start();

        SetCharacterBase();

        characterView.CharacterSelected += CharacterSelected;
        characterView.CharacterDeselected += CharacterDeSelected;
    }

    private void SetCharacterBase()
    {
        characterBase.material = characterView.GetPhotonView.isMine ? playerOwnedMaterial : opponentOwnedMaterial;
        defaultMaterial = characterBase.material;        
    }

    private void CharacterSelected()
    {
        characterBase.material = selectedMaterial;
    }

    private void CharacterDeSelected()
    {
        characterBase.material = defaultMaterial;
    }
}
