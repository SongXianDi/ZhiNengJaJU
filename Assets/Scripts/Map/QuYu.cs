using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuYu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject o;
    private void Awake()
    {
        o = transform.GetChild(0).gameObject;
        o.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
        o.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        o.gameObject.SetActive(false);
    }
}
