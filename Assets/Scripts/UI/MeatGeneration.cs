using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeatGeneration : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _meatCounterText;

    [SerializeField] private float _regenerationIncreasingFactor = 10;
    private Image _timerImg;
    private float _fillAmount;

    public int meatAmount;
    public Action E_MeatMeterFilled;

    // Start is called before the first frame update
    void Start()
    {
        _timerImg = GetComponent<Image>();   
    }

    // Update is called once per frame
    void Update()
    {
        FillUpMeatTimer();
    }



    private void FillUpMeatTimer()
    {
        if (_fillAmount < 100)
        {
            _fillAmount += Time.deltaTime * _regenerationIncreasingFactor;
        }
        else
        {
            _fillAmount = 0;
            meatAmount++;
            E_MeatMeterFilled?.Invoke();
        }

        _timerImg.fillAmount = _fillAmount * 0.01f;
        _meatCounterText.text = meatAmount.ToString();
    }
}
