using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
/// <summary>分数管理</summary>
public class ScoreManager : BaseInstanceMono<ScoreManager>
{
    private IlabData _ilabData;
    protected override void InitInStart()
    {
        base.InitInStart();
        _ilabData = GetComponent<HTTP>().TESTData;
    }
    public void InitIlabDataSteps(IlabData ilabData)
    {
        foreach (var step in ilabData.steps)
        {
            //float scoreRatio = (float)step.score / step.maxScore;
            //string evaluation;
            //if (scoreRatio == 1)
            //{
            //    evaluation = "优";
            //}
            //else if (scoreRatio > 0.5f)
            //{
            //    evaluation = "良";
            //}
            //else
            //{
            //    evaluation = "不合格";
            //}
            step.score = step.maxScore = 100 / ilabData.steps.Count;
            step.evaluation = "优";
            step.Init();
        }
    }

    /// <summary>
    /// 设置实验步骤的开始时间
    /// 在所用已经构造好的实验报表结构中找到对应的实验名称，并设置当前时间为实验开始时间
    /// </summary>
    public void SetTestProcedureStartTime(string procedureName)
    {
        TestStepData step = _ilabData.FindStepByName(procedureName);
        if (step == null)
        {
            Debug.Log("没有找到可以设置开始时间的实验步骤：" + procedureName);
            return;
        }
        else
        {
            step.startTime = GetNowTimestamp();
        }
    }


    /// <summary>
    /// 设置实验步骤的得分值
    /// 在所用已经构造好的实验报表结构中找到对应的实验名称，并设置实验步骤的得分值
    /// </summary>
    public void SetTestProcedureScore(string procedureName, int score)
    {
        TestStepData step = _ilabData.FindStepByName(procedureName);
        if (step == null)
        {
            Debug.Log("没有找到该实验步骤：" + procedureName);
            return;
        }
        else
        {
            step.score = score;
        }
    }

    /// <summary>
    /// 设置实验步骤的得分值和操作值以及结束时间
    /// 在所用已经构造好的实验报表结构中找到对应的实验名称，并设置实验步骤的得分值和操作值
    /// </summary>
    public void SetTestProcedureScoreAndValueAdnEndTime(string procedureName, int score, string value, string scoringModel, int expectTime, int repeatCount)
    {
        TestStepData step = _ilabData.FindStepByName(procedureName);
        if (step == null)
        {
            Debug.Log("没有找到可以设置信息该实验步骤：" + procedureName);
            return;
        }
        else
        {
            step.score = score;
            step.remarks = value;
            step.expectTime = expectTime;
            step.scoringModel = scoringModel;
            step.repeatCount = repeatCount;
            step.endTime = GetNowTimestamp();
        }
    }
    /// <summary>
    /// 在实验报表中添加实验步骤并设置实验步骤的各参数
    /// </summary>
    /// <param name="procedureName"></param>
    /// <param name="maxScore"></param>
    /// <param name="expectTime"></param>
    /// <param name="scoringModel"></param>
    /// <param name="repeatCount"></param>
    public void AddTestProcedure(string procedureName, int maxScore, int expectTime, string scoringModel, int repeatCount)
    {
        TestStepData step = new TestStepData();
        step.startTime = GetNowTimestamp();
        step.seq = _ilabData.steps.Count + 1;
        step.title = procedureName;
        step.maxScore = maxScore;
        step.expectTime = expectTime;
        step.scoringModel = scoringModel;
        step.repeatCount = repeatCount;
        _ilabData.steps.Add(step);
    }

    /// <summary>
    /// 得到当前时间的时间戳
    /// </summary>
    /// <returns></returns>
    public long GetNowTimestamp()
    {
        var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        var timestamp = ((long)(DateTime.Now - startTime).TotalMilliseconds);
        return timestamp;
    }
}