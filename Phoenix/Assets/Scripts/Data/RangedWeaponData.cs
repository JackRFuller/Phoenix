using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon Data",menuName = "Data/Weapon/Ranged Weapon", order = 1)]
public class RangedWeaponData : WeaponData
{
    [Header("Ranged Weapon Attributes")]
    public float weaponRange;
    public float numberOfWeaponProjectiles; 
}
