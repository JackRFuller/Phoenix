using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : CharacterComponent
{
    private Animator characterAnimator;

    protected override void Start()
    {
        base.Start();
        characterAnimator = GetComponentInChildren<Animator>();
        characterAnimator.runtimeAnimatorController = characterView.GetCharacterData.characterAnimator; 
    }
}
