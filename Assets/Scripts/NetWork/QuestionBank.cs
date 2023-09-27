using System.Collections.Generic;

[System.Serializable]
    public class QuestionBank
    {
        public List<Question> data;
    }
    [System.Serializable]
    public class Question
    {
        public int id;
        public string name;
        public string option;
        public List<OptionConent> OptionList;
        public string manyChoice;
        public int score;
        public int difficulty;
        public string TrueStr()
        {
            string trueStr = "";
            List<string> trueList = new List<string>();
            for (int i = 0; i < OptionList.Count; i++)
            {
                if (OptionList[i].correct)
                    trueList.Add(i.ToString());
            }
            for (int i = 0; i < trueList.Count; i++)
            {
                trueStr += trueList[i];
                if (i != trueList.Count - 1)
                {
                    trueStr += ',';
                }
            }
            return trueStr;
        }
        public List<string> OptionArray()
        {
            List<string> optinNameList = new List<string>();
            for (int i = 0; i < OptionList.Count; i++)
            {
                var optionContent = OptionList[i];
                optinNameList.Add(optionContent.name);
            }
            return optinNameList;
        }
        public int OptionType()
        {
            return int.Parse(manyChoice);
        }
    }

    /// <summary>
    /// 选项
    /// </summary>
    [System.Serializable]
    public class OptionConent
    {
        public int id;
        public string name;
        public bool correct;
    }

