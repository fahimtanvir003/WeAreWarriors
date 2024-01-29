using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    
    public CharacterScriptableObj character;

    private Transform _trans;
    public List<Transform> currentDetectedEnemiesList;

    private float closestTargetDist;
    public Transform NearestEnemy;

    public Action E_EnemyDetected;
    private float minimumDist = 5000;

    private void Start()
    {
        _trans = transform;
    }

    private void FixedUpdate()
    {
        
    }

    public Transform DetectedNearestEnemy()
    {
        foreach (var enemy in currentDetectedEnemiesList)
        {
            closestTargetDist = Vector3.Distance(_trans.position, enemy.position);

            if (closestTargetDist < minimumDist)
            {
                minimumDist = closestTargetDist;
                NearestEnemy = enemy;
            }
        }
        minimumDist = 5000;
        return NearestEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!character.isEnemy)
        {
            if (other.CompareTag("Enemy"))
            {
                if (!currentDetectedEnemiesList.Contains(other.transform))
                {
                    currentDetectedEnemiesList.Add(other.transform);
                }
                E_EnemyDetected?.Invoke();

                Debug.Log("Enemy Detected");
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                if (!currentDetectedEnemiesList.Contains(other.transform))
                {
                    currentDetectedEnemiesList.Add(other.transform);
                }
                E_EnemyDetected?.Invoke();

                Debug.Log("Player Detected");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //NearestEnemy = null;
        //currentDetectedEnemiesList.Clear();
    }
}
