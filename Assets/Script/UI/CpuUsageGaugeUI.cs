using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// CPU使用量のゲージUIを管理するクラス
/// <para>更新：線形補間を行わないように修正（頻繁に更新があると追いつけないため）</para>
/// </summary>
public class CpuUsageGaugeUI : MonoBehaviour
{
    /// <summary>
    /// CPUメイン
    /// </summary>
    [Header("CPUメイン")]
    [SerializeField] CpuMain m_cpuMain;

    // [Header("線形補間のかかる時間")]
    // [SerializeField]float fillLerpTime = 0.25f;

    /// <summary>
    /// ゲージのスプライト
    /// </summary>
    private Image m_image;

    // //画像のフィル量
    // private float oldFillAmount, newFillAmount;
    // //線形補間の現在時間
    // private float nowLerpTime = 0f;

    private void Start(){
        m_image = GetComponent<Image>();
        //イベント登録
        m_cpuMain.OnUsageChanged += ChangeUsageUI;
        m_image.fillAmount = 0f;
    }

    // private void Update() {
    //     if(oldFillAmount != newFillAmount){
    //         FillAmountLerpSmooth();
    //     }
    // }

    private void OnDestroy() {
        //なくてもいけそう
        m_cpuMain.OnUsageChanged -= ChangeUsageUI;
    }

    // //スプライトのフィル量の線形補間（スムーズ版）
    // private void FillAmountLerpSmooth(){
    //     image.fillAmount = Mathf.Lerp(oldFillAmount, newFillAmount, Mathf.SmoothStep(0f, 1f, nowLerpTime / fillLerpTime));
    //     if(nowLerpTime >= fillLerpTime){
    //         oldFillAmount = newFillAmount;
    //     }
    //     nowLerpTime += Time.unscaledDeltaTime;
    // }

    /// <summary>
    /// ゲージのフィル量を変わる
    /// </summary>
    /// <param name="usage">現在のCPU使用量</param>
    public void ChangeUsageUI(float usage){
        // nowLerpTime = 0f;
        // //最大100想定
        // newFillAmount = usage / 100;
        m_image.fillAmount = usage / 100;
    }
}
