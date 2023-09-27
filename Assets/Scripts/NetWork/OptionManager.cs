using System.Collections.Generic;
using UnityEngine;

/// <summary>题库管理</summary>
public class OptionManager 
{
    private static OptionManager _instance;
    public static OptionManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new OptionManager();
            }
            return _instance;
        }
    }
    private List<SingleOption> _optionList;

    /// <summary>
    /// 最终提交的答题数据,需要对分数进行赋值
    /// </summary>
    public OptionListClass OptionListClass;

    public void SetQuestionBank(QuestionBank questionBank)
    {
        _optionList = new List<SingleOption>();
        for (int i = 0; i < questionBank.data.Count; i++)
        {
            var question = questionBank.data[i];

            SingleOption singleOption = new SingleOption() { Code = i + 1, headStr = question.name };
            singleOption.optionArray = question.OptionArray().ToArray();
            singleOption.trueStr = question.TrueStr();
            singleOption.Score = question.score;
            singleOption.OptionType = question.OptionType();
            _optionList.Add(singleOption);
        }
        OptionListClass.OptionList=_optionList;
    }
    /// <summary>
    /// 获取题目
    /// </summary>
    /// <param name="questionBankJosn"></param>
    /// <returns></returns>
    public bool SetQuestionBank(string questionBankJosn)
    {
        QuestionBank questionBank = JsonUtility.FromJson<QuestionBank>(questionBankJosn);
        for (int i = 0; i < questionBank.data.Count; i++)
        {
            //questionBank.data[i].OptionList = JsonMapper.ToObject<List<OptionConent>>(questionBank.data[i].option);
            questionBank.data[i].OptionList = JsonUtility.FromJson<List<OptionConent>>(questionBank.data[i].option);
        }
        if (questionBank.data == null || questionBank.data.Count <= 0)
            return false;
        SetQuestionBank(questionBank);
        return true;
    }
}
