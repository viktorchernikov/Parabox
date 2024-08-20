using System.Collections;
using UnityEngine;

public class Wormbox : MonoBehaviour, IInteractable
{
    [Header("State")]
    public Player player;
    public bool isOpen = true;
    public bool isBusy = false;
    [Header("Settings")]
    public Wormbox linkedBox;
    public Animator animator;
    public float playerScale = 1f;
    [Header("Points")]
    public Transform sitPoint;


    public InteractableHoverResponse GetHoverResponse(IInteractor interactor)
    {
        if (isBusy)
            return InteractableHoverResponse.None;

        return InteractableHoverResponse.Jumpin;
    }
    public bool CanInteract(IInteractor interactor)
    {
        Player player = interactor as Player;

        return !isBusy && player.currentScale == playerScale && isOpen;
    }
    public void OnInteract(IInteractor interactor)
    {
        if (isBusy)
            return;

        player = interactor as Player;
        StartCoroutine(WormholingSequence());
    }

    public void OpenFlaps()
    {
        animator.SetTrigger("OnOpen");
        isOpen = true;
    }
    public void CloseFlaps()
    {
        animator.SetTrigger("OnClose");
        isOpen = false;
    }

    IEnumerator WormholingSequence()
    {
        isBusy = true;
        linkedBox.isBusy = true;

        player.GetModule<PlayerHoldingModule>().Drop();
        PlayerCinematicController playerCinematicController = player.GetModule<PlayerCinematicController>();

        playerCinematicController.onWormholeTeleport += ChangeScale;
        playerCinematicController.PlayWormholeJumpin(this);

        yield return null;
        yield return new WaitUntil(() => !playerCinematicController.isPlaying);
        playerCinematicController.onWormholeTeleport -= ChangeScale;

        isBusy = false;
        linkedBox.isBusy = false;
        player = null;
    }

    void Awake()
    {
        if (isOpen) animator.SetTrigger("OnOpen");
    }
    void ChangeScale()
    {
        player.SetScale(linkedBox.playerScale);
    }
}
