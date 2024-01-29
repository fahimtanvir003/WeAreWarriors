using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterBehavior : MonoBehaviour
{
    [Header("---Components---")]
    [Space(5f)]

    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRend;
    [SerializeField] private Animator _anim;
    [SerializeField] private Slider _healthSlider;

    private Transform _trans;
    private Transform _target;
    private CharacterState _currentState;
    private int _defaultHealth;
    public int health;
    private float _attackInterval;
    private float _stoppingDistanceForEnemy;
    private float _stoppingDistanceForEnemyHouse;

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

        ResetCharacterHealth();
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
        if (health <= 0)
        {
            Die();
        }

        CheatDie();
    }
    private void FixedUpdate()
    {
        if (_currentState != CharacterState.Fighting)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        SetNavmeshAndAppropriateTarget();

        _currentState = CharacterState.Running;
    }

    private void Die()
    {
        VfxPool.instance.PlayVfx("Blast", _trans.position, Quaternion.identity);
        //Play sound 

        gameObject.SetActive(false);
        ClearDetectedEnemyList();

        _trans.position = defaultPos;
    }
    private void ResetCharacterHealth()
    {
        health = _defaultHealth;
        _healthSlider.maxValue = health;
        _healthSlider.value = health;
    }

    private void StartCombat()
    {
        _currentState = CharacterState.Fighting;

        _anim.SetTrigger("Attack");
    }





    #region Helper Functions

    private void FindTargetAccordingly(CharacterScriptableObj characterScriptableObj)
    {
        if (!characterScriptableObj.isEnemy)
        {
            _target = GameObject.Find(characterScriptableObj.targetHouseName).transform;
            SetCharacterInitialRotation(0);
            SetMainWeaponLayer(7);
        }
        else
        {
            _target = GameObject.Find(characterScriptableObj.targetHouseName).transform;
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
        script_CharacterWeapon.E_WeaponCollidedWithOppositeCharacter += SimulateHit;
    }

    private void UnSubscribeToEvents()
    {
        script_CharacterWeapon.E_WeaponCollidedWithOppositeCharacter -= SimulateHit;
    }

    private void ClearDetectedEnemyList()
    {
        script_EnemyDetection.currentDetectedEnemiesList.Clear();
        script_EnemyDetection.NearestEnemy = null;
    }
    private void InitializeScriptableObjectsReference(CharacterScriptableObj characterScriptableObj)
    {
        script_EnemyDetection.character = characterScriptableObj;
        script_CharacterWeapon.weaponScriptableObj = characterScriptableObj.weaponScriptableObj;
        script_CharacterWeapon.isEnemyWeapon = characterScriptableObj.isEnemy;
    }

    private void InitializePlayerAttributes(CharacterScriptableObj characterScriptableObj)
    {
        gameObject.tag = characterScriptableObj.characterTag;
        _navMeshAgent.speed = characterScriptableObj.speed;
        health = characterScriptableObj.health;
        _stoppingDistanceForEnemy = characterScriptableObj.stoppingDistanceForEnemy;
        _stoppingDistanceForEnemyHouse = characterScriptableObj.stoppingDistanceForEnemyHouse;
        _attackInterval = characterScriptableObj.attackInterval;
        _skinnedMeshRend.materials = characterScriptableObj.characterMaterials;
        _defaultHealth = characterScriptableObj.health;

        SetHealthProperties(characterScriptableObj);
        SetWeaponTagAccordingly(characterScriptableObj);
    }

    private void SimulateHit(CharacterBehavior script_CharacterBehavior)
    {
        script_CharacterBehavior.DecreaseHealth(script_CharacterWeapon._damage);
    }

    private void TargetTheHouse()
    {
        if (_target != null)
        {
            _navMeshAgent.SetDestination(new Vector3(_target.position.x, _trans.localPosition.y, _trans.localPosition.z));
        }
        _navMeshAgent.stoppingDistance = _stoppingDistanceForEnemyHouse;

        if (_navMeshAgent.remainingDistance <= _stoppingDistanceForEnemyHouse)
        {
            AttackAfterTheInterval();
            ToggleWalkingAnimation(false); 
        }
        else
        {
            ToggleWalkingAnimation(true); 
        }
    }

    private void TargetTheEnemy()
    {
        _navMeshAgent.stoppingDistance = _stoppingDistanceForEnemy;

        _navMeshAgent.SetDestination(new Vector3(script_EnemyDetection.DetectedNearestEnemy().position.x,
                _trans.position.y, script_EnemyDetection.DetectedNearestEnemy().position.z));

        if (_navMeshAgent.remainingDistance < 3)
        {
            AttackAfterTheInterval();
            ToggleWalkingAnimation(false);
        }
        else
        {
            ToggleWalkingAnimation(true);
        }
    }

    private void AttackAfterTheInterval()
    {
        if (_attackInterval <= 0)
        {
            StartCombat();
            _attackInterval = 1;
        }
        else
        {
            _attackInterval -= Time.deltaTime;
        }
    }

    private void SetHealthProperties(CharacterScriptableObj characterScriptableObj)
    {
        _healthSlider.maxValue = characterScriptableObj.health;
        _healthSlider.value = characterScriptableObj.health;
    }
    private void SetWeaponTagAccordingly(CharacterScriptableObj characterScriptableObj)
    {
        if (characterScriptableObj.isEnemy)
        {
            script_CharacterWeapon.gameObject.tag = "EnemyWeapon";
        }
        else
        {
            script_CharacterWeapon.gameObject.tag = "PlayerWeapon";
        }
    }

    private void ToggleWalkingAnimation(bool state)
    {
        _anim.SetBool("Walking", state);
    }

    public void DecreaseHealth(int damage)
    {
        health -= damage;
        _healthSlider.value = health;
    }
    #endregion
}

public enum CharacterState
{
    Running,
    Fighting
};
