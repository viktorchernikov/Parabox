using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [Header("State")]
    public List<Rigidbody> zonedRigidbodies = new List<Rigidbody>();
    public List<Rigidbody> collidedRigidbodies = new List<Rigidbody>();
    public bool Activated = false;
    public bool direction = true;
    public float lerpDirection = 0f;
    public Vector3 beltForward => transform.forward;
    [Header("Settings")]
    public float maxSpeed = 5f;
    public float directionChangeSpeed = 0.5f;
    public MeshRenderer[] beltParts;


    void FixedUpdate()
    {
        lerpDirection = Mathf.Lerp(lerpDirection, Activated ? (direction ? 1 : -1) : 0, directionChangeSpeed * Time.fixedDeltaTime);
        if (!Activated) return;
        foreach (var rigidbody in zonedRigidbodies)
        {
            if (!collidedRigidbodies.Contains(rigidbody)) continue;
            float inDir = Vector3.Dot(beltForward * lerpDirection, rigidbody.velocity);
            if (inDir < maxSpeed) rigidbody.AddForce(beltForward * lerpDirection, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider) return;
        if (!collider.attachedRigidbody) return;

        Rigidbody rigidbody = collider.attachedRigidbody;

        if (zonedRigidbodies.Contains(rigidbody)) return;

        zonedRigidbodies.Add(rigidbody);
    }
    void OnTriggerExit(Collider collider)
    {
        if (!collider) return;
        if (!collider.attachedRigidbody) return;

        Rigidbody rigidbody = collider.attachedRigidbody;

        if (!zonedRigidbodies.Contains(rigidbody)) return;

        if (collidedRigidbodies.Contains(rigidbody)) zonedRigidbodies.Remove(rigidbody);
        zonedRigidbodies.Remove(rigidbody);
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision == null) return;
        
        Collider collider = collision.collider;
        if (!collider) return;

        Rigidbody rigidbody = collider.attachedRigidbody;
        if (!rigidbody) return;

        if (collidedRigidbodies.Contains(rigidbody)) return;
        if (!zonedRigidbodies.Contains(rigidbody)) return;

        collidedRigidbodies.Add(rigidbody);
    }

    public void ChangeDirection() {
        direction = !direction;
    }

    public void SetDirection(bool dir) {
        if (dir != direction) {
            zonedRigidbodies.Clear();
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
