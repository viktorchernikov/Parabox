using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    void Start() {
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
    }

    public void StartGame() {
        LoadingMenuManager.inst.LoadScene("Main");
    }

    public void OpenSettings() {
        FindFirstObjectByType<SettingsMenuManager>().OpenUp();
    }

    public void QuitApplication() {
        Application.Quit();
    }
}
