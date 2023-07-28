using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GetManager : BaseInstance<GetManager>
{
    private GameObject root;
    public GameObject Root
    {
        get
        {
            if (!root)
                root = GameObject.Find("SYS002Root");
            return root;
        }
    }
    public GameObject GetRootChild(string rootPath)
    {
        var obj = Root.transform.Find(rootPath);
        if (!obj)
            Debug.LogError(rootPath+"找不到");
        return obj.gameObject;
    }
    public GameObject GetToolChild(string toolPath)
    {
        GameObject obj = GetRootChild("Tools/" + toolPath);
        if (!obj)
            Debug.LogError(toolPath + "找不到");
        return obj;
    }
    public BaseTool GetBaseTool(string toolPath)
    {
        GameObject obj = GetRootChild("Tools/" + toolPath);
        if (!obj)
            Debug.LogError(toolPath + "找不到");

        BaseTool baseTool = obj.GetComponent<BaseTool>();
        if (!baseTool)
            Debug.LogError(toolPath + "找不到脚本");
        return baseTool;
    }
    public ClockZhen ClockZhen { get { return GetRootChild("Canvas/Mask/ClockPlane/Clock").GetComponent<ClockZhen>(); } }

    public void Invok(float time,Action action)
    {
        int i = 0;
        DOTween.To(() => i, x => i = x, 10, time).OnComplete(()=>action?.Invoke());
    }
}
