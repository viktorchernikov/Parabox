using UnityEngine;

public class PlayerScaleDebugging : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.O))
        {
            Player.local.SetScale(Player.local.currentScale + 0.2f);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Player.local.SetScale(Player.local.currentScale - 0.2f);
        }
#endif
    }
}
