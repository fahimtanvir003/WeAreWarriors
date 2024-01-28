using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private MeatGeneration script_MeatGeneration;
    [SerializeField] private List<SpawnButton> _spawnButtonList;


    private void OnDisable()
    {
        UnSubscribeToEvents();
    }

    private void Awake()
    {
        _spawnButtonList = GetComponentsInChildren<SpawnButton>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ManageLockingAndUnlockingOfSpawnButtons()
    {
        foreach (var button in _spawnButtonList)
        {
            if (script_MeatGeneration.meatAmount >= button.meatAmountToUnlock)
            {
                button.UnlockButton();
            }
            else
            {
                button.LockButton();
            }
        }
    }

    private void SubscribeToEvents()
    {
        script_MeatGeneration.E_MeatMeterFilled += ManageLockingAndUnlockingOfSpawnButtons;

        foreach (var button in _spawnButtonList)
        {
            button.E_ButtonPressed += SubstractMeatAmountOnButtonPress;
        }
    }
    private void UnSubscribeToEvents()
    {
        script_MeatGeneration.E_MeatMeterFilled -= ManageLockingAndUnlockingOfSpawnButtons;

        foreach (var button in _spawnButtonList)
        {
            button.E_ButtonPressed -= SubstractMeatAmountOnButtonPress;
        }
    }

    private void SubstractMeatAmountOnButtonPress(int amountSpent)
    {
        script_MeatGeneration.meatAmount -= amountSpent;
    }
}
