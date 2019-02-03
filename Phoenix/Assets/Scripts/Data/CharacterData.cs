using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data",menuName = "Data/Character Data", order = 1)]
public class CharacterData : ScriptableObject
{
    public string characterName;

    [Header("Movement Attributes")]
    public float maxMovementDistance;

    [Header("Weapons")]
    public RangedWeaponData rangedWeapon;
}
