using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class StepManager : MonoBehaviour
{
    public StepImageManager[] stepImages;
    private void Start()
    {

    }
    /// <summary>
    /// 进入下一步骤(没有激活过的)
    /// </summary>
    public void NextSetp(StepType setp)
    {
        GameManager.Instance.ZhuChiRen.TiShiShow(true);
        OperationHintManager.Instance.ChangeText("到达" + setp.ToString()+ "点击引导人或点击物体");

        stepImages[(int)setp].arrviedThis();
    }

    /// <summary>
    /// 返回之前步骤
    /// </summary>
    public void BeforSetp(StepType step)
    {
        if (GameManager.Instance.CurrentSetpType != step)
        {
            GameManager.Instance.ZhuChiRen.TiShiShow(true);
            OperationHintManager.Instance.ChangeText("回到" + step.ToString()+"点击引导人或点击物体");
        }
        stepImages[(int)step].arrviedThis();
    }
}
