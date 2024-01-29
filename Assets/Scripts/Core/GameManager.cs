using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private House _enemyHouse;
    [SerializeField] private House _playerHouse;

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
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }
    private void SubscribeToEvents()
    {
        _enemyHouse.E_HouseDestroyed += OpenWinPanel;
        _playerHouse.E_HouseDestroyed += OpenLosePanel;
    }
    private void UnSubscribeToEvents()
    {
        _enemyHouse.E_HouseDestroyed -= OpenWinPanel;
        _playerHouse.E_HouseDestroyed -= OpenLosePanel;
    }


    private void OpenWinPanel()
    {
        _uiManager.TurnOnWinPanel();
        Time.timeScale = 0;
    }
    private void OpenLosePanel()
    {
        _uiManager.TurnOnLosePanel();
        Time.timeScale = 0;
    }
}
