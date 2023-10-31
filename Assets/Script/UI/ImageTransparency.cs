using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画像のα値を調整するクラス
/// </summary>
public class ImageTransparency : MonoBehaviour
{
    [Header("画像のα値")]
    [SerializeField] private float alpha = 1f;

    private Image m_image;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        Color color = m_image.color;
        color.a = alpha;
        m_image.color = color;
    }

}
