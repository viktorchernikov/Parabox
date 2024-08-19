using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class StandardPickupable : MonoBehaviour, IInteractable, IPickupable
{
    public Transform self => transform;

    public virtual float holdingDistance => 2f;
    public virtual bool throwable => false;

    public IInteractor holdedBy;

    public virtual InteractableHoverResponse GetHoverResponse(IInteractor interactor)
    {
        return CanInteract(interactor) ? InteractableHoverResponse.Pick : InteractableHoverResponse.None;
    }

    public virtual bool CanInteract(IInteractor interactor)
    {
        IPickupable ipck = ((Player)interactor).GetModule<PlayerHoldingModule>().currentlyHolding;
        return ipck == null || ipck == (IPickupable)this;
    }

    public virtual void OnInteract(IInteractor interactor) 
    {
        PlayerHoldingModule phm = ((Player)interactor).GetModule<PlayerHoldingModule>();

        holdedBy = interactor;

        if (phm.currentlyHolding == null) { phm.currentlyHolding = this; GetComponent<Rigidbody>().useGravity = false; }
        else { phm.Drop(); holdedBy = null; }
    }

    public virtual void Throw(Vector3 lookDir, float force)
    {
        if (!throwable) return;

        GetComponent<Rigidbody>().AddForce(lookDir * force, ForceMode.VelocityChange);
    }

    public void DropItself()
    {
        if (holdedBy == null) return;

        (holdedBy as Player).GetModule<PlayerHoldingModule>().Drop();
    }
}
