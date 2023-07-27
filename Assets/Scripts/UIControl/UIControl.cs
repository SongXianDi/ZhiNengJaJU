using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Button StartBtn;
    public Button KanBan_Btn;
    public Button RongYu_Btn;

    public Transform Tishi;

    private Transform KanBanPanel;
    private Transform RongYUPanel;

    public ScrollRect scrollRect;

    public Button left;
    public Button right;
    private void Awake()
    {
        StartBtn = transform.Find("bg/开始").GetComponent<Button>();
        KanBan_Btn = transform.Find("bg/看板展示").GetComponent<Button>();
        RongYu_Btn = transform.Find("bg/荣誉介绍").GetComponent<Button>();

        KanBanPanel = transform.Find("看板展示");
        RongYUPanel = transform.Find("荣誉介绍");
        //Tishi.gameObject.SetActive(false);
    }

    void Start()
    { 
        StartBtn.onClick.AddListener(Init);

        KanBan_Btn.onClick.AddListener(() =>
        {
            RongYUPanel.XPUICloseOpen(KanBanPanel);
        });
        RongYu_Btn.onClick.AddListener(() =>
        {
            KanBanPanel.XPUICloseOpen(RongYUPanel);
        });
        left.onClick.AddListener(() =>
        {
            scrollRect.horizontalNormalizedPosition -= 0.2f;
        });
        
        right.onClick.AddListener(() =>
        {
            scrollRect.horizontalNormalizedPosition += 0.2f;
        });
    }
    private void Init()
    {
        //Tishi.XPUIOpen();
        transform.Find("bg").gameObject.SetActive(false);
        StartBtn.transform.XPUIClose();
        StartBtn.gameObject.SetActive(false);  
        GameManager.Instance.gameControl.StartGame();
        transform.Find("地图").gameObject.SetActive(true);
        transform.Find("流程图").gameObject.SetActive(true);
        RongYUPanel.XPUIClose();
        KanBanPanel.XPUIClose();
        StartBtn.GetComponent<StepImageManager>().arrviedThis();
    }
    public void SetpBtn(int button)
    {
        if ((StepType)button != GameManager.Instance.CurrentSetpType)
        {
            GameManager.Instance.ZhuChiRen.TiShiShow(false);
            GameManager.Instance.PlayerSetpType = (StepType)button;
            GameManager.Instance.ZhuChiRenMove(button);
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


}
