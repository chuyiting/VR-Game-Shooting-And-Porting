using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("on collision enter");
        if (collision.gameObject.tag == "Target")
        {
            collision.gameObject.SendMessageUpwards("ApplyDamage", SendMessageOptions.DontRequireReceiver);
        }
    }
}
