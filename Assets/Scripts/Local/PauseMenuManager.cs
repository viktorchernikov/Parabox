using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public bool showMenu;
    public bool childMenuOpen;
    public GameObject menu;

    CursorLockMode prev;


    public static PauseMenuManager inst;

    void Awake() {
        inst = this;
    }

    void Start() {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(menu);
    }

    public void OpenUp() {
        showMenu = true;
    }

    public void CloseDown() {
        showMenu = false;
    }

    bool oldShowMenu = false;
    private void Update() {
        if (!FindFirstObjectByType<SettingsMenuManager>().IsShown()) childMenuOpen = false;

        if (Input.GetKeyDown(KeyCode.I)){
            if (showMenu && !childMenuOpen) showMenu = false;
            else if (!showMenu && SceneManager.GetActiveScene().name != "Menu") showMenu = true;
        }

        if (showMenu != oldShowMenu) {
            menu.SetActive(showMenu);
            if (showMenu) {
                prev = Cursor.lockState;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("showmenu");
            }
            else {
                Cursor.lockState = prev;
                Cursor.visible = false;
            }
        }

        Debug.Log(Cursor.lockState);
        oldShowMenu = showMenu;
    }



    public void Resume() {
        CloseDown();
        FindFirstObjectByType<SettingsMenuManager>().Close();
        Debug.Log("Resume");
    }

    public void Settings() {
        FindFirstObjectByType<SettingsMenuManager>().OpenUp();
        childMenuOpen = true;
        Debug.Log("RTYU");
    }

    public void MainMenu() {
        LoadingMenuManager.inst.LoadScene("Main");
        Debug.Log("MIAN");
    }
}
