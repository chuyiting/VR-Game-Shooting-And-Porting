using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour {

    public GameObject m_Pointer;
    public SteamVR_Action_Boolean m_TeleportAction;
    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool m_HasPosition = false;
    private bool m_IsTeleporting = false;
    private float m_FadeTime = 0.5f;


    private void Awake() {
        Debug.Log("awake");
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update() {
        // Pointer 
        m_HasPosition = UpdatePointer();
        Vector3 currPos = m_Pointer.transform.position;
       
        m_Pointer.SetActive(m_HasPosition);

        //Teleport
        if (m_TeleportAction.GetStateUp(m_Pose.inputSource)) {
            TryTeleport();
        }
    }

    private void  TryTeleport() {
        Debug.Log("try teleportation");
        if (!m_HasPosition || m_IsTeleporting) {
            return;
        }

        Debug.Log(SteamVR_Render.Top() == null);
        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 headPosition = SteamVR_Render.Top().head.position;

        Vector3 groundPosition = new Vector3(headPosition.x, cameraRig.position.y, headPosition.z);
        Vector3 translateVector = m_Pointer.transform.position - groundPosition;

        StartCoroutine(MoveRig(cameraRig, translateVector));
    }

    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation) {
        m_IsTeleporting = true;

        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        yield return new WaitForSeconds(m_FadeTime);
        cameraRig.position += translation;

        // Fade to clear
        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        m_IsTeleporting = false;

        yield return null;
    }

    private bool UpdatePointer() {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            m_Pointer.transform.position = hit.point;
            return true;
        }

        return false;
    }
}