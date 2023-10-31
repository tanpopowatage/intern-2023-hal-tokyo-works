using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UITimer : MonoBehaviour
{
    [Header("クリアまでの秒数")]
    [SerializeField]private float m_clearTime = 30f;

    /// <summary>
    /// タイマーが有効なのか
    /// </summary>
    public bool m_timerActive;

    /// <summary>
    /// タイマーのテキスト
    /// </summary>
    private TextMeshProUGUI m_timerText;

    /// <summary>
    /// 現在の時間
    /// </summary>
    private float m_currentTime;

    /// <summary>
    /// タイマーがクリア時間になったら呼ぶ関数
    /// </summary>
    public event Action OnTimerZero = delegate{};

    // Start is called before the first frame update
    void Start()
    {
        m_currentTime = 0f;
        m_timerActive = true;
        m_timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_timerActive){
            m_currentTime += Time.deltaTime;
            SetUIText();
            if(m_currentTime >= m_clearTime){
                OnTimerZero();
                m_timerActive = false;
            }
        }
    }

    /// <summary>
    /// タイマーテキストを更新
    /// </summary>
    private void SetUIText(){
        float countdownTime = m_clearTime - m_currentTime;
        if(countdownTime < 0f){countdownTime = 0f;}
        int seconds = Mathf.FloorToInt(countdownTime);
        int mseconds = Mathf.FloorToInt((countdownTime - seconds) * 100);

        string uiTime = string.Format("{0:00}:{1:00}", seconds, mseconds);
        m_timerText.text = uiTime;
    }
}
