using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterBehavior : MonoBehaviour
{
    [Header("---Components---")]
    [Space(5f)]

    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRend;

    private Transform _trans;
    private Transform target;
    private CharacterState currentState;
    private int defaultHealth;
    public int _health;
    private float _stoppingDistance;

    [Space(5f)]
    [Header("---Script Reference---")]
    [Space(5f)]

    [SerializeField] private CharacterWeapon script_CharacterWeapon;
    [SerializeField] private EnemyDetection script_EnemyDetection;

    private Vector3 defaultPos;




    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    private void Awake()
    {
        _trans = transform;
        defaultPos = _trans.position;
        _navMeshAgent.updateRotation = false;
    }

    public void Initialize(CharacterScriptableObj characterScriptableObj)
    {
        InitializeScriptableObjectsReference(characterScriptableObj);
        InitializePlayerAttributes(characterScriptableObj);
        FindTargetAccordingly(characterScriptableObj);
    }

    void Update()
    {
        if (_health <= 0)
        {
            Die();
        }

        CheatDie();
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
        SetNavmeshAndAppropriateTarget();

        currentState = CharacterState.Running;
    }

    private void Die()
    {
        //play VFX

        gameObject.SetActive(false);
        ClearDetectedEnemyList();

        _trans.position = defaultPos;
    }
    private void ReviveCharacter()
    {
        _health = defaultHealth;
        gameObject.SetActive(true);
    }

    private void StartCombat()
    {
        currentState = CharacterState.Fighting;
        _health--;
        //Play Fight Animation

        //Play VFX
    }





    #region Helper Functions

    private void FindTargetAccordingly(CharacterScriptableObj characterScriptableObj)
    {
        if (!characterScriptableObj.isEnemy)
        {
            target = GameObject.Find(characterScriptableObj.targetHouseName).transform;
            SetCharacterInitialRotation(0);
            SetMainWeaponLayer(7);
        }
        else
        {
            target = GameObject.Find(characterScriptableObj.targetHouseName).transform;
            SetCharacterInitialRotation(180);
            SetMainWeaponLayer(8);
        }
    }


    private void SetNavmeshAndAppropriateTarget()
    {
        if (script_EnemyDetection.DetectedNearestEnemy() == null)
        {
            TargetTheHouse();
        }
        else
        {
            TargetTheEnemy();
        }
    }
    private void SetMainWeaponLayer(int _layer)
    {
        script_CharacterWeapon.SetWeaponLayer(_layer);
    }

    private void SetCharacterInitialRotation(float rot)
    {
        _trans.rotation = Quaternion.Euler(0, rot, 0);
    }
    private void CheatDie()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!script_EnemyDetection.character.isEnemy)
            {
                Die();
            }
        }
    }

    private void SubscribeToEvents()
    {
        //script_CharacterWeapon.E_WeaponCollidedWithEnemy += StartCombat;

    }

    private void UnSubscribeToEvents()
    {
        //script_CharacterWeapon.E_WeaponCollidedWithEnemy -= StartCombat;

    }
    private void ClearDetectedEnemyList()
    {
        script_EnemyDetection.currentDetectedEnemiesList.Remove(script_EnemyDetection.NearestEnemy);
        script_EnemyDetection.NearestEnemy = null;
    }
    private void InitializeScriptableObjectsReference(CharacterScriptableObj characterScriptableObj)
    {
        script_EnemyDetection.character = characterScriptableObj;
        script_CharacterWeapon.weaponScriptableObj = characterScriptableObj.weaponScriptableObj;
    }

    private void InitializePlayerAttributes(CharacterScriptableObj characterScriptableObj)
    {
        gameObject.tag = characterScriptableObj.characterTag;
        _navMeshAgent.speed = characterScriptableObj.speed;
        _health = characterScriptableObj.health;
        _stoppingDistance = characterScriptableObj.stoppingDistance;
        skinnedMeshRend.materials = characterScriptableObj.characterMaterials;
        defaultHealth = characterScriptableObj.health;
    }

    private void TargetTheHouse()
    {
        _navMeshAgent.SetDestination(new Vector3(target.position.x, _trans.localPosition.y, _trans.localPosition.z));
        //_navMeshAgent.stoppingDistance = 4f;
    }

    private void TargetTheEnemy()
    {
        _navMeshAgent.stoppingDistance = _stoppingDistance;

        _navMeshAgent.SetDestination(new Vector3(script_EnemyDetection.DetectedNearestEnemy().position.x,
                _trans.position.y, script_EnemyDetection.DetectedNearestEnemy().position.z));
    }

    public void DecreaseHealth(int damage)
    {
        _health -= damage;
    }
    #endregion
}

public enum CharacterState
{
    Running,
    Fighting
};
