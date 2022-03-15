using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Gun : MonoBehaviour
{
    public SteamVR_Action_Boolean fireAction;
    public GameObject bullet;
    public Transform target = null;
    public Transform barrelPivot;
    public Transform traceOrigin;
    public float shootingSpeed = 40;
    public GameObject muzzleFlash;
    public bool showTrace = true;
    public bool isEnemy = false;
    public GameObject trace;
    public float gunRange = 50f;
    public float traceDuration = 0.2f;

    public int maxbullets = 10;
    private int currentbullets;

    public TMPro.TextMeshPro bulletPrompt;
    public bool showBulletPrompt;

    public AudioSource audioSource;
    public AudioClip fire;
    public AudioClip reload;
    public AudioClip nobullet;

    private Animator animator;
    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        if (showBulletPrompt)
        {
            currentbullets = maxbullets;
            bulletPrompt.text = currentbullets.ToString();
        } 
        else 
        {
            Destroy(bulletPrompt);
        }
    
        animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemy) 
        {
            return;
        }


        // fire
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;

            if (fireAction[source].stateDown)
            {
                Fire();
            }
        }

        // reload
        if (Vector3.Angle(transform.up, Vector3.up) > 100 && currentbullets < maxbullets)
        {
            Reload();
        }
    }

    public IEnumerator DelayFire(float delay)
    {
         yield return new WaitForSeconds(delay);
         Fire();
    }

    public void Fire()
    {   
        if (showBulletPrompt && currentbullets == 0)
        {
            audioSource.PlayOneShot(nobullet);
            return;
        }

        audioSource.PlayOneShot(fire);
        // instantiate bullet
        GameObject bulletInstance = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation);
        foreach (Rigidbody rb in bulletInstance.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 direction = barrelPivot.forward;
            if (target != null)
            {
                direction = (target.position - barrelPivot.position).normalized;
            }
            rb.AddForce(direction * shootingSpeed);
        }

        // update UI
        if (showBulletPrompt)
        {
            currentbullets--;
            bulletPrompt.text = currentbullets.ToString();
        }

        // draw trace
        if (showTrace)
        {
            GameObject traceInstance = Instantiate(trace);
            LineRenderer traceRenderer = traceInstance.GetComponent<LineRenderer>();
            traceRenderer.SetPosition(0, traceOrigin.position); // set the beginning of the trace to the trace origin
            traceRenderer.SetPosition(1, traceOrigin.position + barrelPivot.forward * gunRange);
            traceRenderer.enabled = true;
            Destroy(traceInstance, traceDuration);
            Destroy(bulletInstance, 5.0f);
        } 
        else 
        {
            Destroy(bulletInstance, 1.0f);
        }
        
        
        Instantiate(muzzleFlash, barrelPivot.position, barrelPivot.rotation);
    }

    void Reload()
    {
        currentbullets = maxbullets;
        audioSource.PlayOneShot(reload);
        
        // update UI
        if (showBulletPrompt)
        {
            bulletPrompt.text = currentbullets.ToString();
        }
    }
}
