using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum StepType
{
    NIL = -1,
    一号车间分拣线 = 0,
    智能AGV小车调度区,
    一号车间码垛线,
    一号车间梳齿指接线,
    指接养生房,
    连续拼板线,
    拼板养生房,
    自动砂光区,
    冷压拼板区,
    自动开料区,
    End,
}



public class GameManager : BaseInstanceMono<GameManager>
{

    public GameControl gameControl;

    public ZhuChiRen ZhuChiRen;

    public StepManager stepManager;

    public HighlightPlus.HighlightProfile highlightTool;

    public PlayerControl player;

    //当前阶段
    private StepType currentSetpType = StepType.NIL;
    //实际(调试时跳过步骤可改)
    private StepType realStepType = StepType.一号车间分拣线;

    //阶段每个地点
    public Transform[] points;

    //玩家选择的阶段
    private StepType playerSetpType = StepType.NIL;

    public StepType PlayerSetpType
    {
        get => playerSetpType; set
        {
            if (playerSetpType <= realStepType)
                playerSetpType = value;
            else
                playerSetpType = realStepType;
        }
    }

    public StepType CurrentSetpType { get => currentSetpType; }
    public StepType RealSetpType { get => realStepType; }

    private void Start()
    {
        ZhuChiRen = GameObject.FindGameObjectWithTag("Enemy").GetComponent<ZhuChiRen>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }
    /// <summary>
    /// 主持人说完后进入下一步（或者按钮点击的下一步）
    /// 步骤结束
    /// </summary>
    /// <param name="i"></param>
    public void ZhuChiRenMove(StepType i)
    {
        if (RealSetpType < StepType.End)
        {
            UIManager.Instance.ButtoniSAct(GameManager.Instance.RealSetpType);
        }
        UIManager.Instance.ButtoniSColorChange((int)i);
        ZhuChiRen.TiShiShow(false);
        PlayerSetpType =i;
        ZhuChiRen.FlipTo(points[(int)i].position);

        Debug.Log(realStepType);
        Debug.Log(i);
    }

    /// <summary>
    /// 主持人到达地点后处理的事情（激活玩家跟随按钮）
    /// </summary>
    public void ZCRArrviePonit()
    {
        print("到达");
        UIManager.Instance.FollowBtnShow();
    }


    /// <summary>
    /// 步骤通用方法
    /// </summary>
    /// <param name="tips">提示信息</param>
    /// <param name="audioName">播放音效</param>
    /// <param name="obje">下一步需要提示的物品</param>
    /// <param name="isNext">是否需要下一步</param>
    public void StepGeneralMethod(StepType step, string audioName = null, bool isNext = true)
    {
        //是（以前步骤） 否（下一步骤）
        if ((int)realStepType > (int)step)
        {
            Debug.Log("以前步骤");
            stepManager.BeforeSetp(step);
        }
        else if (realStepType == step)
        {
            print("下一步骤");
            stepManager.NextSetp(step);
            realStepType = step + 1;

            if (currentSetpType != StepType.NIL)
            {
                ScoreManager.Instance.SetTestProcedureScoreAndValueAdnEndTime(currentSetpType.ToString(), 0, "", "赋分模型", 60, 1);
            }

            //分数增加（只有一次）
        }
        currentSetpType = step;
        //AudioManage.Instance.PlayMusicSource(audioName, 0.5f);
        //obje.GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;
    }


}
