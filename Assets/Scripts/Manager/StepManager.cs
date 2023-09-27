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
    public void NextSetp(StepType step)
    {
        if (step != StepType.End)
        {
            ScoreManager.Instance.AddTestProcedure(step.ToString(), 0, 60, "赋分模型", 1);
        }
        GameManager.Instance.ZhuChiRen.TiShiShow(true);
        OperationHintManager.Instance.ChangeText("到达" + step.ToString() + "点击引导人或点击物体");

        //根据传进的流程名称进行保持实验步骤/*ScoreManager.Instance.SetTestProcedureStartTime(procedureName);*/
        stepImages[(int)step].arrviedThis();
    }

    /// <summary>
    /// 返回之前步骤
    /// </summary>
    public void BeforeSetp(StepType step)
    {
        if (GameManager.Instance.CurrentSetpType != step)
        {
            GameManager.Instance.ZhuChiRen.TiShiShow(true);
            OperationHintManager.Instance.ChangeText("回到" + step.ToString() + "点击引导人或点击物体");
        }
        stepImages[(int)step].arrviedThis();
    }
}
