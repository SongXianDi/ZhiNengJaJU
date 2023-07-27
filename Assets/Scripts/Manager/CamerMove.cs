using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerMove : MonoBehaviour
{
    //角色的移动速度
    public float m_speed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= new Vector3(0,m_speed,0) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.position += new Vector3(0, m_speed, 0) * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(m_speed, 0, 0) * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(m_speed, 0, 0) * Time.deltaTime;
        }
    }
}
