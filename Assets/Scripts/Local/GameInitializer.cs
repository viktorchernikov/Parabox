using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    public static bool loadedAlwaysActive = false;
    void Start() {
        if (!loadedAlwaysActive)
        {
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
            //SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
            SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
            loadedAlwaysActive = true;
        }
    }

    public void StartGame() {
        LoadingMenuManager.inst.LoadScene("Main");
    }

    public void OpenSettings() {
        FindFirstObjectByType<SettingsMenuManager>().OpenUp();
    }

    public void QuitApplication() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
