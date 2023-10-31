using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CpuUsageChangePreview : MonoBehaviour
{
    private TextMeshProUGUI m_text;

    private void Start() {
        m_text = GetComponent<TextMeshProUGUI>();
        m_text.text = "";
    }

    /// <summary>
    /// テキストを設定<para>change = 0: 非表示</para>
    /// </summary>
    /// <param name="change"></param>
    public void SetText(float change){
        if(m_text == null)return;
        
        if(change == 0){
            m_text.text = "";
            return;
        }
        Color color;
        if(change > 0){
            color = Color.red;
            m_text.color = color;
            m_text.text = "+" + change.ToString("F1");
        }
        else{
            color = Color.blue;
            m_text.color = color;
            m_text.text = "-" + Mathf.Abs(change).ToString("F1");
        }
    }
}
