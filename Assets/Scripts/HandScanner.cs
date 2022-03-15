using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandScanner : MonoBehaviour
{
    public Loader loader;
    public Gate gate;
    private Interactable interactable;
    private void Awake() {
        interactable = GetComponent<Interactable>();
        //loader.gameObject.SetActive(false);
    }
    
    private void OnHandHoverBegin(Hand hand)
    {
        //loader.gameObject.SetActive(true);
        loader.StartLoading();
    }

    private void OnHandHoverEnd(Hand hand)
    {
        if (loader.completness == 1.0f)
        {
            return;
        }
        loader.StopLoading();
        //loader.gameObject.SetActive(false);
    }


    private void HandHoverUpdate(Hand hand)
    {
        if (loader.completness >= 1.0f)
        {
            gate.Open();
        }
    }

    public void OnLoadingComplete()
    {
        gate.Open();
    }
}
