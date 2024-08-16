using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable
{
    #region State
    public bool isActive { get => _isActive; private set => _isActive = value; }
    public bool isBusy { get => _isBusy; private set => _isBusy = value; }
    #endregion
    #region Events
    public event Action onActivate;
    public event Action onDeactivate;
    #endregion

    #region Editor State
    [Header("State")]
    [SerializeField] bool _isActive = false;
    [SerializeField] bool _isBusy = false;
    #endregion
    #region Unity Events
    [Header("Events")]
    [SerializeField] UnityEvent _onActivateEvent;
    [SerializeField] UnityEvent _onDeactivateEvent;
    #endregion
    #region Delays
    [Header("Delays")]
    [SerializeField] float _switchAnimationDelay = 0.4f;
    [SerializeField] float _switchPostDelay = 0.2f;
    #endregion
    #region Parameters
    [Header("Parameters")]
    [SerializeField] bool _isActiveDefault = false;
    #endregion


    public InteractableHoverResponse GetHoverResponse(IInteractor interactor)
    {
        if (isBusy)
            return InteractableHoverResponse.None;
        
        return isActive ? InteractableHoverResponse.Disable : InteractableHoverResponse.Enable;
    }
    public bool CanInteract(IInteractor interactor)
    {
        return !isBusy;
    }
    public void OnInteract(IInteractor interactor)
    {
        if (isBusy)
            return;
        StartCoroutine(SwitchSequence());
    }

    IEnumerator SwitchSequence()
    {
        isBusy = true;
        yield return new WaitForSeconds(_switchAnimationDelay);
        isActive = !isActive;

        if (isActive)
        {
            _onActivateEvent.Invoke();
            onActivate?.Invoke();
        }
        else
        {
            _onDeactivateEvent.Invoke();
            onDeactivate?.Invoke();
        }
        yield return new WaitForSeconds(_switchPostDelay);
        isBusy = false;
    }
}
