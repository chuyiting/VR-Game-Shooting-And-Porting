using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public SteamVR_Actions_Vector2 input;
    public float speed = 1;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // avoid confusion between teleportation
        // if (input.axis.magnitude > 0.1f)
        // {
        //     // turn face to the moving direction
        //     Vector3 direction = PlayerController.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        //     characterController.move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction.Vector3.Up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
        // }

    }
}
