using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepImageManager : MonoBehaviour
{
    public OnClickObj[] clickObjs;
    private void Start()
    {

    }


    public void arrviedThis()
    {
        //图片颜色改变
        //按钮激活
        GetComponent<Button>().interactable = true;

        if (clickObjs.Length > 0)
        {
            foreach (var item in clickObjs)
            {
               // item.OnActive(true);
                item.ObjsActive();
            }
        }
    }
}
