using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HigherJump : MonoBehaviour
{
    float lastJumpForce = -1f;
    public float newJumpForce = 10f;

    void OnTriggerEnter(Collider collider) {
        Player player = null;
        try {
            player = collider.GetComponentInParent<Player>();
        } catch { return; }

        if (player == null) return;

        if (lastJumpForce < 0f) lastJumpForce = player.GetModule<PlayerGroundMotor>().jumpForce;
        player.GetModule<PlayerGroundMotor>().jumpForce = newJumpForce;
    }

    void OnTriggerExit(Collider collider) {
        Player player = null;
        try {
            player = collider.GetComponentInParent<Player>();
        } catch { return; }

        if (player == null) return;

        if (lastJumpForce > 0f) player.GetModule<PlayerGroundMotor>().jumpForce = lastJumpForce;
        lastJumpForce = -1f;
    }
}
