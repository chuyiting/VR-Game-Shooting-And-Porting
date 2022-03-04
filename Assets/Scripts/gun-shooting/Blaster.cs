using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    public GameObject m_ProjectilePrefab = null;
    private ProjectilePool m_ProjectilePool = null;

    private void Awake()
    {
        m_ProjectilePool = new ProjectilePool(m_ProjectilePrefab, 10);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
