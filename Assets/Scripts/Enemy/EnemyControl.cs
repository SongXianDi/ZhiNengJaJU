using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyControl : MonoBehaviour
{

    /// <summary>
    /// 注册所有状态
    /// </summary>
    public enum StateType
    {
        Move,
        Rotate,
    }

    public float moveSpeed = 2f;
    public float idleTime = 3f;

    private Animator _animator;
    protected NavMeshAgent nav;

    public float timer = 0f;

    //声明当前状态
    private StateType currentState;

    public GameObject zhuchi;
    public GameObject LuBiao;
    void Start()
    {
        zhuchi = GameObject.FindGameObjectWithTag("Enemy");
    }
    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        currentState = StateType.Rotate;
    }

    /// <summary>
    /// 目标地点
    /// </summary>
    /// <param name="transform"></param>
    public virtual void FlipTo(Vector3 target)
    {
        nav.isStopped = false;
        GetComponent<Animator>().SetBool("New Bool", true);
        //StopAllCoroutines();
        //AI目标点
        nav.destination = target;
        nav.SetDestination(target);

        if (currentState == StateType.Move && IsArrward(target))
        {
            currentState = StateType.Rotate;
        }
        StopAllCoroutines();
        StartCoroutine(WaitForDestination());
    }
    private IEnumerator WaitForDestination()
    {
        while (nav.pathPending)
        {
            yield return null;
        }

        while (nav.remainingDistance-0.5f > nav.stoppingDistance)
        {

            yield return null;
        }
        nav.isStopped = true;
        // 到达目的地后触发的回调函数
        _animator.SetBool("New Bool", false);
        StartCoroutine(Rotate());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="vector3"></param>
    /// <param name="action"> 导航移动方法</param>
    /// <returns></returns>
    IEnumerator RotateTo(Vector3 point, UnityAction<Vector3> action)
    {
        float t = 0;
        while (true)
        {
            if (currentState == StateType.Rotate)
            {
                t += Time.deltaTime * 0.2f; ;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.forward, point - transform.position), t);
                if (t > 1)
                {
                    currentState = StateType.Move;
                    action.Invoke(point);
                }
            }
            yield return null;
        }

    }

    private bool IsArrward(Vector3 vector3)
    {
        if (Vector3.Distance(transform.position, vector3) < 5f)
        {
            return true;
        }
        else
            return false;
    }

    public void Setup(float speed, float angle)
    {
        float angularSpeed = angle / 0.6f;
        //敌人速度平滑转变传给动画控制器
        _animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        _animator.SetFloat("AngularSpeed", angularSpeed, 0.7f, Time.deltaTime);
    }

    //获取角度
    public float FindAngle(Vector3 toVector)
    {
        if (toVector == Vector3.zero)
        {
            return 0;
        }
        float angle = Vector3.Angle(transform.forward, toVector);
        Vector3 noemal = Vector3.Cross(transform.forward, toVector);//法向量
        angle *= Mathf.Sign(Vector3.Dot(noemal, transform.up));
        angle *= Mathf.Deg2Rad;
        if (Mathf.Abs(angle) < 5f * Mathf.Deg2Rad)
        {
            transform.LookAt(transform.position + toVector);
            angle = 0f;

        }
        return angle;
    }

    private IEnumerator Rotate()
    {
        timer = 0;
        while (timer * 0.5f < 1)
        {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(zhuchi.transform.position - transform.position), timer * 0.5f);
            yield return null;
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }


}


