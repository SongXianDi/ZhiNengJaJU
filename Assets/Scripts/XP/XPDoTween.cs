using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPDoTween : BaseInstanceMono<XPDoTween>
{
   public void OnTweenOpen(Transform tran)
    {
        tran.XPUIOpen();
    }
    public void OnTweenClose(Transform tran)
    {
        tran.XPUIClose();
    }
    public void OnTweenCloseOpen(Transform closeTran,Transform openTran)
    {
        closeTran.XPUICloseOpen(openTran);
    }
}
