using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(CharacterController))]
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
    //CharacterController m_chaController;
    //用于获取摄像机tranform组件
    public Transform m_camTransform;
    //用于获取摄像机的角度
    Vector3 m_camRot;
    //用于控制摄像机的高度
    float m_camHigh = 1f;


    protected NavMeshAgent nav;

    Vector3 moveTo;
    private void Awake()
    {
        m_tranform = this.transform;
        nav = transform.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        //m_chaController = GetComponent<CharacterController>();
        //获取主摄像机的transform组件
        //m_camTransform = Camera.main.transform;
        m_camTransform.position = m_tranform.TransformPoint(0, m_camHigh, 0);
        m_camTransform.rotation = m_tranform.rotation;
        m_camRot = m_tranform.eulerAngles;
    }
    void Update()
    {

        Control();

    }
    private void FixedUpdate()
    {
        if(moveTo!=Vector3.zero)
        {
            //nav.isStopped = true;
        }
    }
    //用来控制角色移动
    void Control()
    {
        //控制主角移动代码
        //moveTo = transform.position.y*Vector3.up;
        moveTo = transform.position.y*Vector3.zero;

        moveTo.x = Input.GetAxis("Horizontal") * Time.deltaTime * m_speed;
        moveTo.z = Input.GetAxis("Vertical") * Time.deltaTime * m_speed;


        transform.Translate(moveTo);

        transform.Rotate(Vector3.up, r_speed * Input.GetAxis("QE") * Time.deltaTime, Space.World);
        if (Input.GetMouseButton(1))
        {
            CameraRotator();
        }
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
        //Vector3 camrot = m_camTransform.eulerAngles;
        //camrot.x = 0;
        //camrot.z = 0;

        m_tranform.eulerAngles = m_camTransform.eulerAngles.y * Vector3.up;
        //m_tranform.eulerAngles = camrot; //仅仅只需要让主角的面朝向相机的方向就行了，不用旋转别的方向
        //PlayerRota(m_camTransform.eulerAngles);
        //保持主角摄像机在上方
        m_camTransform.position = transform.TransformPoint(0, m_camHigh, 0);

        m_camTransform.eulerAngles = m_camRot; //旋转摄像机

        
    }

    public void playerLookAt(Transform transform)
    {
        transform.LookAt(transform);
    }

    public void PlayerRota(Vector3 vector3)
    {
       // m_tranform.eulerAngles = vector3.y*Vector3.up; //仅仅只需要让主角的面朝向相机的方向就行了，不用旋转别的方向
    }
    /// <summary>
    /// 玩家到达某个区域
    /// </summary>
    public void Arrvied()
    {

    }
    /// <summary>
    /// 目标地点
    /// </summary>
    /// <param name="transform"></param>
    public void FlipTo(Vector3 target)
    {
        nav.isStopped = false;
        //StopAllCoroutines();
        //AI目标点
        nav.destination = target;
        nav.SetDestination(target);
        print(nav.destination);
        //StopAllCoroutines();
        StartCoroutine(WaitForDestination());
    }
    private IEnumerator WaitForDestination()
    {
        while (nav.pathPending)
        {
            yield return null;
        }

        while (nav.remainingDistance - 0.5f > nav.stoppingDistance)
        {

            yield return null;
        }
        nav.isStopped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Point")
        {
            if (other.GetComponent<Point>().stepType == GameManager.Instance.PlayerSetpType)
            {
                nav.isStopped = true;
                GameManager.Instance.StepGeneralMethod(other.GetComponent<Point>().stepType);

            }
        }
    }
}
