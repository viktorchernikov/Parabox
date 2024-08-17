using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Player cinematic sequence", menuName = "Warehouse/Player Cinematic Sequence", order = 1)]
public class PlayerCinematicSeqauence : ScriptableObject
{
    public float duration = 1f;
    public GameObject sequence;
    public AnimatorController animator;
    public Vector3 cameraStartViewAngles = Vector3.zero;
}
