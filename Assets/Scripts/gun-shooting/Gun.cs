using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Gun : MonoBehaviour
{
    public SteamVR_Action_Boolean fireAction;
    public GameObject bullet;
    public Transform barrelPivot;
    public float shootingSpeed = 40;
    public GameObject muzzleFlash;

    private Animator animator;
    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if grabbed
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;

            if (fireAction[source].stateDown)
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        Debug.Log("fire");
        GameObject bulletInstance = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation);
        foreach (Rigidbody rb in bulletInstance.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(barrelPivot.forward * shootingSpeed);
        }
        
        Instantiate(muzzleFlash, barrelPivot.position, barrelPivot.rotation);
    }
}
