using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    [HideInInspector] public WeaponObject weaponScriptableObj;
    public int _damage;
    public bool isEnemyWeapon;

    public Action<CharacterBehavior> E_WeaponCollidedWithOppositeCharacter;

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
            E_WeaponCollidedWithOppositeCharacter?.Invoke(other.GetComponent<CharacterBehavior>());
            VfxPool.instance.PlayVfx("Blood", transform.position, Quaternion.identity);
        }
        if (other.CompareTag("Player") && isEnemyWeapon)
        {
            E_WeaponCollidedWithOppositeCharacter?.Invoke(other.GetComponent<CharacterBehavior>());
            //other.GetComponent<CharacterBehavior>().DecreaseHealth(_damage);;
        }
    }
}
