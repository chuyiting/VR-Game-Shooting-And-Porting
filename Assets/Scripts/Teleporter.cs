using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour {

    public GameObject m_Pointer;
    public Material pointerMatOk;
    public Material pointerMatNotOk;
    public LayerMask teleporationMask;
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
       
        if (m_HasPosition)
        {
            m_Pointer.GetComponentInChildren<MeshRenderer>().material = pointerMatOk;
        }
        else 
        {
            m_Pointer.GetComponentInChildren<MeshRenderer>().material = pointerMatNotOk;
        }

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
        RaycastHit hit, block;
        bool hitTeleportPoint = Physics.Raycast(transform.position, transform.forward, out hit, 30f, teleporationMask);
        bool hitOtherPoint = Physics.Raycast(transform.position, transform.forward, out block, 30f, ~teleporationMask);

        if (hitTeleportPoint) 
        {
            m_Pointer.SetActive(true);
            if (hitOtherPoint && Vector3.Distance(transform.position, hit.point) > Vector3.Distance(transform.position, block.point))
            {
                m_Pointer.transform.position = block.point;
                m_Pointer.transform.rotation = Quaternion.FromToRotation(m_Pointer.transform.up, block.normal) * m_Pointer.transform.rotation;
                return false;
            }
            m_Pointer.transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
            m_Pointer.transform.rotation = Quaternion.FromToRotation(m_Pointer.transform.up, hit.normal) * m_Pointer.transform.rotation;
            return true;
        }

        if (hitOtherPoint)
        {
            m_Pointer.SetActive(true);
            m_Pointer.transform.position = block.point;
            m_Pointer.transform.rotation = Quaternion.FromToRotation(m_Pointer.transform.up, block.normal) * m_Pointer.transform.rotation;
            return false;
        }

        m_Pointer.SetActive(false);

        return false;
    }
}