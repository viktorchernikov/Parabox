using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PneumaticDoor : MonoBehaviour
{
    public GameObject door;
    public Transform closedAnchor;
    public Transform openAnchor;
    public bool isOpen = false;
    public bool isBusy = false;
    public float openingAnimTime = 1f;
    public float closingAnimTime = 4f;

    public void Open() {
        if (isBusy) return;

        StartCoroutine(OpeningSequence());
    }

    public void Close() {
        if (isBusy) return;

        StartCoroutine(ClosingSequence());
    }

    float easeOutExpo(float x) {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }

    double easeOutBounce(double x) {
        const double n1 = 7.5625f;
        const double d1 = 2.75f;

        if (x < 1 / d1) {
            return 1 - (n1 * x * x);
        } else if (x < 2 / d1) {
            return 1 - (n1 * (x -= 1.5 / d1) * x + 0.75);
        } else if (x < 2.5 / d1) {
            return 1 - (n1 * (x -= 2.25 / d1) * x + 0.9375);
        } else {
            return 1 - (n1 * (x -= 2.625 / d1) * x + 0.984375);
        }
    }

    float timePassed = 0f;
    IEnumerator OpeningSequence() {
        if (!isBusy) {
            timePassed = 0f;
            isBusy = true;

            yield return null;
            while (!isOpen) {
                door.transform.position = Vector3.Lerp(closedAnchor.position, openAnchor.position, easeOutExpo(timePassed / openingAnimTime));
                if (timePassed >= openingAnimTime) {
                    door.transform.position = openAnchor.position;
                    isOpen = true;
                }

                yield return null;
                timePassed += Time.deltaTime;
            }

            isBusy = false;
        }
    }

    IEnumerator ClosingSequence() {
        if (!isBusy) {
            timePassed = 0f;
            isBusy = true;

            yield return null;
            while (isOpen) {
                door.transform.position = Vector3.Lerp(closedAnchor.position, openAnchor.position, (float)easeOutBounce(double.Parse((timePassed).ToString())));
                if (timePassed >= openingAnimTime) {
                    door.transform.position = closedAnchor.position;
                    isOpen = false;
                }

                yield return null;
                timePassed += Time.deltaTime;
            }

            isBusy = false;
        }
    }
}
