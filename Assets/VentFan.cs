using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VentFan : MonoBehaviour
{
    public float affectedArea = 4f;
    public Transform rotor;
    public float maxSpeed = 5f;

    public bool activated;

    void Update() {
        if (!activated) return;

        rotor.eulerAngles += new Vector3(0, 0, maxSpeed * 50 * Time.deltaTime);
    }

    void OnTriggerStay(Collider collider) {
        if (!activated) return;

        Rigidbody rb = collider.attachedRigidbody;

        float inDir = Vector3.Dot(transform.forward, rb.velocity);
        if (inDir < maxSpeed) {
            rb.AddForce(transform.forward * 100f);
        }
        else if (inDir > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void Activate() {
        activated = true;
    }

    public void Desactivate() {
        activated = false;
    }

    public void SetActive(bool active) {
        activated = active;
    }

    public void SpeedSpeed(float speed_) {
        maxSpeed = speed_;
    }

    /*void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3[] vertices = multVertVec3(GetVert(), new Vector3(transform.lossyScale.x, transform.lossyScale.y, affectedArea));
        for (int f = 0; f < conn.Length; f+=2) {
            Vector3 start = vertices[conn[f]] + startAnchor.position;
            Vector3 end = vertices[conn[f + 1]] + startAnchor.position;

            Debug.Log(start + " " + end);

            Gizmos.DrawLine(start, end);
        }
    }

#region wireframe rect and util
    Vector3[] multVertVec3(Vector3[] a, Vector3 b) {
        Vector3[] q = new Vector3[a.Length];
        for (int i = 0; i < a.Length; i++) {
            q[i] = new Vector3(a[i].x * b.x, a[i].y *b.y, a[i].z *b.z);
        }

        return q;
    }

    Vector3[] GetVert() {
        return new Vector3[] {
            Vector3.zero,
            transform.right,
            transform.up,
            transform.right + transform.up,
            transform.forward,
            transform.forward + transform.right,
            transform.forward + transform.up,
            transform.forward + transform.right + transform.up
        };
    }
    int[] conn = {
        0, 1,
        0, 2,
        0, 4,
        1, 3,
        1, 5,
        2, 3,
        2, 6,
        3, 7,
        4, 5,
        4, 6,
        5, 7,
        6, 7
     };
#endregion*/
}
