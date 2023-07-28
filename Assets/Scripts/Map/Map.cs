using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Map : ScrollRect
{
    //缩小
    public Button reduxe;
    //放大
    public Button amplification;

    public RectTransform map;
    public Button lockPotion;

    public Transform Player;
    public Transform ZhuChiRen;
    public RectTransform PlayerMap;
    public RectTransform ZhiChiRenMap;

    public RectTransform ButoonGroup;

    public GameObject smallMap;
    public GameObject BigMap;
    //玩家在地图的实际位置
    private Vector2 playerPoint;
    private Vector2 zhuChiRenPoint;
    private float size;
    protected override void Awake()
    {
        horizontal = true;
        vertical = true;
        viewport = transform.Find("Map/Viewport").GetComponent<RectTransform>();
        content = transform.Find("Map/Viewport/RawImage").GetComponent<RectTransform>();
        reduxe = transform.Find("Map/ButtonGroup/Reduxe_Btn").GetComponent<Button>();
        amplification = transform.Find("Map/ButtonGroup/Amplif_Btn").GetComponent<Button>();
        lockPotion = transform.Find("Map/ButtonGroup/Lock_Btn").GetComponent<Button>();
        map = transform.Find("Map").GetComponent<RectTransform>(); 
        ButoonGroup = transform.Find("Map/ButtonGroup").GetComponent<RectTransform>();
        smallMap = transform.parent.Find("地图").gameObject;
        BigMap = transform.parent.Find("大地图").gameObject;

        PerSonPointInit();
    }

    private void PerSonPointInit()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        ZhuChiRen = GameObject.FindGameObjectWithTag("Enemy").transform;
        PlayerMap = transform.Find("Map/Viewport/RawImage/PlayerPoint").GetComponent<RectTransform>();
        ZhiChiRenMap = transform.Find("Map/Viewport/RawImage/ZhuChiRenPoint").GetComponent<RectTransform>();
        playerPoint = Vector2.one;
        zhuChiRenPoint = Vector2.one;
    }
    //地图宽高比
    private Vector2 vector2 = new Vector2(2, 1);
    protected override void Start()
    {
        reduxe.onClick.AddListener(() =>
        {
            smallMap.gameObject.SetActive(true);
            BigMap.gameObject.SetActive(false);
        });
        amplification.onClick.AddListener(() =>
        {
            smallMap.gameObject.SetActive(false);
            BigMap.gameObject.SetActive(true);
        });
        lockPotion.onClick.AddListener(() =>
        {
            //内容图片的锚点偏移量=玩家移动偏移量+玩家在内容图片的初始位置+玩家在视口位置的初始位置
            content.anchoredPosition = -PlayerMap.anchoredPosition - Vector2.right * (content.rect.width / 2 + viewport.rect.width / 2);
        });
    }

    private void Update()
    {
        //获取真实场景于地图尺寸比
        size = content.rect.width / 100;
        PlayerMap.rotation = Quaternion.AngleAxis(180f - Player.rotation.eulerAngles.y+40, Vector3.forward);
        playerPoint.x = Player.position.z * size;
        playerPoint.y = -Player.position.x * size;
        zhuChiRenPoint.x = ZhuChiRen.position.z * size;
        zhuChiRenPoint.y = -ZhuChiRen.position.x * size;
        ZhiChiRenMap.anchoredPosition = zhuChiRenPoint;
        if (playerPoint.x < 0.5 && playerPoint.y > -24.5 * size && playerPoint.y < 24.5 * size)
            PlayerMap.anchoredPosition = playerPoint;
    }
    public override void OnEndDrag(PointerEventData eventData)
    {

    }

    public override void OnScroll(PointerEventData eventData)
    {
        if (content.rect.width < 460)
        {
            content.offsetMin -= vector2 * Mathf.Abs(eventData.scrollDelta.y * 30);
        }
        if (content.rect.width > 2000)
        {
            content.offsetMin += vector2 * Mathf.Abs(eventData.scrollDelta.y * 30);
        }
        else
        {
            content.offsetMin -= vector2 * eventData.scrollDelta.y * 30;
        }
    }

}
