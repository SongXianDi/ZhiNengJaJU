using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetMaskLoad : MonoBehaviour
{
    public enum LoadState
    {
        Load,
        Error,
        Win
    }
    public class LoadData
    {
        public Action Action;
        public float Time;
    }
    private float _startRequestTime = 0;

    private float _loadTime = 0;
    private GameObject _bg;
    private Action _finishReqestEvent;
    private Queue<LoadData> _allActionQ;
    private Tweener _loopT;
    public static SetMaskLoad Instance => _instance;
    private static SetMaskLoad _instance;
    private Text _conentText;
    private Button _button;
    private GameObject _stateObj;

    private void Awake()
    {
        _instance = this;
        _bg = transform.Find("BG").gameObject;
        _conentText = _bg.transform.Find("Plane/Text").GetComponent<Text>();
        _button = _bg.transform.Find("Plane/Button").GetComponent<Button>();
        _stateObj = _bg.transform.Find("Plane/State").gameObject;
        _allActionQ = new Queue<LoadData>();

        _bg.SetActive(false);
    }
    private void Update()
    {
        if (_allActionQ.Count > 0)
        {
            if (Time.time - _startRequestTime > _loadTime)
            {
                var data = _allActionQ.Dequeue();
                _startRequestTime = Time.time;
                _loadTime = data.Time;
                data.Action?.Invoke();
            }
        }
    }


    public void StartMaskText(string str, bool isButton = false, Action onClick = null, LoadState loadState = LoadState.Load,
                             float defaultLoadTime = 3, float defaultCloseTime = 0.1f, Action<GameObject> closeEvent = null)
    {
        LoadData data = new LoadData();
        data.Action = () =>
        {
            Set(str, defaultLoadTime, isButton, onClick, loadState);
        };
        data.Time = defaultLoadTime;
        _allActionQ.Enqueue(data);
        if (closeEvent != null)
        {
            LoadData data1 = new LoadData();
            data1.Action = () =>
            {
                closeEvent?.Invoke(_bg.gameObject);
            };
            data1.Time = defaultCloseTime;
            _allActionQ.Enqueue(data1);
        }
    }
    private void Set(string str, float defaultloadtime, bool isButton = false, Action onClick = null, LoadState loadState = LoadState.Load)
    {
        if (_conentText.text != str)
        {
            if (_loopT != null)
                _loopT.Kill();
            if (defaultloadtime < 1)
                _conentText.text = str;
            else
            {
                _conentText.text = "";
                _loopT = _conentText.DOText(str, str.Length / 5f).SetEase(Ease.Linear).SetLoops(-1);
            }

        }
        SetStateObj(loadState);
        _bg.gameObject.SetActive(true);
        _button.gameObject.SetActive(isButton);
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => { _bg.gameObject.SetActive(false); _startRequestTime = -100; onClick?.Invoke(); });

    }
    public void Hide()
    {
        _bg.SetActive(false);
    }

    private void SetStateObj(LoadState loadState)
    {
        foreach (Transform tran in _stateObj.transform)
        {
            tran.gameObject.SetActive(loadState.ToString() == tran.name);
        }
    }
}
