using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject[] Categories;
    int selectedCategory = 0;
    public Color selected;
    public Color notSelected;

    public GameObject menu;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SfxSlider;
    public TMP_InputField mouseX;
    public TMP_InputField mouseY;


    public void SetCategory(int categoryNum) {
        int i = 0;
        foreach (GameObject f in Categories) {
            f.SetActive(i == categoryNum);
            f.transform.parent.Find("Selected").GetComponent<Image>().color = i == categoryNum ? selected : notSelected;
            i++;
        }
        selectedCategory = categoryNum;
    }

    public void SetQuality(int value) {
        PlayerSettings.SetQuality((PlayerSettings.Quality)value);
        PlayerSettings.Save();
    }

    public void SetMasterVolume() {
        PlayerSettings.SetMasterVolume(MasterSlider.value);
        PlayerSettings.Save();
    }

    public void SetMusicVolume() {
        PlayerSettings.SetMusicVolume(MusicSlider.value);
        PlayerSettings.Save();
    }

    public void SetSfxVolume() {
        PlayerSettings.SetSfxVolume(SfxSlider.value);
        PlayerSettings.Save();
    }

    public void SetSensitivity() {
        try { PlayerSettings.SetMouseSensitivity(new Vector2(int.Parse(mouseX.text), int.Parse(mouseY.text))); }
        catch {}

        PlayerSettings.Save();
    }

    public void Save() {
        PlayerSettings.Save();
    }


    bool showMenu = false;
    CursorLockMode prev;

    void Start() {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(menu);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) showMenu = !showMenu;
        else return;

        menu.SetActive(showMenu);
        if (showMenu) {
            prev = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Cursor.lockState = prev;
        }
    }
}
