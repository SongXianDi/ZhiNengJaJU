using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
     Button StartBtn;
     Toggle KanBan_Btn;
     Toggle RongYu_Btn;

     Transform Tishi;

    private Transform KanBanPanel;

     ScrollRect scrollRect;

     Button UP;
     Button Down;

    public Sprite KanBanSelect;
    public Sprite KanBanUnSelect;
    private void Awake()
    {
        StartBtn = transform.Find("btn/开始").GetComponent<Button>();
        KanBan_Btn = transform.Find("看板展示/看板展示").GetComponent<Toggle>();
        RongYu_Btn = transform.Find("看板展示/荣誉介绍").GetComponent<Toggle>();
        Tishi = transform.Find("提示");
        KanBanPanel = transform.Find("看板展示");
        scrollRect = transform.Find("流程图").GetComponent<ScrollRect>();
        UP = transform.Find("流程图/UP").GetComponent<Button>();
        Down = transform.Find("流程图/Down").GetComponent<Button>();
        //Tishi.gameObject.SetActive(false);
    }

    void Start()
    { 
        StartBtn.onClick.AddListener(Init);

        KanBan_Btn.onValueChanged.AddListener((a) =>
        {
            if(a)
            {
                KanBan_Btn.image.sprite = KanBanSelect;
                KanBanJieShaoPanleChange("看板介绍", " <color=#00000000>111</color>南康家具产业从90年代发展至今，" +
                    "已经形成了两千多亿的产业集群规模，如何向五千亿和万亿迈进，南康区委区政府结合本地产业的集群优势提出以发展数字经济、" +
                    "平台经济和共享经济为抓手的发展思路，通过人才计划，引进复旦大学专家博士团队运用5G工业互联网技术打造了一网五中心促推产业的数字化转型升级，实现产业资源最佳配置，达到共享生产、协同制造的目的。" +
                    "其中一网络是指中国家具产业智联网，五中心分别是国际木材交易中心、创新设计中心、共享智能备料中心、共享喷涂中心和销售物流中心，现在我们所在的就是五中心里面共享智能备料板块其中的一个备料实体工厂。" +
                    " <color=#00000000>111</color>龙回备料中心项目占地2万平米，年产值约2亿元，项目于2020年9月投产，是目前亚洲单体最大、智能化水平最高的实木备料工厂，" +
                    "弥补了南康家具产业没有共享备料的空白，通过数字、科技的赋能实现降本提质增效。备料中心通过共享的生产模式，提供了家具备料及零部件，让南康家具实现了像造汽车一样造家具。");
            }
            else
            {
                KanBan_Btn.image.sprite = KanBanUnSelect;
            }
        });
        RongYu_Btn.onValueChanged.AddListener((a) =>
        {
            if(a)
            {
                RongYu_Btn.image.sprite = KanBanSelect;
                KanBanJieShaoPanleChange("荣誉介绍", " <color=#00000000>111</color>项目运营公司（即城发家具智能制造公司）自2019年11月成立以来，" +
                    "始终坚持以服务地方产业为宗旨、重塑生产方式为手段，通过智能制造创新引领，结合各大高校、院所联合开发运用最新科技成果，为南康家具产业发展提供了新思路、新模板。已先后荣获2021年第五届世界橡胶木产业大会“金橡奖”等荣誉称号，" +
                    "成功入选国家级2021年度智能制造优秀场景，先后获批成为科技型中小企业、国家级高新技术企业，2021年度“赣州市智能制造标杆企业”；共享智能备料中心项目已列入江西“海智计划”工作站，荣获中国第四届“绽放杯”5G应用征集大赛江西省一等奖，" +
                    "推动建设的中国家具产业智联网平台项目已列入国家级“03专项及5G项目科技重大专项立项”。");

            }
            else
            {
                RongYu_Btn.image.sprite = KanBanUnSelect;
            }
        });
        UP.onClick.AddListener(() =>
        {
            scrollRect.verticalNormalizedPosition += 0.2f;
        });
        
        Down.onClick.AddListener(() =>
        {
            scrollRect.verticalNormalizedPosition -= 0.2f;
        });
        KanBanPanel.gameObject.SetActive(true);
        KanBan_Btn.onValueChanged.Invoke(true);
    }
    private void Init()
    {
        //Tishi.XPUIOpen();
        Tishi.gameObject.SetActive(true);
        StartBtn.transform.XPUIClose();
        StartBtn.gameObject.SetActive(false);  
        GameManager.Instance.gameControl.StartGame();
        transform.Find("地图").gameObject.SetActive(true);
        transform.Find("流程图").gameObject.SetActive(true);
        KanBanPanel.XPUIClose();
        StartBtn.GetComponent<StepImageManager>().arrviedThis();
    }
    //按钮点击触发主持人移动
    public void SetpBtn(int button)
    {
        if ((StepType)button != GameManager.Instance.CurrentSetpType)
        {
            GameManager.Instance.ZhuChiRenMove((StepType)button);
        }
        else
        {
            OperationHintManager.Instance.ChangeText("请点击其他步骤");
        }

    }


    public void ShowPanel(Transform transform)
    {
        transform.XPUIOpen();
    }

    private void KanBanJieShaoPanleChange(string title,string context)
    {
        KanBanPanel.transform.Find("Title").GetComponent<Text>().text = title;
        KanBanPanel.transform.Find("Image/Text").GetComponent<Text>().text = context;
    }
}
