using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject menu;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SfxSlider;
    public TMP_InputField mouseX;
    public TMP_InputField mouseY;

    public void SetQuality(int value) {
        PlayerSettings.SetQuality((PlayerSettings.Quality)value);
    }

    public void SetMasterVolume() {
        PlayerSettings.SetMasterVolume(MasterSlider.value);
    }

    public void SetMusicVolume() {
        PlayerSettings.SetMusicVolume(MusicSlider.value);
    }

    public void SetSfxVolume() {
        PlayerSettings.SetMusicVolume(SfxSlider.value);
    }

    public void SetSensitivity() {
        try { PlayerSettings.SetMouseSensitivity(new Vector2(int.Parse(mouseX.text), int.Parse(mouseY.text))); }
        catch {}
    }

    public void Save() {
        PlayerSettings.Save();
    }


    bool showMenu = false;
    CursorLockMode prev;

    void SceneChanged(Scene prevScene, Scene newScene) {
        prev = Cursor.lockState;
    }

    void Start() {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(menu);
        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) showMenu = !showMenu;
        else return;

        menu.SetActive(showMenu);
        if (showMenu) {
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Cursor.lockState = prev;
        }
    }
}
