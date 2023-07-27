using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;

/// <summary>
/// 可点击物体
/// </summary>
public class OnClickObj : MonoBehaviour
{
    protected HighlightEffect highlight;


    // Start is called before the first frame update
    void Start()
    {
        highlight = transform.GetComponent<HighlightEffect>();
    }

    public void OnActive(bool isAct)
    {
        GetComponent<HighlightEffect>().ProfileLoad(GameManager.Instance.highlightTool);
        GetComponent<Collider>().enabled = isAct;
        GetComponent<HighlightEffect>().highlighted = isAct;
    }

    protected virtual void OnMouseDown()
    {
        //highlight.highlighted = false;

        OnActive(false);
    }
    public void ObjsActive()
    {

        if (GetComponent<HighlightPlus.HighlightEffect>())
        {
            GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;
        }
        else
        {
            gameObject.AddComponent<HighlightPlus.HighlightEffect>().highlighted = true;
        }
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
        OnActive(true);
    }

}

