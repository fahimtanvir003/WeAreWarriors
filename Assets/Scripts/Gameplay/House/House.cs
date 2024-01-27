using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private float houseHealth;
    [SerializeField] private HouseParty houseRole;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") )
        {
            if(houseHealth <= 0)
            {
                //Destroy house
            }
            Debug.Log("Damagee");
        }
    }
}

public enum HouseParty
{
    Player,
    Enemy
};
