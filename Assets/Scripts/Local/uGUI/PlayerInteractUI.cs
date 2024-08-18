using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    #region Components
    [Header("Components")]
    [SerializeField] GameObject containerInteractUI;
    [SerializeField] TextMeshProUGUI textInteract;
    #endregion

    private void FixedUpdate()
    {
        PlayerInteractionSeeker seeker = Player.local.GetModule<PlayerInteractionSeeker>();
        containerInteractUI.SetActive(false);

        if (seeker.hoveredObject != null)
        {
            InteractableHoverResponse res = seeker.hoveredObject.GetHoverResponse(Player.local);

            if (res != InteractableHoverResponse.None)
            {
                bool CanInteract = seeker.hoveredObject.CanInteract(Player.local);

                if (CanInteract)
                {
                    if (res == InteractableHoverResponse.Enable)
                    {
                        textInteract.text = "Press (E) to disable";
                    }
                    else
                    {
                        textInteract.text = "Press (E) to enable";
                    }
                    containerInteractUI.SetActive(true);
                }
                else
                {
                    containerInteractUI.SetActive(false);
                }
            }
        }
    }
}
