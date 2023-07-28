using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 需要展示信息的可点击物体
/// </summary>
public class ClickPanel : OnClickObj
{
    public Sprite picture;
    [TextArea(2, 2)] [Header("产地")]
    public string text;
    [TextArea(10, 10)][Header("描述")]
    public string describe;
    private Transform panel;

    private WaitForSeconds seconds;
    void Start()
    {
        panel = UIManager.Instance.MuCaiJieShao;
        seconds = new WaitForSeconds(3f);
    }

    protected override void OnMouseDown()
    {
        panel.transform.Find("ImageLeft/Picture").GetComponent<Image>().sprite = picture;
        panel.transform.Find("ImageLeft/Text").GetComponent<Text>().text = text;
        panel.transform.Find("ImageRight/Text").GetComponent<Text>().text = describe;
        base.OnMouseDown();
        UIManager.Instance.Show(panel);
        StartCoroutine(ShowHLight());
    }

    IEnumerator ShowHLight()
    {
        yield return seconds;
        OnActive(true);
    }

    void OnDestory()
    {
        StopCoroutine(ShowHLight());
    }
}
