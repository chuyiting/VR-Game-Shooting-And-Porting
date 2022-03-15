using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("bullet hit something");
        Debug.Log(collision.gameObject.tag);;
        if (collision.gameObject.tag == "Target")
        {
            Debug.Log("hit enemy");
            collision.gameObject.SendMessageUpwards("ApplyDamage", collision);
            Destroy(gameObject, 0.1f); // destroy the bullet after
        } 
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessageUpwards("ApplyDamage", collision);
            Destroy(gameObject, 0.1f); // destroy the bullet after
        }
    }
}
