using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    
    public CharacterScriptableObj character;

    private Transform _trans;
    public List<Transform> currentDetectedEnemiesList;

    private float _closestTargetDist;
    public Transform NearestEnemy;

    public Action E_EnemyDetected;
    private float _minimumDist = 5000;

    private void Start()
    {
        _trans = transform;
    }


    public Transform DetectedNearestEnemy()
    {
        foreach (var enemy in currentDetectedEnemiesList)
        {
            _closestTargetDist = Vector3.Distance(_trans.position, enemy.position);

            if (_closestTargetDist < _minimumDist)
            {
                _minimumDist = _closestTargetDist;
                NearestEnemy = enemy;
            }
        }
        _minimumDist = 5000;
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
            }
        }
    }

}
