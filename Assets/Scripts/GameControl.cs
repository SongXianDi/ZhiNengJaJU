using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public ZhuChiRen ZhuChiRen;

    //[SerializeField]
    // public SerializableDictionary<StepType,OnClickObj[]> dic;



    private void Awake()
    {

    }
    void Start()
    {
        ZhuChiRen = GameObject.FindGameObjectWithTag("Enemy").GetComponent<ZhuChiRen>();
    }



    public void StartGame()
    {
        ZhuChiRen.TiShiShow(true);
    }

    /// <summary>
    /// 主持人说完话后处理h
    /// </summary>
    /// <param name="setpType"></param>
    public void ZhuChiRenArrive()
    {
        if (GameManager.Instance.RealSetpType < StepType.End)
        {
            UIManager.Instance.ButtoniSAct(GameManager.Instance.RealSetpType);
        }

    }

    public void PlayerArrivePoint()
    {

    }
}

[System.Serializable]
public class SerializableDictionary<TKey, TValue>:IEnumerable
{
    [SerializeField]
    private List<KeyValuePair<TKey, TValue>> _list = new List<KeyValuePair<TKey, TValue>>();

    Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
    public IEnumerator GetEnumerator()
    {
        yield return dictionary;
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        foreach (KeyValuePair<TKey, TValue> kvp in _list)
        {
            dictionary.Add(kvp.Key, kvp.Value);
        }

        return dictionary;
    }
}
