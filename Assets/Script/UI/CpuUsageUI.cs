using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// CPU使用量の数値UI
/// </summary>
public class CpuUsageUI : MonoBehaviour
{
    /// <summary>
    /// CPUメイン
    /// </summary>
    [SerializeField] CpuMain m_cpuMain;

    /// <summary>
    /// 数値高い時震える幅
    /// </summary>
    [SerializeField] float m_shakeDistance = 4f;

    /// <summary>
    /// 数値変更時生成するテキストオブジェクト
    /// </summary>
    [Header("数値変更テキストのプレハブ")]
    [SerializeField] GameObject m_changedTextPrefab;

    /// <summary>
    /// 数値変更テキストの格納場所
    /// </summary>
    [Header("数値変更テキストの格納場所")]
    [SerializeField] RectTransform m_changedTextFolder;

    /// <summary>
    /// UIテキスト
    /// </summary>
    private TextMeshProUGUI m_text;

    private RectTransform m_rectTransform;

    private Vector2 m_originalPosition;

    /// <summary>
    /// 前回取得したCPU使用量
    /// </summary>
    private float m_oldUsage;

    private bool m_cleared;

    void Awake(){
        m_cpuMain.OnUsageChanged += ChangeUsageUI;
        m_text = GetComponent<TextMeshProUGUI>();
        m_text.text = ((int)m_cpuMain.Usage).ToString() + "%";
        m_rectTransform = GetComponent<RectTransform>();
        CalculatePosY(0);
        m_originalPosition = m_rectTransform.anchoredPosition;
        m_oldUsage = m_cpuMain.Usage;
        m_cleared = false;
    }

    // private void OnDestroy() {
    //     //m_cpuMain.OnUsageChanged -= ChangeUsageUI;
    // }

    /// <summary>
    /// UI数値更新
    /// </summary>
    /// <param name="usage"></param>
    public void ChangeUsageUI(float usage){
        if(m_cleared)return;
        m_text.text = usage.ToString("F1") + "%";
        if(usage > 75){
            m_text.color = Color.red;
        }
        else if(usage > 50){m_text.color = new Color(255f/255f, 165f/255f, 0);} //orange
        else if(usage > 25){m_text.color = Color.yellow;}
        else{m_text.color = Color.green;}
        CalculatePosY(usage);
        m_rectTransform.anchoredPosition = m_originalPosition;
        if(usage >= m_cpuMain.GetGameOverPercentage()){m_cleared = true;}
    }

    private void Update() {
        ShakeText();
        if(m_cleared)return;
        if(Time.frameCount % 20 == 0){
            float newUsage = m_cpuMain.Usage;
            if(Mathf.Abs(newUsage - m_oldUsage) > 0.005f){
                Instantiate(m_changedTextPrefab, m_changedTextFolder).GetComponent<ChangedCpuUsage>().SetText(newUsage - m_oldUsage);
                m_oldUsage = newUsage;
            }
        }
    }

    private void ShakeText(){
        if(m_text.color == Color.red){
            Vector2 newPos = m_originalPosition;
            newPos.x += Random.Range(-1*m_shakeDistance, m_shakeDistance);
            newPos.y += Random.Range(-1*m_shakeDistance, m_shakeDistance);
            m_rectTransform.anchoredPosition = newPos;
        }
        else{
            m_rectTransform.anchoredPosition = m_originalPosition;
        }
    }

    private void CalculatePosY(float usage){
        float usageMax100 = usage;
        if(usageMax100 > 100f) usageMax100 = 100f;
        Vector2 pos = m_rectTransform.anchoredPosition;
        pos.y = -140 + 2.4f * usageMax100;
        pos.x = -23f;
        m_rectTransform.anchoredPosition = pos;
        m_originalPosition = pos;
    }
}
