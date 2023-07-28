using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class XPShortcutExtensions
{
    /// <summary>
    /// 关闭UI的方法
    /// </summary>
    /// <param name="rectTran"></param>
    /// <returns></returns>
    public static Tweener XPUIClose(this Transform rectTran)
    {
        return rectTran.DOScaleY(0, 0.3f).OnComplete(() => rectTran.gameObject.SetActive(false)).SetEase(Ease.InBack);
    }
    /// <summary>
    /// 打开UI的方法
    /// </summary>
    /// <param name="rectTran"></param>
    /// <returns></returns>
    public static Tweener XPUIOpen(this Transform rectTran)
    {
        rectTran.gameObject.SetActive(true);
        rectTran.localScale = new Vector3(1, 0, 1);
        return rectTran.DOScaleY(1, 0.3f).SetEase(Ease.InOutBack);
    }
    /// <summary>
    /// 关闭当前的，打开下一个
    /// </summary>
    /// <param name="closeTran"></param>
    /// <param name="openTran"></param>
    public static void XPUICloseOpen(this Transform closeTran, Transform openTran)
    {
        XPUIClose(closeTran).OnComplete(() => XPUIOpen(openTran));
    }
    /// <summary>
    /// 通过速度进行移动动画
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="endPos"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static Tweener XPDoMoveSpeed(this Transform tran, Vector3 endPos, float speed = 1)
    {
        float time = Vector3.Distance(tran.position, endPos) / speed;
        return tran.DOMove(endPos, time);
    }
    /// <summary>
    /// 可以选择顺时针还是逆时针旋转
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="z">0-360</param>
    /// <param name="time"></param>
    /// <param name="isJust">是否顺时针方向</param>
    /// <returns></returns>
    public static Tweener XPDoLocalRotateQuaternionZ(this Transform tran, float orginZ, float z, float time, bool isJust = true)
    {
        var endQ = Quaternion.Euler(tran.localRotation.x, tran.localRotation.y, orginZ);
        tran.transform.localRotation = endQ;
        float addZ = z - orginZ;
        addZ = addZ < 0 ? 360 + addZ : addZ;
        addZ = addZ > 360 ? addZ - 360 : addZ;

        if (!isJust)
            addZ = 360 - addZ;
        if (Mathf.Abs(addZ) < 180f)

        {
            endQ = Quaternion.Euler(tran.localRotation.x, tran.localRotation.y, z);
            return tran.DOLocalRotateQuaternion(endQ, time);
        }

        else
        {
            if (isJust)
                endQ = Quaternion.Euler(tran.transform.localRotation.x, tran.localRotation.y, orginZ + addZ / 2f);
            else
                endQ = Quaternion.Euler(tran.transform.localRotation.x, tran.localRotation.y, orginZ + addZ);
            tran.DOLocalRotateQuaternion(endQ, time / 2).OnComplete(() =>
            {
                endQ = Quaternion.Euler(tran.localRotation.x, tran.localRotation.y, z);
                tran.DOLocalRotateQuaternion(endQ, time / 2);
            });
            return DOTween.To(() => { return 1; }, x => { }, 1, time);
        }
    }

    public static Tweener XPDoLocalRotateQuaternionY(this Transform tran, float orginY, float y, float time, bool isJust = true)
    {
        var endQ = Quaternion.Euler(tran.localRotation.x, orginY, tran.localRotation.z);
        tran.transform.localRotation = endQ;
        float addZ = y - orginY;
        addZ = addZ < 0 ? 360 + addZ : addZ;
        addZ = addZ > 360 ? addZ - 360 : addZ;

        if (!isJust)
            addZ = 360 - addZ;
        if (Mathf.Abs(addZ) < 180f)
        {
            endQ = Quaternion.Euler(tran.localRotation.x, y, tran.localRotation.z);
            return tran.DOLocalRotateQuaternion(endQ, time);
        }
        else
        {
            if (isJust)
                endQ = Quaternion.Euler(tran.transform.localRotation.x, orginY + addZ / 2f, tran.localRotation.z);
            else
                endQ = Quaternion.Euler(tran.transform.localRotation.x, orginY + addZ, tran.localRotation.z);
            tran.DOLocalRotateQuaternion(endQ, time / 2).OnComplete(() =>
            {
                endQ = Quaternion.Euler(tran.localRotation.x, y, tran.localRotation.z);
                tran.DOLocalRotateQuaternion(endQ, time / 2);
            });
            return DOTween.To(() => { return 1; }, x => { }, 1, time);
        }
    }
   
}
