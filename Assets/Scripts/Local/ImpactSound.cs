using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    public AudioSource audioSource;
    public float minVelocity = 1f;

    void OnCollisionEnter(Collision collision)
    {
        if (minVelocity > collision.relativeVelocity.magnitude) return;

        AudioSource.PlayClipAtPoint(audioSource.clip, collision.contacts[0].point, audioSource.volume);
    }
}
