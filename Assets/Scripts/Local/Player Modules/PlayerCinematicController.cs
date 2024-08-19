using System;
using System.Collections;
using UnityEngine;

public class PlayerCinematicController : PlayerModule
{
    public bool isPlaying { get; private set; } = false;
    [Header("Cinematics")]
    [SerializeField] PlayerCinematicSeqauence wormJumpinCinematic;
    [SerializeField] PlayerCinematicSeqauence wormJumpoutCinematic;
    [Header("Tweaks")]
    [SerializeField] AnimationCurve adjustPosAndVeCurve;

    public event Action onWormholeTeleport;


    private void Start()
    {

    }
    public void PlayWormholeJumpin(Wormbox box)
    {
        StartCoroutine(WormboxSequence(box));
    }

    #region Sequences
    IEnumerator WormboxSequence(Wormbox box)
    {
        isPlaying = true;
        parent.SetDuringCinematic(true);

        Vector3 playerToBoxDir = box.sitPoint.position - parent.usedRigidbody.position;
        Quaternion playerToBoxAng = Quaternion.LookRotation(playerToBoxDir) * Quaternion.Euler(wormJumpinCinematic.cameraStartViewAngles);
        float playerToBoxYaw = playerToBoxAng.eulerAngles.y;
        Vector3 playerToBoxPos = (Quaternion.Euler(0, playerToBoxYaw, 0) * Vector3.back * 3 * parent.currentScale) + box.sitPoint.position;
        Vector3 cameraToBoxPos = playerToBoxPos + Vector3.up * parent.cameraAnchor.localPosition.y;

        yield return AdjustCameraPositionAndViewAngles(cameraToBoxPos, playerToBoxAng.eulerAngles, 1f, 30f);

        float flapsCloseDelay = 1.5f;
        StartCoroutine(PlaySequence(wormJumpinCinematic, playerToBoxPos, Quaternion.identity * Quaternion.Euler(0, playerToBoxYaw, 0), Vector3.one * parent.currentScale));
        yield return new WaitForSeconds(flapsCloseDelay);
        box.CloseFlaps();
        box.linkedBox.CloseFlaps();
        yield return new WaitForSeconds(wormJumpinCinematic.duration - flapsCloseDelay);

        onWormholeTeleport?.Invoke();

        box.OpenFlaps();
        box.linkedBox.OpenFlaps();
        yield return PlaySequence(wormJumpoutCinematic, box.linkedBox.sitPoint.position, Quaternion.identity * Quaternion.Euler(0, box.linkedBox.transform.eulerAngles.y, 0), Vector3.one * box.linkedBox.playerScale, true);

        parent.SetDuringCinematic(false);
        isPlaying = false;
    }
    #endregion

    #region Helpers
    IEnumerator PlaySequence(PlayerCinematicSeqauence cinematic, Vector3 pos, Quaternion rot, Vector3 scale, bool teleportEndPlayer = false)
    {
        GameObject sequence = Instantiate(cinematic.sequence, pos, rot);
        sequence.transform.localScale = scale;

        yield return null;

        Transform camera_anchor = sequence.transform.GetChild(0).GetChild(0);
        float time = 0f;
        while (time <= cinematic.duration)
        {
            parent.usedCamera.SetPosition(camera_anchor.position);
            parent.usedCamera.SetViewAngles(camera_anchor.eulerAngles);
            time += Time.deltaTime;
            yield return null;
        }
        if (teleportEndPlayer)
        {
            Transform player_end_anchor = sequence.transform.GetChild(0).GetChild(1);
            parent.Teleport(player_end_anchor.position);
            parent.usedCamera.SetPosition(camera_anchor.position);
        }
        parent.usedCamera.SetViewAngles(camera_anchor.eulerAngles);
        Destroy(sequence);
    }
    IEnumerator AdjustCameraPositionAndViewAngles(Vector3 desiredPosition, Vector3 desiredViewAngles, float moveStep = 1f, float lookStep = 10f)
    {
        Quaternion desiredRotation = Quaternion.Euler(desiredViewAngles);
        float initialDistance = Vector3.Distance(parent.usedCamera.position, desiredPosition);
        float initialAngleDifference = Quaternion.Angle(Quaternion.Euler(parent.usedCamera.viewAngles), desiredRotation);


        while ((Vector3.Distance(parent.usedCamera.position, desiredPosition) >= float.Epsilon) || (Quaternion.Angle(Quaternion.Euler(parent.usedCamera.viewAngles), desiredRotation) >= 0.05f))
        {
            float posCompletePercentage = 1f - (Vector3.Distance(parent.usedCamera.position, desiredPosition)) / initialDistance;
            float rotCompletePercentage = 1f - (Quaternion.Angle(Quaternion.Euler(parent.usedCamera.viewAngles), desiredRotation) / initialAngleDifference);

            parent.usedCamera.SetPosition(Vector3.MoveTowards(parent.usedCamera.position, desiredPosition, moveStep * adjustPosAndVeCurve.Evaluate(posCompletePercentage) * Time.deltaTime));
            parent.usedCamera.SetViewAngles(Quaternion.RotateTowards(Quaternion.Euler(parent.usedCamera.viewAngles), desiredRotation, lookStep * adjustPosAndVeCurve.Evaluate(rotCompletePercentage) * Time.deltaTime).eulerAngles);
            
            Debug.Log($"{parent.usedCamera.position}, {parent.usedCamera.viewAngles}");
            
            yield return null;
        }
        parent.usedCamera.SetPosition(desiredPosition);
        parent.usedCamera.SetViewAngles(desiredViewAngles);
    }
    #endregion
}
