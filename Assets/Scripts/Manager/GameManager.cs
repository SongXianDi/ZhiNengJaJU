﻿using System.Collections;
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



    //当前阶段
    private StepType currentSetpType = StepType.NIL;
    //实际(调试时跳过步骤可改)
    private StepType realSetpType = StepType.一号车间分拣线;

    //阶段每个地点
    public Transform[] points;

    //玩家选择的阶段
    private StepType playerSetpType = StepType.NIL;

    public StepType PlayerSetpType
    {
        get => playerSetpType; set
        {
            if (playerSetpType <= realSetpType)
                playerSetpType = value;
            else
                playerSetpType = realSetpType;
        }
    }

    public StepType CurrentSetpType { get => currentSetpType; }
    public StepType RealSetpType { get => realSetpType; }

    private void Start()
    {
        ZhuChiRen = GameObject.FindGameObjectWithTag("Enemy").GetComponent<ZhuChiRen>();
        //mater.GetComponent<MeshRenderer>().material.SetFloat("_Speed",0.5f);
    }
    public void ZhuChiRenMove(int i)
    {
        ZhuChiRen.FlipTo(points[i].position);
    }
    /// <summary>
    /// 玩家到达地点后处理的事情（动画，可点击...）
    /// </summary>
    public void PlayerArrviePonit(StepType setpType)
    {
        ZhuChiRen.TiShiShow(true);

        // StepGeneralMethod(setpType);
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
        if ((int)realSetpType > (int)step)
        {
            Debug.Log("以前步骤");
            stepManager.BeforSetp(step);
        }
        else if (realSetpType == step)
        {
            print("下一步骤");
            stepManager.NextSetp(step);
            realSetpType = step + 1;

        }
        currentSetpType = step;


        //AudioManage.Instance.PlayMusicSource(audioName, 0.5f);
        //obje.GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;
    }

}
