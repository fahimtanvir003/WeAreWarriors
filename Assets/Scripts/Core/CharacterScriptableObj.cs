using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObj", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObj : ScriptableObject
{
    public GameObject character;
    public Material[] characterMat;
    public int health;
}
