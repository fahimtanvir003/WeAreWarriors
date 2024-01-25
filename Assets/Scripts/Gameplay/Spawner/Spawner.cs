using Sirenix.Utilities;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [HideInInspector] public Transform trans;

    public Transform leftPos;
    public Transform rightPos;

    [Space(10)]
    [Header("---Waves Attributes---")]
    [Space(5)]

    [SerializeField] private Wave[] waves;


    private void Awake()
    {
        trans = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Wave wave in waves)
        {
            wave.SpawnCharacters(leftPos, rightPos, trans);
        }
    }

    public void Button_SpawnCharacterFromThePoolOfList()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            for (int j = 0; j < waves[i].spawnnedCharactersList.Count; j++)
            {
                int pickRandom = UnityEngine.Random.Range(0, waves[i].spawnnedCharactersList.Count - 1);

                GameObject selectedCharacter = waves[i].spawnnedCharactersList[pickRandom];

                if (!selectedCharacter.activeInHierarchy)
                {
                    selectedCharacter.SetActive(true);
                }
                break;
            }
        }
    }

    [Serializable]
    private class Wave
    {
        [Header("---Spawner Attributes---")]
        [Space(5)]

        [SerializeField] private Transform spawnParent;
        [SerializeField] private int characterAmnt;
        public CharacterScriptableObj[] charactersToSpawn;

        [Space(15)]

        public List<GameObject> spawnnedCharactersList;


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
                obj.SetActive(false);
                //obj.tag = "Hudai";
                spawnnedCharactersList.Add(obj);
            }
        }

        #endregion
    }
}