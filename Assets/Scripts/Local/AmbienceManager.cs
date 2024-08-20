using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager main;

    public AudioClip[] tracks;
    public float[] volumes;
    public int currentTrack = 0;
    public bool askedTrack = false;
    public float transitionSpeed = 0.5f;

    AudioSource[] audioSources;


    public void SetTrack(int trackIndex)
    {
        askedTrack = true;
        currentTrack = trackIndex;
    }

    private void Awake()
    {
        main = this;
        audioSources = new AudioSource[tracks.Length];

        for (int i = 0; i < tracks.Length; i++)
        {
            var aus = gameObject.AddComponent<AudioSource>();
            aus.volume = volumes[i];
            aus.clip = tracks[i];
            aus.loop = true;
            aus.volume = 0;
            aus.Play();
            audioSources[i] = aus;
        }
    }
    private void Update()
    {
        if (!askedTrack) return;

        for (int i = 0; i < audioSources.Length; i++)
        {
            float desiredVolume = i == currentTrack ? volumes[i] : 0f;
            audioSources[i].volume = Mathf.Lerp(audioSources[i].volume, desiredVolume, transitionSpeed * Time.deltaTime);
        }
    }
}
