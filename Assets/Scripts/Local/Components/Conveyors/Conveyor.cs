using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float maxSpeed = 5f;

    public Vector3 beltForward => transform.forward;


    public bool Activated = false;
    public bool direction = true;
    int Direction => direction ? 1 : -1;

    public MeshRenderer[] beltParts;

    List<Collider> stopped = new List<Collider>();

    void OnTriggerStay(Collider collider) {
        float inDir = Vector3.Dot(beltForward * Direction, collider.attachedRigidbody.velocity);
        
        if (!Activated) {
            if (inDir > 0) {
                if (!stopped.Contains(collider)) {
                    collider.attachedRigidbody.AddForce(beltForward * Direction * -inDir * 4, ForceMode.Acceleration);
                    stopped.Add(collider);
                }
            }

            return;
        }

        if (inDir < maxSpeed) {
            collider.attachedRigidbody.AddForce(beltForward * 100f * Direction);
        }
        if (inDir > maxSpeed) {
            collider.attachedRigidbody.velocity = beltForward * Direction * maxSpeed;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (stopped.Contains(collider)) stopped.Remove(collider);
    }

    public void ChangeDirection() {
        stopped.Clear();

        direction = !direction;
    }

    public void SetDirection(bool dir) {
        if (dir != direction) {
            stopped.Clear();
        }

        direction = dir;
    }

    public void ChangeActivation() {
        Activated = !Activated;
    }

    public void SetActivation(bool active) {
        Activated = active;
    }

    void Update() {
        if (Activated) {
            foreach (MeshRenderer mr in beltParts) {
                mr.material.SetFloat("_timePassed", mr.material.GetFloat("_timePassed") + Time.deltaTime * -Direction * maxSpeed / 20);
            }
        }
    }
}
