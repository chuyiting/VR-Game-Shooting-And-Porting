using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject bsplt;

    private void ApplyDamage(Collision dmgPoint)
    {
        bsplt.transform.parent = dmgPoint.transform;
        Quaternion quaternion = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        GameObject blood = Instantiate(bsplt, dmgPoint.contacts[0].point, quaternion);
        Destroy(blood, 3.0f);
    }

}
