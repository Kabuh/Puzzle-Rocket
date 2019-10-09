using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject Gear;

    public void MakePause() {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        Gear.SetActive(false);
    }

    public void Resume() {
        Time.timeScale = 1;
        Gear.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void GetToMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
