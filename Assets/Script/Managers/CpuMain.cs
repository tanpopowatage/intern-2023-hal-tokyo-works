using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// CPU使用量の管理
/// </summary>
public class CpuMain : MonoBehaviour
{
    
    private float m_usage;

    /// <summary>
    /// CPU使用量（0～100）
    /// </summary>
    public float Usage
    {
        get { return m_usage; }
        set { m_usage = value; }
    }

    /// <summary>
    /// 使用率変わった時呼ぶイベント
    /// </summary>
    public event Action<float> OnUsageChanged = delegate{};

    /// <summary>
    /// 使用率が100%になった時呼ぶイベント
    /// </summary>
    public event Action OnUsageFull = delegate{};

    /// <summary>
    /// カウントダウンが始まるパーセンテージ
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField]private float m_countdownStartPercentage = 100;

    /// <summary>
    /// 強制ゲームオーバーになるパーセンテージ
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField]private float m_gameoverPercentage = 120;

    [SerializeField]private float m_countDownTime = 10;

    /// <summary>
    /// カウントダウンフラグ
    /// </summary>
    [SerializeField] bool m_countDown = false;

    private AudioSource m_audioSource;

    private bool m_isPlayedSound;

    private void Awake() {
        m_usage = 0;
        m_audioSource = GetComponent<AudioSource>();
        m_isPlayedSound = false;
    }

    /// <summary>
    /// CPU使用量の変化（内部処理）
    /// </summary>
    /// <param name="amount">変化量</param>
    private void UsageChange(float amount){
        if(!m_isPlayedSound){
            if(m_usage < 75f && m_usage + amount >= 75f){
                m_audioSource.Play();
                m_isPlayedSound = true;
                Invoke("CanPlaySound", 1f);
            }
        }
        m_usage += amount;
        if(m_usage >= m_gameoverPercentage){
            m_usage = m_gameoverPercentage;
            //UI更新・クリア処理
            OnUsageChanged(m_usage);
            OnUsageFull();
            return;
        }else if(m_usage >= m_countdownStartPercentage){
            m_countDown = true;
        }else{
            m_countDown = false;
        }
        if(m_usage <= 0){
            m_usage = 0;
        }
        //UI更新
        OnUsageChanged(m_usage);
    }

    /// <summary>
    /// CPU使用率に変動を登録
    /// <para>下がらない変動はcpuLoad.impactTime = -1に登録</para>
    /// </summary>
    /// <param name="cpuLoad">CPU負荷単位</param>
    public void UsageRegister(CPULoad cpuLoad){
        UsageChange(cpuLoad.raiseRate);

        //常時上昇ならここまで
        if(cpuLoad.impactTime < 0)return;

        StartCoroutine(RemoveUsage(cpuLoad));
    }

    /// <summary>
    /// impactTime時間後登録した使用量を戻るコルーチン
    /// </summary>
    /// <param name="cpuLoad">CPU負荷単位</param>
    /// <returns></returns>
    IEnumerator RemoveUsage(CPULoad cpuLoad){
        yield return new WaitForSeconds(cpuLoad.impactTime);
        UsageChange(-1 * cpuLoad.raiseRate);
    }

    public float GetGameOverPercentage(){return m_gameoverPercentage;} 
    public bool GetCountDownFlag(){return m_countDown;}
    public float GetCotuntDownTime(){return m_countDownTime;}

    private void CanPlaySound(){m_isPlayedSound = false;}
}
