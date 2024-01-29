using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class House : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;

    [SerializeField] private float _houseHealth;
    [SerializeField] private HouseParty houseRole;

    public Action E_HouseDestroyed;

    private void Start()
    {
        _healthSlider.maxValue = _houseHealth;
        _healthSlider.value = _houseHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (houseRole == HouseParty.Player)
        {
            if (other.CompareTag("EnemyWeapon"))
            {
                if (_houseHealth > 0)
                {
                    _houseHealth--;
                    _healthSlider.value = _houseHealth;
                }
                else
                {
                    E_HouseDestroyed?.Invoke();
                    Destroy(transform.parent.gameObject);
                }
            }
        }
        else
        {
            if (other.CompareTag("PlayerWeapon"))
            {
                if (_houseHealth > 0)
                {
                    _houseHealth--;
                    _healthSlider.value = _houseHealth;
                }
                else
                {
                    E_HouseDestroyed?.Invoke();
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
}

public enum HouseParty
{
    Player,
    Enemy
};
