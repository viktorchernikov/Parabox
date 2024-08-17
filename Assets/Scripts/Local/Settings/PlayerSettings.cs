using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings
{

    #region set
    public static void SetVolume(float master = -1f, float music = -1f, float sfx = -1f) {
        if (master > 0f) PlayerPrefs.SetFloat("master_volume", Mathf.Clamp(0f, 1f, master));
        if (music > 0f) PlayerPrefs.SetFloat("music_volume", Mathf.Clamp(0f, 1f, music));
        if (sfx > 0f) PlayerPrefs.SetFloat("sfx_volume", Mathf.Clamp(0f, 1f, sfx));
    }

    public static void SetVolume(Volume v) {
        SetVolume(v.master, v.music, v.sfx);
    }

    public static void SetMasterVolume(float val) {
        SetVolume(master: val);
    }

    public static void SetMusicVolume(float val) {
        SetVolume(music: val);
    }

    public static void SetSfxVolume(float val) {
        SetVolume(sfx: val);
    }

    
    public static void SetQuality(Quality quality, string region = "overall") {
        PlayerPrefs.SetInt(region.ToLower() + "_quality", (int)quality);
    }

    public static void SetMouseSensitivity(Vector2 newSensitivity)
    {
        PlayerPrefs.SetFloat("mouse_sensitivity_x", newSensitivity.x);
        PlayerPrefs.SetFloat("mouse_sensitivity_y", newSensitivity.y);
    }
    #endregion

    #region get
    public static Volume GetVolume() {
        return new Volume(PlayerPrefs.GetFloat("master_volume"), PlayerPrefs.GetFloat("music_volume"), 
                          PlayerPrefs.GetFloat("sfx_volume"));
    }

    public static Quality GetQuality(string region) {
        return (Quality)PlayerPrefs.GetInt(region + "_quality");
    }

    public static Vector2 GetMouseSensitiity() {
        return new Vector2(PlayerPrefs.GetFloat("mouse_sensitivity_x"), PlayerPrefs.GetFloat("mouse_sensitivity_y"));
    }
    #endregion

    

    public static void Save() {
        PlayerPrefs.Save();
    }    

    public enum Quality { // Enum specjalnie w środku aby wiadomo było że to jest PlayerSettings.Quality
        Low,
        Medium,
        High
    }

    public struct Volume {
        public float master;
        public float music;
        public float sfx;

        public Volume(float master, float music, float sfx) {
            this.master = master;
            this.music = music;
            this.sfx = sfx;
        }
    }
}
