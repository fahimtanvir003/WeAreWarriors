using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private Button _spawnButton;
    [SerializeField] private TextMeshProUGUI _meatRequirementText;
    public int meatAmountToUnlock;

    public Action<int> E_ButtonPressed;

    // Start is called before the first frame update
    void Start()
    {
        LockButton();

        _meatRequirementText.text = meatAmountToUnlock.ToString();
    }

    public void ButtonPressed()
    {
        E_ButtonPressed?.Invoke(meatAmountToUnlock);
    }

    public void LockButton()
    {
        _spawnButton.interactable = false;
    }
    public void UnlockButton()
    {
        _spawnButton.interactable = true;
    }
}
