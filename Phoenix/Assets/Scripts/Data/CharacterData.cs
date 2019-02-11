using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data",menuName = "Data/Character Data", order = 1)]
public class CharacterData : ScriptableObject
{
    public string characterName;

    [Header("Health")]
    [Range(1, 10)]
    public int hitPoints;

    [Header("Movement Attributes")]
    public float maxMovementDistance;

    [Header("Combat Attributes")]
    [Range(1,5)]
    public int handToHandSkill;
    [Range(1, 5)]
    public int shootingSkill;
    [Range(1, 10)]
    public int strength;
    [Range(1, 10)]
    public int defense;

    [Header("Weapons")]
    public RangedWeaponData rangedWeapon;

    [Header("Animation")]
    public RuntimeAnimatorController characterAnimator;
}
