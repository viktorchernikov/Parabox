using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AmbienceZone : MonoBehaviour
{
    public int trackId;

    BoxCollider trigger;

    private void Awake()
    {
        trigger = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other) return;

        Player player = other.GetComponentInParent<Player>();

        if (!player) return;

        AmbienceManager.main.SetTrack(trackId);
        player.ambientSource.Play();
    }
}
