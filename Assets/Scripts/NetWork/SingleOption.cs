using System.Collections.Generic;
using System.Linq;

/// <summary>答题实验部分信息</summary>
[System.Serializable]
public class OptionListClass
{
   public List<SingleOption> OptionList; 
}

[System.Serializable]
public class SingleOption
{
    /// <summary>
    /// 标题1,2,3,4,5
    /// </summary>
    public int Code;
    /// <summary>
    /// 选择题内容
    /// </summary>
    public string headStr;
    /// <summary>
    /// 所有选项
    /// </summary>
    public string[] optionArray;
    /// <summary>
    /// 正确的选项，对应所有选项里的索引，多个选项用逗号间隔（0,1,2)
    /// </summary>
    public string trueStr;
    /// <summary>
    /// 用户所选择的多个选择用逗号间隔为（0,1,2）
    /// </summary>
    public string selectStr;
    /// <summary>
    /// 这个题是否填对了
    /// </summary>
    public bool TrueFalse = false;
    /// <summary>
    /// 这个选择题多少分
    /// </summary>
    public int Score;
    /// <summary>
    /// 选择题类型0单选，1多选，2或
    /// </summary>
    public int OptionType;
    /// <summary>
    /// 用户选择的列表
    /// </summary>
    public List<string> SelectList;
    public SingleOption(int Code, string TrueStr, params string[] optionArray)
    {
        this.Code = Code;
        this.trueStr = TrueStr;
        this.optionArray = optionArray;
        SelectList = new List<string>();
    }
    public SingleOption() { SelectList = new List<string>(); }
    public string HeadStr
    {
        get
        {
            return string.Format("{0}、{1}", Code, headStr);
        }
    }

    //这里做可以做自己的判断操作
    public bool IsTrueFalse()
    {
        string[] trueArray = trueStr.Split(',');
        selectStr = "";
        if (SelectList != null &&SelectList.Count>=1)
        {
            SelectList.Sort();
            for (int i = 0; i < SelectList.Count - 1; i++)
            {
                selectStr += SelectList[i] + ",";
            }
            selectStr += SelectList[SelectList.Count - 1];
        }
        if (OptionType == 2)
        {
            if (SelectList == null || SelectList.Count == 0)
                TrueFalse = false;
            else
            {
                foreach (var select in SelectList)
                {
                    if (!trueArray.ToList().Contains(select))
                        TrueFalse = false;
                }
                TrueFalse = true;
            }
        }
        else
        {
            TrueFalse = trueStr.Equals(selectStr);
        }
        return TrueFalse;
    }
    //end
}
