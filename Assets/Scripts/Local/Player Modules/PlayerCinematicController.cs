using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerCinematicController : PlayerModule
{
    [SerializeField] AnimationCurve adjustPosAndVeCurve;


    private void Start()
    {
        
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
}
