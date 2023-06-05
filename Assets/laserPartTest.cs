using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserPartTest : MonoBehaviour
{

    private DataManager m_DataManager;

    // Start is called before the first frame update
    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            m_DataManager.LaserManager.PrintLaserPart();
        }
    }
}
