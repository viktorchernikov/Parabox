using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerCinematicController : PlayerModule
{
    [Header("Cinematics")]
    [SerializeField] PlayerCinematicSeqauence wormJumpinCinematic;
    [SerializeField] PlayerCinematicSeqauence wormJumpoutCinematic;
    [Header("Tweaks")]
    [SerializeField] AnimationCurve adjustPosAndVeCurve;


    private void Start()
    {
        PlayWormholeJumpin(parent.usedRigidbody.position, 0);
    }
    public void PlayWormholeJumpin(Vector3 startPlayerPos, float startPlayerYaw)
    {
        StartCoroutine(WormboxSequence(startPlayerPos, startPlayerYaw));
    }

    #region Sequences
    IEnumerator WormboxSequence(Vector3 startPlayerPos, float startPlayerYaw)
    {
        parent.SetDuringCinematic(true);
        yield return PlaySequence(wormJumpinCinematic, startPlayerPos, Quaternion.identity);
        yield return PlaySequence(wormJumpoutCinematic, startPlayerPos, Quaternion.identity, true);
        parent.SetDuringCinematic(false);
    }
    #endregion

    #region Helpers
    IEnumerator PlaySequence(PlayerCinematicSeqauence cinematic, Vector3 pos, Quaternion rot, bool teleportEndPlayer = false)
    {
        GameObject sequence = Instantiate(cinematic.sequence, pos, rot);
        Animator anim = sequence.AddComponent<Animator>();
        anim.runtimeAnimatorController = cinematic.animator;

        yield return null;

        Transform camera_anchor = sequence.transform.GetChild(0);
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
            Transform player_end_anchor = sequence.transform.GetChild(1);
            parent.Teleport(player_end_anchor.position);
            parent.usedCamera.SetPosition(camera_anchor.position);
        }
        yield return null;

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
