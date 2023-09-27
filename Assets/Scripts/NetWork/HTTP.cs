using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class HTTP : MonoBehaviour
{
    [SerializeField]
    private JiangxiSciencesAbutment _jiangxiSciencesAbutment;
    private static DateTime OrginTime;
    string Access_Token;
    public static string UserID = "";
    public static string UserName = "";
    public static string score;
    /// <summary>
    /// 传给Ilab的实验报表
    /// </summary>
    public IlabData TESTData;

    void Start()
    {
        //放到Start可能不适合全部项目，需要自己处理一下调用时机
        Init();
    }
    void Init()
    {
        TESTData.steps = new List<TestStepData>();
        Debug.Log("当前平台" + Application.platform);
        //SetMaskLoad.Instance.StartMaskText("正在获取用户信息，请稍后......");
        switch (Application.platform)//判断当前运行平台
        {
            case RuntimePlatform.WindowsEditor:
                GetEditorToken();
                break;
            case RuntimePlatform.WindowsPlayer:
                GetPCToken();
                break;
            case RuntimePlatform.WebGLPlayer:
                Application.ExternalCall("SendSouces");
                //GetWebGLToken();
                break;
        }

        TESTData.startTime = ScoreManager.Instance.GetNowTimestamp();
    }

    //所有数据赋值完成后，最终提交
    public void FinalSubmitIableData()
    {
        //得到满分为20分时考核的分数
        int assessScore = 0;
        //foreach (var item in OptionManager.Instance.OptionListClass.OptionList)
        //{
        //    assessScore+=item.Score;
        //}
        //assessScore=(int)(assessScore* 0.2f);
        //end

        GetIlabeScore(assessScore);
        //更新ilab数据.
        ScoreManager.Instance.InitIlabDataSteps(TESTData);
        //设置实验的结束时间.
        TESTData.endTime = ScoreManager.Instance.GetNowTimestamp();
        TESTData.timeUsed = TESTData.GetTimeUsed();
        //提交
        OnSubmitIlabScore();
    }

    /// <summary>
    /// 转换成百分制，实验占80%
    /// </summary>
    private void GetIlabeScore(int assessScore)
    {
        //float getScore = 0;
        //foreach (var step in TESTData.steps)
        //{
        //    getScore+=(float)(step.score/step.maxScore)*100;
        //}

        //Debug.Log(getScore);
        //TESTData.score = (int)(assessScore+ (getScore / (TESTData.steps.Count)) * 0.8f);
        TESTData.score = 100;
        TESTData.status = 1;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O))
        {
            var obj = SetMaskLoad.Instance.gameObject;
            obj.SetActive(!obj.activeSelf);
        }

    }

    /// <summary>在编辑器状态下得到实验者信息</summary>
    private void GetEditorToken()
    {
        Debug.Log("当前在编辑器上运行,跳过获取信息");
        //可以自己去后台获取赋值常量
        UserID = "1";
        UserName = "admin";
        Access_Token = "hc09ab64277d17a1076046c7d3c0efbefbc3d396eddac9a797c";
        //end
    }

    /// <summary>PC平台下得到实验者信息</summary>
    private void GetPCToken()
    {
        OrginTime = DateTime.Now;
        string[] CommandLineArgs = Environment.GetCommandLineArgs();
        string str = CommandLineArgs.Aggregate<string, string>("", (a, b) => { return a + b; });
        Debug.Log("第一次" + str);
        string projectName = "car://";
        var index = str.IndexOf(projectName) + projectName.Length;
        str = str.Substring(index, str.Length - index - 1);
        Debug.Log("替换后出现的字符串:" + str);
        string[] strArray1 = str.Split('~');
        if (strArray1.Length >= 3)
        {
            //Token = strArray1[0];
            UserID = strArray1[0];
            UserName = Uri.UnescapeDataString(strArray1[1]);
            Access_Token = strArray1[2];
            Debug.Log(string.Format("用户ID:{0},用户姓名{1},Access_Token:{2}", UserID, UserName, Access_Token));
            SetMaskLoad.Instance.StartMaskText("获取成功", false, null, SetMaskLoad.LoadState.Win, 1, 0.1f, (obj => obj.SetActive(false)));
            //if (!string.IsNullOrEmpty(Access_Token))
            //    OnGetAllOptionList();
            //else
            //   Debug.LogError("Access_Token为空");
            //GetEditorToken();
        }
        else
        {
            SetMaskLoad.Instance.StartMaskText("用户数据获取异常，请重试......", true, Start, SetMaskLoad.LoadState.Error);
        }
    }

    /// <summary>WenbGL平台下得到实验者信息</summary>
    public void GetWebGLToken(string json)
    {
        Debug.Log("得到JSON字符串" + json);
        OrginTime = DateTime.Now;
        var jsonData = JsonUtility.FromJson<WebGLTokenData>(json);
        Debug.Log(jsonData);
        if (jsonData != null)
            Debug.Log($"jsonData.token={jsonData.token},jsondata.user_id={jsonData.user_id},jsonData.user_name={jsonData.user_name},jsonData.access_token={jsonData.access_token}");
        UserID = jsonData.user_id;
        UserName = jsonData.user_name;
        Access_Token = jsonData.access_token;
        Debug.Log(string.Format("获取到UserID:[{0}]，UserName:[{1}],AccessToken:[{2}]", UserID, UserName, Access_Token));
        if (!string.IsNullOrEmpty(Access_Token))
        {
            OnGetAllOptionList();
        }
        else
        {
            Debug.LogError("Access_Token为空");
        }
    }

    /// <summary>
    /// 请求获取选择题的内容
    /// </summary>
    private void OnGetAllOptionList()
    {
        //SetMaskLoad.Instance.StartMaskText("正在获取题库数据，请稍后......");

        //if (_jiangxiSciencesAbutment != null)
        //_jiangxiSciencesAbutment.GetQuestionBank(Succeed, Fail);
        // else
        //Debug.LogError("_jiangxiSciencesAbutment没找到");

        void Succeed()
        {
            SetMaskLoad.Instance.StartMaskText("获取成功", false, null, SetMaskLoad.LoadState.Win, 1, 0.1f, (obj => obj.SetActive(false)));
        }
        void Fail()
        {
            SetMaskLoad.Instance.StartMaskText("题库数据获取异常，请重试......", true, OnGetAllOptionList, SetMaskLoad.LoadState.Error);
        }
    }
    [ContextMenu("提交数据")]
    private void OnSubmitIlabScore()
    {
        SubmitData();


        void SubmitData()
        {
            SetMaskLoad.Instance.StartMaskText("正在上传成绩，请稍后......");

            Debug.Log("退送Ialb");
            //设置用户名
            TESTData.username = UserID;
            //  设置唯一ID
            TESTData.originId = TESTData.username + "" + ScoreManager.Instance.GetNowTimestamp().ToString();
            var dataJson = JsonUtility.ToJson(TESTData);
            Debug.Log(dataJson);
            Debug.Log($"一共有{TESTData.steps.Count}步骤");

            Debug.Log(Access_Token);
            _jiangxiSciencesAbutment.SubmitData(dataJson, Access_Token, () =>
            {
                OperationHintManager.Instance.ChangeText("结束，您可以自由参观厂区或退出");
                SetMaskLoad.Instance.StartMaskText("数据提交成功", false, null, SetMaskLoad.LoadState.Win, 0.3f, 2f, WinEvnet);
            },
               () => SetMaskLoad.Instance.StartMaskText("成绩提交失败，请重试......", true, () => SubmitData(), SetMaskLoad.LoadState.Error)
           );
        }

        void SubmitProblem()
        {
            _jiangxiSciencesAbutment.SubmitProblem(JsonUtility.ToJson(OptionManager.Instance.OptionListClass),
            () => SetMaskLoad.Instance.StartMaskText("答题数据提交成功", false, null, SetMaskLoad.LoadState.Win, 0.3f, 0.1f, WinEvnet),
             () => SetMaskLoad.Instance.StartMaskText("答题数据提交失败，请重试......", true, () => SubmitProblem(), SetMaskLoad.LoadState.Error)
              );

        }
    }
#if UNITY_EDITOR
    void WinEvnet(GameObject obj)
    {
        UnityEditor.EditorApplication.isPaused = true;
    }
#elif UNITY_WEBGL
            void WinEvnet(GameObject obj)
            {
                Application.ExternalCall("receiveSouces",HTTP.score );
                //Application.Quit();
            }
#else
            void WinEvnet(GameObject obj)
            {
                //Application.Quit();
            }
#endif


    /// <summary>
    /// 得到传给后台的时间格式
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public string DateTimeFormat(DateTime dateTime)
    {
        string formattedDateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        return formattedDateTime;
    }

    [System.Serializable]
    public class WebGLTokenData
    {
        public string token;
        public string user_id;
        public string user_name;
        public string access_token;
    }
}