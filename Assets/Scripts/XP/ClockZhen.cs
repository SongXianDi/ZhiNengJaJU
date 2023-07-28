using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Events;

[Serializable]
public class UpdateTime : UnityEvent<float> { }
public class ClockZhen : MonoBehaviour, IPointerDownHandler,IDragHandler
{
    private RectTransform thisRectTran;
    private GameObject childInput;
    private Text childInputText;
    private GameObject _YES_ButtonGO;

    private InputField input;
    private Text childStartText;
    private bool isInit = false;

    private bool isStartDown = false;
    private bool isStartDoTween = false;
    private float startMin = 60;
    [SerializeField] private string str;
    [Header("指针")]
    public RectTransform ZhenRectTran;
    [Header("动画播放总时间")]
    public float timeCount;
    [Header("钟表时间（分钟）")]
    public int Num;
    [Header("时间调节时调用该事件")]
    public UpdateTime TiaojieUpdate;
    [Header("指针动画播放时每帧调用该事件")]
    public UpdateTime PlayUpdate;
    [Header("时间设置好了确定按钮额外事件，参数为当前时间")]
    public Action<int> YesEvent;
    [Header("指针动画播完调用该事件")]
    public Action Finish;
    public PointerEventData PointerEventData;
    /// <summary>
    /// 得到鼠标坐标对应的分钟
    /// </summary>
    public float MouseMin
    {
        get
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRectTran, Input.mousePosition, PointerEventData?.enterEventCamera, out Vector2 localMouse);
            float z = GetAngle(localMouse);
            return z <= 0 ? 0 : (360 - z) * 2f;
        }
    }
    /// <summary>
    /// 得到指针Z旋转对应的分钟
    /// </summary>
    public float ZhenRoZMin
    {
        get
        {
            float z = ZhenRectTran.localRotation.eulerAngles.z;
            return z <= 0 ? 0 : (360 - z) * 2f;
        }
    }
    void Awake()
    {

    }
    void OnEnable()
    {
        OnReset();
        OnYes();
    }

    void Init()
    {
        thisRectTran = this.GetComponent<RectTransform>();
        childInput = transform.parent.Find("Input").gameObject;
        childInputText = childInput.transform.Find("Text").GetComponent<Text>();
        childStartText = transform.parent.Find("Start/Text").GetComponent<Text>();
        input = childInput.transform.Find("InputField").GetComponent<InputField>();
        _YES_ButtonGO = transform.parent.Find("YES_Button").gameObject;
        isInit = true;
    }
    void OnReset()
    {
        if (!isInit)
            Init();
        childStartText.transform.parent.gameObject.SetActive(false);
        _YES_ButtonGO.SetActive(true);
        childInput.SetActive(true);
        isStartDoTween = false;
        childStartText.text = "0";
        input.text = startMin.ToString();
        childInputText.text = str;
      //  OnInputFiled(input.text);
        
    }
    /// <summary>
    /// 有确认
    /// </summary>
    /// <param name="str"></param>
    /// <param name="yesEvent"></param>
    /// <param name="finish"></param>
    public void Set(string str = "请输入",float startMin = 30,Action<int> yesEvent = null, Action finish = null)
    {
        this.startMin = startMin;
        this.YesEvent=(yesEvent);
        this.Finish=(finish);
        this.str = str;
        this.gameObject.SetActive(true);
        this.transform.parent.gameObject.SetActive(true); 
    }
    /// <summary>
    /// 只有动画
    /// </summary>
    /// <param name="orgin"></param>
    /// <param name="yesEvent"></param>
    /// <param name="finish"></param>
    public void SetStart(float orgin, Action<int> yesEvent = null, Action finish = null)
    {
        Set("", orgin, yesEvent,finish);
        OnYes();
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isStartDown = false;
        }
        if (isStartDown)
        {
            input.text = ((int)MouseMin).ToString();
            TiaojieUpdate?.Invoke(MouseMin);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isStartDoTween)
            isStartDown = true;
    }
    void Dotween()
    {
        float z = ZhenRectTran.localRotation.eulerAngles.z;
        z = z < 0 ? z + 360 : z;
        Tweener t;
        void thisFinish()
        {
             this.transform.parent.gameObject.SetActive(false);
            childStartText.text = "完成";
            Finish?.Invoke();
        }
        void thisUpdate()
        {
            PlayUpdate?.Invoke(ZhenRoZMin);
            childStartText.text = string.Format("请等待：{0}分", (ZhenRoZMin).ToString("F1"));
        }

        if (z > 180)
        {
            t = ZhenRectTran.DOLocalRotate(Vector3.zero, timeCount).OnUpdate(thisUpdate).OnComplete(thisFinish);
        }
        else
        {
            t = ZhenRectTran.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, (360 + z) / 2), timeCount / 2).OnComplete(() =>
              {
                  ZhenRectTran.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 360), timeCount / 2).OnUpdate(thisUpdate).OnComplete(thisFinish);
              }).OnUpdate(thisUpdate);
        }
    }


    public void OnYes()
    {
        Num =(int) float.Parse(input.text);
        isStartDoTween = true;
        childInput.SetActive(false);
        childStartText.transform.parent.gameObject.SetActive(true);
        _YES_ButtonGO.SetActive(false);
        YesEvent?.Invoke(Num);
        Dotween();
    }

    float GetAngle(Vector2 local)
    {
        float angle = 0;
        if (local.x == 0)
        {
            if (local.y > 0)
            {
                angle = 0;
            }
            if (local.y < 0)
            {
                angle = 180;
            }
        }
        else
        {
            float tan = local.y / local.x;
             angle = Mathf.Atan(tan) * 180 / Mathf.PI;
            if (local.x > 0)
            {
                angle -= 90;
            }
            else
            {
                angle += 90;
            }
            while (angle<0)
            {
                angle += 360;
            }
            while (angle >360)
            {
                angle -= 360;
            }
        }
        return angle;
    }
    public void OnInputFiled(string min)
    {
        float.TryParse(min, out float shuzhi);
        shuzhi = Mathf.Clamp(shuzhi, 0, 720);
        //input.text = shuzhi.ToString();
        float angle = (720 - shuzhi) / 2f;
        ZhenRectTran.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnDrag(PointerEventData eventData)
    {
        PointerEventData = eventData;
    }
}
