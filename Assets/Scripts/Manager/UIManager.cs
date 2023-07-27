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

    public Button[] buttons;

    private GameObject DongTaiPanel;
    //物体介绍
    public GameObject WuTiJieShao;

    public Transform MuCaiJieShao;

    private Color color;
    public override void Awake()
    {
        base.Awake();
        WuTiJieShao = transform.Find("物体介绍").gameObject;
        MuCaiJieShao = transform.Find("木材介绍");
        uiControl = transform.GetComponent<UIControl>();
        color = new Color(255, 255, 255, 0.5f);
    }
    private void Start()
    {

        for(int i=0;i<buttons.Length;i++)
        {
            if(i!=0)
            {
                buttons[i].interactable = false;
                buttons[i].transform.GetChild(0).GetComponent<Image>().color = color;
            }
            
            int buttonIndext = i;
            buttons[i].onClick.AddListener(() => {
                ButtoniSColorChange(buttonIndext);
                uiControl.SetpBtn(buttonIndext); 
            });
        }
        WuTiJieShao.transform.Find("关闭").GetComponent<Button>().onClick.AddListener(() =>
        {
            WuTiJieShao.transform.XPUIClose();
        });
    }

    public void Show(Transform transform)
    {
        uiControl.ShowPanel(transform);
    }

    public void ButtoniSAct(StepType setpType)
    {
        buttons[(int)setpType].interactable = true;

        //buttons[(int)setpType].transform.GetChild(0).GetComponent<Image>().color=Color.white;
    }
    public void ButtoniSColorChange(int setpType)
    {
        if(GameManager.Instance.CurrentSetpType>=0)
        {
            buttons[(int)GameManager.Instance.CurrentSetpType].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            buttons[(int)GameManager.Instance.CurrentSetpType].transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.white;
        }

        buttons[(int)setpType].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        buttons[(int)setpType].transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.yellow;
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
