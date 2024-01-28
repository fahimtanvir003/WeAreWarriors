using Sirenix.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor.UIElements;

using UnityEngine;

public class Spawner : MonoBehaviour
{
    [HideInInspector] public Transform _trans;

    [SerializeField] private int _characterLayerNumberToExcludeFromHouseCollider;

    public Transform leftPos;
    public Transform rightPos;

    [SerializeField] private bool isEnemySpawner;

    [Space(10)]
    [Header("---Waves Attributes---")]
    [Space(5)]

    [SerializeField] private Wave[] waves;

    private Action E_SpawnCharacters;

    private void OnEnable()
    {
        E_SpawnCharacters += SpawnCharactersFromThePoolOfList;
    }
    private void OnDisable()
    {
        E_SpawnCharacters -= SpawnCharactersFromThePoolOfList;
    }

    private void Awake()
    {
        _trans = transform;

        foreach (Wave wave in waves)
        {
            wave.SpawnCharacters(leftPos, rightPos, _trans);
        }

    }

    private void Update()
    {
        if (isEnemySpawner)
        {
            SpawnCharactersFromThePoolOfList();
            isEnemySpawner = false;
        }
    }

    public void Button_SpawnCharacterFromThePoolOfList()
    {
        E_SpawnCharacters?.Invoke();
    }

    private void SpawnCharactersFromThePoolOfList()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            for (int j = 0; j < waves[i].spawnnedCharactersList.Count; j++)
            {
                int pickRandom = UnityEngine.Random.Range(0, waves[i].spawnnedCharactersList.Count - 1);

                CharacterBehavior selectedCharacter = waves[i].spawnnedCharactersList[pickRandom];

                if (!selectedCharacter.gameObject.activeInHierarchy)
                {
                    SetUpSelectedCharacter(selectedCharacter.gameObject);
                    break;
                }
            }
        }
    }

    #region Helper Functions

    private void SetUpSelectedCharacter(GameObject character)
    {
        character.layer = _characterLayerNumberToExcludeFromHouseCollider;
        character.SetActive(true);
    }

    #endregion

    [Serializable]
    private class Wave
    {
        [Header("---Spawner Attributes---")]
        [Space(5)]

        [SerializeField] private Transform spawnParent;
        [SerializeField] private int characterAmnt;
        public CharacterScriptableObj[] charactersToSpawn;

        [Space(15)]

        public List<CharacterBehavior> spawnnedCharactersList;


        public void SpawnCharacters(Transform leftPos, Transform rightPos, Transform spawnerTrans)
        {
            for (int i = 0; i < characterAmnt; i++)
            {
                InstantiateCharactersAndTurnOff(leftPos, rightPos, spawnerTrans);
            }
        }

        #region Helper Functions

        private void InstantiateCharactersAndTurnOff(Transform leftPos, Transform rightPos, Transform spawnerTrans)
        {
            float randomZPosition = UnityEngine.Random.Range(leftPos.position.z, rightPos.position.z);

            foreach (var character in charactersToSpawn)
            {
                GameObject obj = Instantiate(character.character, new Vector3(spawnerTrans.position.x, 0.5f, randomZPosition), Quaternion.identity, spawnParent);
                CharacterBehavior script_CharacterBehavior = obj.GetComponent<CharacterBehavior>();

                script_CharacterBehavior.Initialize(character);
                obj.SetActive(false);

                spawnnedCharactersList.Add(script_CharacterBehavior);
            }
        }

        #endregion
    }
}