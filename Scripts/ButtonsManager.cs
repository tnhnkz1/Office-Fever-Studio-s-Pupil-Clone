using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject settingsPanel;
    
    public void DoSettings()
    {
        settingsButton.SetActive(false);
        settingsPanel.SetActive(true);

        Time.timeScale = 0;
    }

    public void DoResume()
    {
        settingsPanel.SetActive(false);
        settingsButton.SetActive(true);

        Time.timeScale = 1;
    }

    public void DoQuit()
    {
        Application.Quit();
    }

    public void DoRestart()
    {
        SceneManager.LoadScene("ArcadeIdle");

        Time.timeScale = 1;
    }
}
