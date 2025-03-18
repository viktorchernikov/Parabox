using System.Collections;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    public AudioSource audioSource;
    public float minVelocity = 1f;

    bool canPlay = false;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(10f);
        canPlay = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (minVelocity > collision.relativeVelocity.magnitude || !canPlay) return;

        AudioSource.PlayClipAtPoint(audioSource.clip, collision.contacts[0].point, audioSource.volume);
    }
}
