using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorkBoard : MonoBehaviour
{
    Dictionary<IPin, PinState> Attached = new Dictionary<IPin, PinState>();

    void OnTriggerEnter(Collider collider) {
        if (!collider.TryGetComponent(out IPin pin) || !collider.TryGetComponent(out IPickupable pickupable)) return;

        if (!Attached.ContainsKey(pin)) { 
            Attached.Add(pin, PinState.JustAttached);
            pickupable.self.GetComponent<Collider>().enabled = false;
            Physics.Raycast(new Ray(collider.transform.position, -transform.forward), out RaycastHit hit, 1f);
            pickupable.self.GetComponent<Collider>().enabled = true;
            pin.Attach(transform.forward, hit.point);
            pickupable.DropItself();
        }
        else if (Attached[pin] == PinState.JustDetached) {
            Attached.Remove(pin);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (!collider.TryGetComponent(out IPin pin)) return;

        if (Attached.ContainsKey(pin)) {
            if (Attached[pin] == PinState.JustAttached) {
                Attached[pin] = PinState.None;
                return;
            }

            Attached[pin] = PinState.JustDetached;
            pin.Detach();
        }
    }

    enum PinState {
        JustAttached,
        JustDetached,
        None
    }


    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.forward + transform.position, 0.1f);
    }
}
