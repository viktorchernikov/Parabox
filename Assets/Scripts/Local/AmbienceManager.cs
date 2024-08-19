using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager main;

    public AudioClip[] tracks;


    public void SetTrack(int trackIndex)
    {
        Player.local.ambientSource.Stop();
        Player.local.ambientSource.time = 0;
        Player.local.ambientSource.clip = tracks[trackIndex];
        Player.local.ambientSource.Play();
    }

    private void Awake()
    {
        main = this;
    }
}
