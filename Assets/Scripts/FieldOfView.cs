using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject playerRef = null;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
                    yield return wait;
                    FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {   
        // check collision in target mask
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = null;
            foreach (Collider c in rangeChecks)
            {
                if (c.gameObject.tag == "Player")
                {
                    target = c.transform;
                }
            }

            if (target == null)
            {
                Debug.Log("no player");
                canSeePlayer = false;
                return;
            }

            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // check if any obstruction on the view
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else 
                {
                    canSeePlayer = false;
                    Debug.Log("view obstruct");
                }
            }
            else 
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
