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

    private Transform target;
    private CharacterState currentState;
    private float _speed;
    private int _health;


    private void Awake()
    {
        target = GameObject.Find("EnemyHouse").transform;
    }
    
    void Start()
    {
        _speed = characterScriptableObj.speed;
        _health = characterScriptableObj.health;
        skinnedMeshRend.materials = characterScriptableObj.characterMaterials;
    }


    void Update()
    {
        

    }
    private void FixedUpdate()
    {
        if (currentState != CharacterState.Fighting)
        {
            if(rb.position.x <= (target.position.x - 4.85f))
            {
                rb.velocity = new Vector3(target.position.x * _speed, rb.velocity.y, rb.velocity.z);
                Debug.Log(rb.position.x);

            }
            else
            {
                Debug.Log("Reached");
            }

        }
    }
}

public enum CharacterState
{
    Running,
    Fighting
};
