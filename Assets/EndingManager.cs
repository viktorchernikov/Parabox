using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EndingManager : MonoBehaviour
{
    [SerializeField] UnityEvent onEndingSetOff;
    public AudioSource musicSource;

    public GameObject uiCanvas;
    public GameObject[] uistages;

    void OnTriggerEnter(Collider collider) {
        Player player = null;
        try {
            player = collider.GetComponentInParent<Player>();
        }
        catch {
            return;
        }

        SetOffEnding(player);
    }

    void SetOffEnding(Player player) {
        onEndingSetOff?.Invoke();

        player.GetModule<PlayerCameraController>().canLook = false;
        player.GetModule<PlayerGroundMotor>().canMove = false;

        StartCoroutine(Music());
        StartCoroutine(UI());
    }

    IEnumerator Music() {
        yield return new WaitForSeconds(1f);
        musicSource.Play();
        float timePassed = 0f;
        while (timePassed < 1f) {
            musicSource.volume = Mathf.Min(timePassed, 1f);
            yield return null;
            timePassed += Time.deltaTime;
        }
    } 

    IEnumerator UI() {
        float timePassed = 0f;
        while (timePassed < 26f) {
            if (timePassed > 17f) {
                SetUIStagesActive(4);
            }
            else if (timePassed > 11f) {
                SetUIStagesActive(3);
            }
            else if (timePassed > 9f) {
                SetUIStagesActive(2);
            }
            else if (timePassed > 5f) {
                SetUIStagesActive(1);
            }
            else if (timePassed > 1f) {
                SetUIStagesActive(0);
                uiCanvas.SetActive(true);
            }

            yield return null;
            timePassed += Time.deltaTime;
        }

        FindFirstObjectByType<LoadingMenuManager>().LoadScene("Menu");
    }

    void SetUIStagesActive(int index) {
        for (int f = 0; f < uistages.Length; f++) {
            uistages[f].SetActive(f == index);
        }
    }
}
