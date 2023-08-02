using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
//using UnityStandardAssets.Characters.FirstPerson;

public class UIManager : BaseInstanceMono<UIManager>
{
    public UIControl uiControl;

    public Button[] stepButtons;

    private GameObject DongTaiPanel;
    //物体介绍
    public GameObject WuTiJieShao;

    //跟随按钮
    public Button followBtn;

    public Transform MuCaiJieShao;


    private Color color;
    public override void Awake()
    {
        base.Awake();
        WuTiJieShao = transform.Find("物体介绍").gameObject;
        MuCaiJieShao = transform.Find("木材介绍");
        uiControl = transform.GetComponent<UIControl>();
        followBtn = transform.Find("自动跟随").GetComponent<Button>();
        color = new Color(255, 255, 255, 0.5f);
        for (int i = 0; i < transform.Find("流程图/bg2/bg").childCount; i++)
        {
            stepButtons[i] = transform.Find("流程图/bg2/bg").GetChild(i).GetComponent<Button>();
        }
    }
    private void Start()
    {

        for(int i=0;i<stepButtons.Length;i++)
        {
            if(i!=0)
            {
                stepButtons[i].interactable = false;
                stepButtons[i].transform.GetChild(0).GetComponent<Image>().color = color;
            }
            
            int buttonIndext = i;
            stepButtons[i].onClick.AddListener(() => {
                Debug.Log("click");
                uiControl.SetpBtn(buttonIndext); 
            });
        }
        WuTiJieShao.transform.Find("关闭").GetComponent<Button>().onClick.AddListener(() =>
        {
            WuTiJieShao.transform.XPUIClose();
        });
        followBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.player.FlipTo(/*GameManager.Instance.points[(int)GameManager.Instance.PlayerSetpType].position*/GameManager.Instance.ZhuChiRen.transform.position);
            followBtn.transform.XPUIClose();
        });
    }

    public void Show(Transform transform)
    {
        uiControl.ShowPanel(transform);
    }

    public void FollowBtnShow()
    {
        uiControl.ShowPanel(followBtn.transform);
    }

    public void ButtoniSAct(StepType setpType)
    {
        stepButtons[(int)setpType].interactable = true;

        //buttons[(int)setpType].transform.GetChild(0).GetComponent<Image>().color=Color.white;
    }
    public void ButtoniSColorChange(int setpType)
    {
        if(GameManager.Instance.CurrentSetpType>=0)
        {
            stepButtons[(int)GameManager.Instance.CurrentSetpType].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            stepButtons[(int)GameManager.Instance.CurrentSetpType].transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.white;
        }

        stepButtons[(int)setpType].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        stepButtons[(int)setpType].transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.yellow;
        //buttons[(int)setpType].transform.GetChild(0).GetComponent<Image>().color=Color.white;
    }
    public void DoTaiPanelChage(string text)
    {
        DongTaiPanel.transform.Find("Image/Text").GetComponent<Text>().text = text;
        uiControl.ShowPanel(DongTaiPanel.transform);
    }

    public void WuTiJieShaoChage(string title,string text)
    {
        WuTiJieShao.transform.Find("Title").GetComponent<Text>().text = title;
        WuTiJieShao.transform.Find("Content/bg/Image/Text").GetComponent<Text>().text = text;
        WuTiJieShao.transform.Find("Content").GetComponent<ScrollRect>().normalizedPosition = Vector2.one;
        uiControl.ShowPanel(WuTiJieShao.transform);
    }

    public void ColorChange(Button button)
    {
        //button.
    }
}
