

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using HighlightPlus;
using UnityEngine.EventSystems;

[RequireComponent(typeof(HighlightEffect))]
public  class BaseTool : MonoBehaviour
{
    public string ChineseName;
    public float Mass;
    public bool IsUserSetNULL = true;

    public BaseTool Parent;
    public BaseTool Child;
    public UnityEvent OnMouseDownEvent;
    public UnityEvent OnMouseDragEvent;
    public UnityEvent OnMouseUpEvent;

    [HideInInspector]public Vector3 orginPos;
    private Quaternion orginQ;
    private HighlightEffect effect;
    private BoxCollider box;
    private Transform orginParent;
    public void Awake()
    {
        orginPos = transform.position;
        orginQ = transform.rotation;
        effect = this.GetComponent<HighlightEffect>();
        if (!effect)
        {
            effect = this.gameObject.AddComponent<HighlightEffect>();
        }
        effect.profile = Resources.Load<HighlightProfile>("Tools");
        effect.ProfileLoad(effect.profile);
        effect.profileSync = true;

        box = this.GetComponent<BoxCollider>();
        if(box)
        box.isTrigger = true;
        orginParent = transform.parent;
        // effect.highlighted = true; 
        if (transform.Find("Hint/New Text"))
        {
            transform.Find("Hint").gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 得到这个物品的总重量（包含子物体)
    /// </summary>
    /// <param name="baseTool"></param>
    /// <returns></returns>
    public float GetMass()
    {
        float sum = 0;
        var p = this;
        while(p)
        {
            sum += p.Mass;
            p = p.Child;
        }
        return sum;
    }
    /// <summary>
    /// 把自身放到其他身上
    /// </summary>
    public void SelfToQita(BaseTool targetTool)
    {
        Parent = targetTool;
        targetTool.Child = this;
    }
    /// <summary>
    /// 放东西到自身上
    /// </summary>
    public void QitaToSelf(BaseTool qitaTool)
    {
        Child = qitaTool;
        Child.Parent = this;
        Child.transform.SetParent(transform);
    }
    /// <summary>
    /// 把自身脱离这个链条
    /// </summary>
    public void OutSelf(bool isSetOrginParent = true)
    {
        if(Parent)
        {
            Parent.Child = null;
            if(isSetOrginParent)
            transform.SetParent(orginParent);
        }
    }
    /// <summary>
    /// 添加按下事件
    /// </summary>
    /// <param name="action"></param>
    public void AddDownEvent(UnityAction action)
    {
         SetEmission(true);
        OnMouseDownEvent = new UnityEvent();
        OnMouseDownEvent.AddListener(action);
    }
    public void RemoveDownEvent()
    {
        OnMouseDownEvent.RemoveAllListeners();
        SetEmission(false);
    }
    /// <summary>
    /// 添加长按事件
    /// </summary>
    /// <param name="action"></param>
    public void AddDragEvent(UnityAction action)
    {
        SetEmission(true);
        OnMouseDragEvent = new UnityEvent();
        OnMouseDragEvent.AddListener(action);
    }
    public void RemoveDragEvent()
    {
        SetEmission(false);
        OnMouseDragEvent.RemoveAllListeners();
    }
    /// <summary>
    /// 添加弹起事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUpEvent(UnityAction action)
    {
        OnMouseUpEvent = new UnityEvent();
        OnMouseUpEvent.AddListener(action);
    }
    public virtual Tweener OnReset()
    {
        transform.DOMove(orginPos, 0.5f);
       return transform.DORotateQuaternion(orginQ, 0.5f);
    }
    private void OnMouseDown()
    {
       // Debug.Log(OnMouseDownEvent.GetPersistentEventCount());
        if(OnMouseDownEvent!=null )
        {
            OnMouseDownEvent.Invoke();
            SetEmission(false);
            if (IsUserSetNULL)
                OnMouseDownEvent = null;
        }
    }
    private void OnMouseDrag()
    {
        if(OnMouseDragEvent != null)
        {
            SetEmission(false);
            OnMouseDragEvent?.Invoke();
        }
       
    }
    private void OnMouseUp()
    {
        OnMouseUpEvent?.Invoke();
    }
    private void OnMouseEnter()
    {
        //var hint = transform.Find("Hint");
        //if (hint)
        //    hint.gameObject.SetActive(true);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (transform.Find("Hint/New Text"))
            {
                //UIManager.Instance.ShowToolName(transform.Find("Hint/New Text").GetComponent<TextMesh>().text);
            }
        }
    }
    private void OnMouseExit()
    {
       // UIManager.Instance.HideToolName();
        //var hint = transform.Find("Hint");
        //if (hint)
        //    hint.gameObject.SetActive(false);
    }
    public void SetEmission(bool isActive)
    {
        effect.highlighted = isActive;
    }

}
