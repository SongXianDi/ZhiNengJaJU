using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//发出一条射线去判断物体
public class CamerManager : BaseInstanceMono<CamerManager>
{
    //当前涉嫌所点击的物体
    [HideInInspector] public GameObject currentObj;
    private LayerMask mask;
    public UnityAction action;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print("点击鼠标一次");
            SheXianDianJi(action);
        }
    }

    public GameObject SheXianDianJi(UnityAction action)
    {
        currentObj = null;
        //mask = LayerMask.GetMask("交互物体");
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //发射射线，起点是当前物体位置，方向是世界前方
        if (Physics.Raycast(ray,out hitInfo, 100,1 << 15))
        {
            print("点击到交互物体");
            print(hitInfo.transform.gameObject.name);
            currentObj = hitInfo.transform.gameObject;
            action?.Invoke();
        }
        return currentObj;

    }
    /// <summary>
    /// 清除委托中的事件
    /// </summary>
    public void ClearAction()
    {
        action = null;
    }
}
