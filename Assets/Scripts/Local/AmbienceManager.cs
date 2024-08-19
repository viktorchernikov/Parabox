using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager main;

    public AudioClip[] tracks;

    public float changingTime = 1.25f;


    public void SetTrack(int trackIndex)
    {
        Player.local.ambientSource.Stop();
        Player.local.ambientChanging.time = Player.local.ambientSource.time;
        Player.local.ambientChanging.clip = Player.local.ambientSource.clip;
        Player.local.ambientChanging.volume = Player.local.ambientSource.volume;
        Player.local.ambientChanging.Play();
        Player.local.ambientSource.time = 0;
        Player.local.ambientSource.clip = tracks[trackIndex];
        Player.local.ambientSource.Play();

        StartCoroutine(ChangeTrack());
    }

    public IEnumerator ChangeTrack() {
        float volume_ch = Player.local.ambientChanging.volume;
        float volume_sr = Player.local.ambientSource.volume;

        float timePassed = 0f;
        while (timePassed < 1.25f) {
            Player.local.ambientChanging.volume = volume_ch * (1 - (timePassed / 1.25f));
            Player.local.ambientSource.volume = volume_sr * (timePassed / 1.25f);

            yield return null;
            timePassed += Time.deltaTime;
        }

        Player.local.ambientChanging.volume = volume_ch;
        Player.local.ambientSource.volume = volume_sr;
    }

    private void Awake()
    {
        if (main != null) main = this;
    }
}
