public interface IInteractable
{
    public virtual InteractableHoverResponse GetHoverResponse(IInteractor interactor)
    {
        return InteractableHoverResponse.Disabled;
    }
    public virtual bool CanInteract(IInteractor interactor)
    {
        return false;
    }
    /// <summary>
    /// Called when player interacted
    /// </summary>
    public virtual void OnInteract(IInteractor interactor) { }
}

public enum InteractableHoverResponse
{
    None,
    Disabled,
    Talk,
    Use,
    Pick,
    Open,
    Close
}