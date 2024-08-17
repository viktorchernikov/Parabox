using System.Collections;
using UnityEngine;

public class Wormbox : MonoBehaviour, IInteractable
{
    [Header("State")]
    public Player player;
    [Header("Settings")]
    public Wormbox linkedBox;
    public float playerScale = 1f;
    public bool isBusy = false;
    [Header("Points")]
    public Transform sitPoint;


    public InteractableHoverResponse GetHoverResponse(IInteractor interactor)
    {
        if (isBusy)
            return InteractableHoverResponse.None;

        return InteractableHoverResponse.Enable;
    }
    public bool CanInteract(IInteractor interactor)
    {
        Player player = interactor as Player;

        return !isBusy && player.currentScale == playerScale;
    }
    public void OnInteract(IInteractor interactor)
    {
        if (isBusy)
            return;

        player = interactor as Player;
        StartCoroutine(WormholingSequence());
    }

    IEnumerator WormholingSequence()
    {
        isBusy = true;
        linkedBox.isBusy = true;

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

    void ChangeScale()
    {
        player.SetScale(linkedBox.playerScale);
    }
}
