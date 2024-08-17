using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable {
    public Transform self { get; }
    public float holdingDistance { get; }
    public void Throw(Vector3 lookDir, float force);
}
