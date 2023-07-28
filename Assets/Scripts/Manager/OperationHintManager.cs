using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OperationHintManager : BaseInstanceMono<OperationHintManager>
{
    private Text contentText;
    private Tweener t;

    public Button zhanKai;
    public Button guanbi;

    private Tweener _tweener;
    void Start()
    {

        contentText = transform.Find("ContentText").GetComponent<Text>();
        guanbi.onClick.AddListener(Close);
        zhanKai.onClick.AddListener(Open);
        //gameObject.SetActive(false);

        _tweener = transform.DOMoveX(152.1f, 1f);
        _tweener.SetAutoKill(false);
        _tweener.Pause();
    }

    public void Open()
    {
        transform.DOPlayForward();
        zhanKai.gameObject.SetActive(false);
        //_tweener= transform.DOMoveX(152.1f, 2f);
    }

    public void ChangeText(string str)
    {
        if (!contentText)
            Start();
       // if (_tweener != null)
           // _tweener.Kill();
        
       // t = transform.XPUIOpen();
        contentText.text = str;
        Open();

        Invoke("Close", 3f);
    }


    public void Close()
    {
        //transform.DOMoveX(-200, 2f).SetAutoKill(false);
        //transform.XPUIClose();
        transform.DOPlayBackwards();
        zhanKai.gameObject.SetActive(true);
        //transform.DOMoveX(152.1f, 2f).From();
        //_tweener.From();
    }
}
