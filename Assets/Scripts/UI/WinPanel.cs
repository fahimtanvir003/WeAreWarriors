using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    public void TurnOnWinPanel(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void Button_Restart()
    {
        SceneManager.LoadScene(0);
    }
}
