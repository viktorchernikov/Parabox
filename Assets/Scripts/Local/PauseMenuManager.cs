using System;
using System.Runtime.Serialization.Formatters;
using UnityEngine;


public class PauseMenuManager : MonoBehaviour
{
    public bool shown => GamePause.active && !SettingsMenu.visible;
    public GameObject menu;

    public static PauseMenuManager main;

    #region Obsolete
    [Obsolete] public bool showMenu;
    [Obsolete] public bool childMenuOpen;
    [Obsolete] CursorLockMode prev;
    [Obsolete] public static PauseMenuManager inst;
    [Obsolete] public void OpenUp() => showMenu = true;
    [Obsolete] public void CloseDown() => showMenu = false;
    [Obsolete]
    public void MainMenu()
    {
        GamePause.active = false;
        SettingsMenu.visible = false;
        LoadingMenuManager.inst.LoadScene("Menu");
    }
    [Obsolete]
    public void Settings()
    {
        FindFirstObjectByType<SettingsMenuManager>().OpenUp();
    }
    #endregion


    void Awake() {
        if (main != this && main != null) { Destroy(menu); Destroy(gameObject); return; }
        main = this;
        menu.SetActive(false);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(menu);
    }
    void Update() 
    {
        if (!Player.local) return;

        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePause.active)
                Resume();
            else
                Pause();
        }
        menu.SetActive(shown);
    }

    public void Resume()
    {
        GamePause.active = false;
        SettingsMenu.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        menu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Pause()
    {
        GamePause.active = true;
        SettingsMenu.visible = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menu.SetActive(true);
        Time.timeScale = 0f;
    }
}

public static class GamePause
{
    public static bool active { get; set; } = false;
}