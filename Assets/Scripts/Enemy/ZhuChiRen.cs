using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ZhuChiRen : EnemyControl
{
    private GameObject tiShi;
    private HighlightPlus.HighlightEffect highlight;

    public EnemyControl GuanZ1;
    public EnemyControl GuanZ2;

    public GameObject TiShi { get => tiShi; set => tiShi = value; }

    private Animator animator;
    public UnityAction action;
    private Transform player;
    public GameObject TimeLine;
    private bool isOne = true;

    //private UnityTask GetThread;
    public Transform[] machinePoints = new Transform[10];
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        tiShi = transform.Find("ThiShi").gameObject;
        highlight = tiShi.GetComponent<HighlightPlus.HighlightEffect>();
        //highlight.OnObjectHighlightEnd
    }
    public override void FlipTo(Vector3 target)
    {
        ResetStatus();
        nav.isStopped = false;
        animator.SetBool("iswalk", true);
        animator.SetBool("Proteak", false);
        nav.destination = target;
        nav.SetDestination(target);
        GuanZ1?.FlipTo(target + Vector3.left * 1.5f);
        GuanZ2?.FlipTo(target + Vector3.right * 1.5f);
        StartCoroutine(WaitForDestination(false));
        //StopAllCoroutines();
    }
    /// <summary>
    /// 重置状态
    /// </summary>
    public void ResetStatus()
    {
        StopAllCoroutines();
        AudioManage.Instance.PlayMusicSourceAnimator(null, null, null);
        animator.SetBool("istalk", false);
    }
    public void FlipTo2(Vector3 target)
    {
        nav.isStopped = false;
        // animator.SetBool("iswalk", true);
        //animator.SetBool("Proteak", false);
        nav.SetDestination(target);
    }

    public IEnumerator FlipTo3(Vector3 target)
    {

        yield return new WaitForSeconds(1f);
        animator.SetBool("iswalk", true);
        animator.SetBool("Proteak", false);
        FlipTo2(target);
        StartCoroutine(WaitForDestination(true));
    }

    public void TiShiShow(bool isShow)
    {
        tiShi.gameObject.SetActive(isShow);
        highlight.highlighted = isShow;
        GetComponent<CapsuleCollider>().enabled = isShow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isTranslate">是否是去转换点</param>
    /// <returns></returns>
    private IEnumerator WaitForDestination(bool isTranslate)
    {

        while (nav.pathPending)
        {
            //计算路径中...
            yield return null;
        }
        if (!isTranslate)
        {
            StartCoroutine(MoveHalf(nav.remainingDistance));
        }
        while (nav.remainingDistance > nav.stoppingDistance)
        {
            yield return null;
        }

        // 到达目的地后触发的回调函数
        OnReachedDestination(isTranslate);
    }
    private IEnumerator MoveHalf(float distance)
    {
        while (true)
        {
            if (nav.remainingDistance < distance / 2)
            {
                GameManager.Instance.ZCRArrviePonit();
                yield break;
            }
            yield return null;
        }
    }
    private IEnumerator Rotate()
    {
        timer = 0;
        while (timer * 0.5f < 1)
        {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position - Vector3.up * 0.6f), timer * 0.5f);
            yield return null;
        }
    }
    private void OnReachedDestination(bool isTranlate)
    {
        Debug.Log("Reached destination!");
        nav.isStopped = true;
        //animator.SetBool("New Bool", false);
        animator.SetBool("Proteak", true);
        animator.SetBool("iswalk", false);
        if (isTranlate)
        {
            OperationHintManager.Instance.ChangeText("请跟随引导人");
            LuBiao.transform.position = transform.position;
            LuBiao.SetActive(true);
            //GameManager.Instance.ZCRArrviePonit();
        }


        //面朝玩家
        //Mathf
        //transform.LookAt(player);

        //StopCoroutine(WaitForDestination());
        StartCoroutine(Rotate());
    }
    private void OnMouseUp()
    {
        TiShiShow(false);
        animator.SetBool("istalk", true);
        switch (GameManager.Instance.CurrentSetpType)
        {
            //开始步骤
            case StepType.NIL:
                TimeLine.SetActive(true);
                AudioManage.Instance.PlayMusicSource("欢迎辞", () => { TimeLine.SetActive(false); Arrive(); });
                break;
            //结束步骤
            case StepType.自动开料区:
                StartCoroutine(FlipTo3(machinePoints[(int)(GameManager.Instance.CurrentSetpType)].position));
                AudioManage.Instance.PlayMusicSourceAnimator("（" + ((int)GameManager.Instance.CurrentSetpType + 1).ToString() + "）", () =>
                {
                    //动画播放
                    animator.SetTrigger("HandUp");
                }, Last);
                break;
            //正常步骤
            default:
                StartCoroutine(FlipTo3(machinePoints[(int)(GameManager.Instance.CurrentSetpType)].position));
                //说话中播放动画
                AudioManage.Instance.PlayMusicSourceAnimator("（" + ((int)GameManager.Instance.CurrentSetpType + 1).ToString() + "）", () =>
                {
                    //动画播放
                    animator.SetTrigger("HandUp");
                }, Arrive);
                break;
        }

    }

    private async void Last()
    {
        animator.SetBool("istalk", false);
        if (GameManager.Instance.RealSetpType == StepType.End && isOne)
        {
            //isOne = false;
            animator.SetBool("istalk", true);
            await Task.Delay(2000);
            OperationHintManager.Instance.ChangeText("浏览结束，你也可以点击流程观看以前步骤");
            AudioManage.Instance.PlayMusicSource("结束语");
        }

    }

    public async void Arrive()
    {
        animator.SetBool("istalk", false);
        GameManager.Instance.gameControl.ZhuChiRenArrive();
        OperationHintManager.Instance.ChangeText("请点击流程按钮");
        await Task.Delay(1000);
        GameManager.Instance.ZhuChiRenMove(GameManager.Instance.CurrentSetpType + 1);
    }

}
