using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingPin : StandardPickupable, IPin
{
    public Color pinColor;

    public bool attached { get; private set; }

    Vector3 currentBoardDir = Vector3.zero;

    Rigidbody rb;

    public void Attach(Vector3 corkBoardDir, Vector3 pos)
    {
        rb.isKinematic = true;
        attached = true;
        rb.position = pos;
        transform.eulerAngles = -corkBoardDir;
        currentBoardDir = corkBoardDir;
    }

    public void Detach() {
        attached = false;
        rb.isKinematic = false;
        transform.position += currentBoardDir * 0.05f;
    }

    void Awake() {
        rb = GetComponent<Rigidbody>();
        transform.Find("Handle").GetComponent<MeshRenderer>().material.color = pinColor;
    }

    public override void OnInteract(IInteractor interactor) 
    {
        if (attached) {
            Detach();
            rb.AddForce(currentBoardDir, ForceMode.Impulse);
            return;
        }

        base.OnInteract(interactor);
    }
}
