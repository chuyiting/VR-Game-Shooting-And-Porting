using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MyTeleport : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePointer();
        m_Pointer.SetActive(m_HasPosition);
    }

    private bool UpdatePointer() {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            Debug.Log("good");
            m_Pointer.transform.position = hit.point;
            return true;
        }

        return false;
    }

}
