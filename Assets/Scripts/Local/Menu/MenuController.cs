using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    #region Components
    [Header("Components")]
    [SerializeField] GameObject pauseMenuUI;
    #endregion

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !FindAnyObjectByType<SettingsMenuManager>().showMenu)
        {
            if (!PauseController.instance.isPaused)
                PauseGame();
            else
                UnpauseGame();
        }
    }

    public void PauseGame()
    {
        PauseController.instance.Pause();
        pauseMenuUI.SetActive(true);
    }

    public void UnpauseGame()
    {
        PauseController.instance.Unpause();
        pauseMenuUI.SetActive(false);
    }

    public void Settings() {
        FindAnyObjectByType<SettingsMenuManager>().showMenu = true;
    }
}
