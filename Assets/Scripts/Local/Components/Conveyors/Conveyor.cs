using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [Header("State")]
    public List<Rigidbody> affectedRigidbodies = new List<Rigidbody>();
    public bool Activated = false;
    public bool direction = true;
    public float lerpDirection = 0f;
    public Vector3 beltForward => transform.forward;
    [Header("Settings")]
    public float maxSpeed = 5f;
    public float accelerationForce = 30f;
    public float directionChangeSpeed = 0.5f;
    public MeshRenderer[] beltParts;


    void FixedUpdate() 
    {
        foreach(var rigidbody in affectedRigidbodies)
        {
            if (!Activated) return;
            float inDir = Vector3.Dot(beltForward * lerpDirection, rigidbody.velocity);
            if (inDir < maxSpeed) rigidbody.AddForce(beltForward * accelerationForce * lerpDirection);
        }
        lerpDirection = Mathf.Lerp(lerpDirection, direction ? 1 : -1, directionChangeSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider) return;
        if (!collider.attachedRigidbody) return;

        Rigidbody rigidbody = collider.attachedRigidbody;

        if (affectedRigidbodies.Contains(rigidbody)) return;

        affectedRigidbodies.Add(rigidbody);
    }
    void OnTriggerExit(Collider collider)
    {
        if (!collider) return;
        if (!collider.attachedRigidbody) return;

        Rigidbody rigidbody = collider.attachedRigidbody;

        if (!affectedRigidbodies.Contains(rigidbody)) return;

        affectedRigidbodies.Remove(rigidbody);
    }

    public void ChangeDirection() {
        direction = !direction;
    }

    public void SetDirection(bool dir) {
        if (dir != direction) {
            affectedRigidbodies.Clear();
        }

        direction = dir;
    }

    public void ChangeActivation() {
        Activated = !Activated;
    }

    public void SetActivation(bool active) {
        Activated = active;
    }

    void LateUpdate() {
        if (Activated) {
            foreach (MeshRenderer mr in beltParts) {
                mr.material.SetFloat("_timePassed", mr.material.GetFloat("_timePassed") + Time.deltaTime * -lerpDirection * maxSpeed / 20);
            }
        }
    }
}
