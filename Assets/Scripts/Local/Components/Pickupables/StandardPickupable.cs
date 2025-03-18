using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class StandardPickupable : MonoBehaviour, IInteractable, IPickupable
{
    public Transform self => transform;

    public float HoldingDistance = 3f;
    public bool IsThrowable = false;
    public bool isScaleSensitive = false;
    public float minScale = 0f;
    public float maxScale = 2f;

    public virtual float holdingDistance => HoldingDistance;
    public virtual bool throwable => IsThrowable;

    public IInteractor holdedBy;

    public virtual InteractableHoverResponse GetHoverResponse(IInteractor interactor)
    {
        return holdedBy == null ? InteractableHoverResponse.Pick : (IsThrowable ? InteractableHoverResponse.DropThrow : InteractableHoverResponse.Drop);
    }

    public virtual bool CanInteract(IInteractor interactor)
    {
        Player player = interactor as Player;

        if (!player) return false;

        if (isScaleSensitive && (player.currentScale < minScale || player.currentScale > maxScale)) return false;


        IPickupable ipck = player.GetModule<PlayerHoldingModule>().currentlyHolding;
        return ipck == null || ipck == (IPickupable)this;
    }

    public virtual void OnInteract(IInteractor interactor) 
    {
        Player player = (Player)interactor;
        PlayerHoldingModule phm = player.GetModule<PlayerHoldingModule>();

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
    void FixedUpdate()
    {
        if (holdedBy == null) return;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.velocity.magnitude < 10) return;

        Player player = holdedBy as Player;
        player.GetModule<PlayerHoldingModule>().Drop();
    }
}
