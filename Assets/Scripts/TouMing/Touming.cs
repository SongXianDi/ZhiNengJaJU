using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RenderingMode1
{
    Opaque,
    Cutout,
    Fade,
    Transparent,
}

public class Touming : MonoBehaviour
{
    List<Material> marerialList = new List<Material>();
    Color YuanbenColor = Color.white;
    Color YellowColor = Color.yellow;
    private void Awake()
    {
        marerialList = new List<Material>(GetComponent<MeshRenderer>().materials);
        YuanbenColor = marerialList[0].color;
        SetZhencangColor();
    }
    /// <summary>
    /// 透明
    /// </summary>
    public void SetToumingColor()
    {
        foreach (var mareria in marerialList)
        {
            SetMaterialRenderingMode(mareria, RenderingMode1.Fade);
            if (name == "Guntong")
                mareria.color = new Color(YuanbenColor.r, YuanbenColor.g, YuanbenColor.b, 0 / 255f);
            else
                mareria.color = new Color(YuanbenColor.r, YuanbenColor.g, YuanbenColor.b, 100 / 255f);
        }
    }
    /// <summary>
    /// 变成黄色
    /// </summary>
    public void SetYellowColor()
    {
        foreach (var mareria in marerialList)
        {
            SetMaterialRenderingMode(mareria, RenderingMode1.Opaque);
           
            mareria.color = YellowColor;
        }
    }

    /// <summary>
    /// 回复正常
    /// </summary>
    public void SetZhencangColor()
    {
        foreach (var mareria in marerialList)
        {
            SetMaterialRenderingMode(mareria, RenderingMode1.Opaque);
            mareria.color = YuanbenColor;
        }
    }

    public static void SetMaterialRenderingMode(Material material, RenderingMode1 renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode1.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode1.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode1.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode1.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
