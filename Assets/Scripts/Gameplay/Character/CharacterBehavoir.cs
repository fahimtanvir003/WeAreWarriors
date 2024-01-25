using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavoir : MonoBehaviour
{
    [Header("---Components---")]
    [Space(5f)]

    [SerializeField] private CharacterScriptableObj characterScriptableObj;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRend;

    private Transform _trans;
    private Transform target;
    private CharacterState currentState;
    private float _speed;
    private int _health;

    private Vector3 defaultPos;

    private void Awake()
    {
        _trans = transform;

        if (!characterScriptableObj.isEnemy)
        {
            target = GameObject.Find("EnemyHouse").transform;
        }
        else
        {
            target = GameObject.Find("PlayerHouse").transform;
        }

        defaultPos = _trans.position;
    }
    
    void Start()
    {
        _speed = characterScriptableObj.speed;
        _health = characterScriptableObj.health;
        skinnedMeshRend.materials = characterScriptableObj.characterMaterials;
    }


    void Update()
    {
        if (_health <= 0)
        {
            Die();
        }

    }
    private void FixedUpdate()
    {
        if (currentState != CharacterState.Fighting)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        if (!characterScriptableObj.isEnemy)
        {
            if (rb.position.x <= (target.position.x - 4.85f))
            {
                MoveToTarget();
            }
        }
        else
        {
            if (rb.position.x >= (target.position.x + 4.85f))
            {
                MoveToTarget();
            }
        }
    }

    private void Die()
    {
        //play VFX
        gameObject.SetActive(false);
        _trans.position = defaultPos;
    }
    private void ReviveCharacter()
    {
        _health = characterScriptableObj.health;
        gameObject.SetActive(true);
    }

    #region Helper Functions

    private void MoveToTarget()
    {
        rb.velocity = new Vector3(target.position.x * _speed, rb.velocity.y, rb.velocity.z);
        currentState = CharacterState.Running;
    }

#endregion
}

public enum CharacterState
{
    Running,
    Fighting
};
