using UnityEngine;

public class MainCamera : MonoBehaviour {

    Portal[] portals;

    void Awake () {
        portals = FindObjectsOfType<Portal> ();
    }

    bool camInit = false;
    void Update() {
        if (camInit) return;
        GetComponent<Camera>().enabled = true;
    }

    void OnPreCull () {

        for (int i = 0; i < portals.Length; i++) {
            portals[i].PrePortalRender ();
        }
        for (int i = 0; i < portals.Length; i++) {
            portals[i].Render ();
        }

        for (int i = 0; i < portals.Length; i++) {
            portals[i].PostPortalRender ();
        }

    }

}