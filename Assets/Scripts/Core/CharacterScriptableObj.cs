using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObj", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObj : ScriptableObject
{
    public WeaponObject weaponScriptableObj;
    public string targetHouseName = "EnemyHouse";
    public string characterTag = "Player";
    public bool isEnemy;
    public GameObject character;
    public Material[] characterMaterials;
    public int health;
    public int damage;
    public float speed;
    public float stoppingDistance = 2f;
}
