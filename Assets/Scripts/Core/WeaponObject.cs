using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponType", menuName = "ScriptableObjects/Weapon")]
public class WeaponObject : ScriptableObject
{
    public int damage;
    public int weaponLayer;
}