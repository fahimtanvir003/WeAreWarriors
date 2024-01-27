using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    [HideInInspector] public WeaponObject weaponScriptableObj;
    private int _damage;
    public Action E_WeaponCollidedWithEnemy;

    private void Start()
    {
        Initialize();
    }

    public void SetWeaponLayer(int _layer)
    {
        gameObject.layer = _layer;
    }
    void Initialize()
    {
        _damage = weaponScriptableObj.damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            E_WeaponCollidedWithEnemy?.Invoke();
            other.GetComponent<CharacterBehavior>().DecreaseHealth(_damage);
            Debug.Log("HIT");
        }
    }
}
