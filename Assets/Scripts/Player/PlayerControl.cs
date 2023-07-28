using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    //角色获取Player的Transform组件
    public Transform m_tranform;
    //角色的移动速度
    public float m_speed = 3.0f;
    public float r_speed = 3.0f;
    //重力
    public float m_gravity = 2.0f;
    //用来获取角色控制组件
    CharacterController m_chaController;
    //用于获取摄像机tranform组件
    public Transform m_camTransform;
    //用于获取摄像机的角度
    Vector3 m_camRot;
    //用于控制摄像机的高度
    float m_camHigh = 1f;

    Vector3 moveTo;
    private void Awake()
    {
        m_tranform = this.transform;
    }
    void Start()
    {
        m_chaController = GetComponent<CharacterController>();
        //获取主摄像机的transform组件
        //m_camTransform = Camera.main.transform;
        m_camTransform.position = m_tranform.TransformPoint(0, m_camHigh, 0);
        m_camTransform.rotation = m_tranform.rotation;
        m_camRot = m_tranform.eulerAngles;
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            CameraRotator();
        }
        Control();
    }
    private void FixedUpdate()
    {

    }
    //用来控制角色移动
    void Control()
    {
        //控制主角移动代码
        moveTo = Vector3.zero;

        moveTo.x = Input.GetAxis("Horizontal") * Time.deltaTime * m_speed;
        moveTo.z = Input.GetAxis("Vertical") * Time.deltaTime * m_speed;

        m_camRot.y += r_speed * Input.GetAxis("QE") * Time.deltaTime;
        m_camTransform.transform.Rotate(Vector3.up, r_speed * Input.GetAxis("QE") * Time.deltaTime, Space.World);
        m_camTransform.eulerAngles = m_camRot; //旋转摄像机
        moveTo.y -= m_gravity * Time.deltaTime;

        m_chaController.Move(m_tranform.TransformDirection(moveTo));
    }
    //视角旋转
    private void CameraRotator()
    {
        //控制摄像机代码
        float rh = Input.GetAxis("Mouse X") * 1.5f;
        float rv = Input.GetAxis("Mouse Y") * 1.5f;
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camRot.x = Mathf.Clamp(m_camRot.x, -40, 40);
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0;
        camrot.z = 0;
        m_tranform.eulerAngles = camrot; //仅仅只需要让主角的面朝向相机的方向就行了，不用旋转别的方向

        //保持主角摄像机在上方
        m_camTransform.position = transform.TransformPoint(0, m_camHigh, 0);

        m_camTransform.eulerAngles = m_camRot; //旋转摄像机
    }

    public void playerLookAt(Transform transform)
    {
        transform.LookAt(transform);
    }

    /// <summary>
    /// 玩家到达某个区域
    /// </summary>
    public void Arrvied()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Point")
        {
            if (other.GetComponent<Point>().stepType == GameManager.Instance.PlayerSetpType)
            {

                GameManager.Instance.StepGeneralMethod(other.GetComponent<Point>().stepType);

            }
        }
    }
}
