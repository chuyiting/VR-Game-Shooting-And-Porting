using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SimpleAttach : MonoBehaviour
{
    Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        hand.ShowGrabHint();
    }

    private void OnHandHoverEnd(Hand hand)
    {
        hand.HideGrabHint();
    }

    private void OnHandHoverUpdate(Hand hand)
    {
        GrabTypes grabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);

        // GRAB THE OBJECT
        if (interactable.attachedToHand == null && grabType != GrabTypes.None)
        {
            hand.AttachObject(gameObject, grabType);
            hand.HoverLock(interactable); // disable hand hover when an object is grabbed
            hand.HideGrabHint();
        }
        // RELEASE
        else
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
        }

    }

}
