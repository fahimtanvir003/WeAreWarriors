using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    [HideInInspector] public WeaponObject weaponScriptableObj;
    private int _damage;
    public bool isEnemyWeapon;

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
        if (other.CompareTag("Enemy") && !isEnemyWeapon)
        {
            E_WeaponCollidedWithEnemy?.Invoke();
            other.GetComponent<CharacterBehavior>().DecreaseHealth(_damage);
            Debug.Log("HIT");
        }
        if (other.CompareTag("Player") && isEnemyWeapon)
        {
            E_WeaponCollidedWithEnemy?.Invoke();
            other.GetComponent<CharacterBehavior>().DecreaseHealth(_damage);
            Debug.Log("HIT of Enemy");
        }
    }
}
