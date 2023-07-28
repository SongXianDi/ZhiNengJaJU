using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 点击播放动画
/// </summary>
public class ClickAni : OnClickObj
{
    public Animator[] animator;

    public Transform[] transforms;

    ///所有物体的所有Animator
    private List<Animator[]> animatorsList;
    ///一个物体的所有Animotor
    private List<Animator> animatorList;

    public string titleText;
    [TextArea(10, 10)]
    public string text;
    // Start is called before the first frame update
    void Start()
    {
        if (transforms.Length > 0)
        {
            //animatorsList = new List<List<Animator>>();
            animatorsList = new List<Animator[]>();
            animatorList = new List<Animator>();
            foreach (var item in transforms)
            {
                //animatorsList.Add(GetAnimatorChildren2(item));
                Animator[] x = GetAnimatorChildren2(item).ToArray();
                //animatorsList.Add(new Animator[]  GetAnimatorChildren2(item) );
                animatorsList.Add(x);
                animatorList.Clear();
            }
        }

    }
    // 递归函数，返回带 Animator 组件的子物体数组
    private List<Animator> GetAnimatorChildren2(Transform parent)
    {
        Animator animator = parent.GetComponent<Animator>();
        if (animator != null)
        {
            animatorList.Add(animator);
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            GetAnimatorChildren2(child);
        }

        return animatorList;
    }


    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        UIManager.Instance.WuTiJieShaoChage(titleText,text);
        if (animator != null)
        {            
            foreach (var item in animator)
            {
                item.SetBool("New Bool", true);
            }
        }

        if (animatorsList != null)
        {
            foreach (var item in animatorsList)
            {
                Debug.Log(item.Length);
                foreach (var item2 in item)
                {
                    item2.SetBool("New Bool", true);
                }
            }
        }
    }
}
