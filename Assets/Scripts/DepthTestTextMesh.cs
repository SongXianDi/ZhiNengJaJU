using UnityEngine;

public class DepthTestTextMesh : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(1);
        Renderer textRenderer = GetComponent<Renderer>();
        textRenderer.material.SetFloat("_ZWrite", 1); // 启用深度测试
    }
}
