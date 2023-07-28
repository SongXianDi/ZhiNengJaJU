using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FormulaMask : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    GameObject BG1;
    GameObject BG2;

    bool isClick;
    private void Awake()
    {
        BG1 = transform.GetChild(0).gameObject;
        BG2 = transform.GetChild(1).gameObject;
    }
    void Start()
    {
        BG1.SetActive(true);
        BG2.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isClick)
        ShowHint();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClick)
        {
            BG1.SetActive(false);
            BG2.SetActive(true);
            isClick = true;
        }
    }
    void ShowHint()
    {
      Image bg1=  BG1.GetComponent<Image>();
        bg1.color = new Color(0, 0, 0, 1);
        GetManager.Instance.Invok(0.2f, () =>
        {
            bg1.color = new Color(1, 1, 1, 1);
            GetManager.Instance.Invok(0.2f, () =>
            {
                bg1.color = new Color(0, 0, 0, 1);
                GetManager.Instance.Invok(0.2f, () =>
                {
                    bg1.color = new Color(1, 1, 1, 1);
                });
            });
        });
    }


}
