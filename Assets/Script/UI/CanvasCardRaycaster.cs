using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// UIのマウスからのレイキャストを処理するクラス
/// </summary>
public class CanvasCardRaycaster : MonoBehaviour
{
    /// <summary>
    /// CPU使用量変更プレビューUI
    /// </summary>
    [Header("CPU使用量変更プレビューUI")]
    [SerializeField] CpuUsageChangePreview m_cpuUsageChangePreview;

    [Header("CPUゲージプレビュー")]
    [SerializeField] CpuGaugePreview m_cpuGaugePreview;

    /// <summary>
    /// 今ホバーしているカード
    /// </summary>
    private Card m_nowHovering = null;

    /// <summary>
    /// RaycastAllの結果格納用List
    /// </summary>
    private List<RaycastResult> m_rayResult= new List<RaycastResult>();

    private void Update()
    {
        //カード選択中
        if(m_nowHovering != null && Input.GetMouseButton(0)){
            //選択中カードを一番上に表示
            m_nowHovering.transform.SetAsLastSibling(); 
            return;
        }

        // デバッグ用
        // if(nowHovering == null){
        //     Debug.Log("null");
        // }
        // else{
        //     Debug.Log(nowHovering.gameObject.name);
        // }

        GetRaycastResult();

        bool isHoveringCard = ProcessRaycastResult();

        SetCardsHoveredState();

        //なにもホバーしていないならnullに
        if(!isHoveringCard){
            m_nowHovering = null;
        }
        if(m_nowHovering){
            //モンスターカード
            if(m_nowHovering.GetComponent<MonsterCard>()){
                float constantLoad = CardMonsterDictionary.Instance.GetMonsterParamerter(m_nowHovering.CardId).constantLoad.raiseRate;
                m_cpuUsageChangePreview.SetText(constantLoad);
                m_cpuGaugePreview.SetAddedUsage(constantLoad);
            }
            //呪文カード
            else{
                float spawnLoad = CardMonsterDictionary.Instance.GetMonsterParamerter(m_nowHovering.CardId).spawnLoad.raiseRate;
                m_cpuUsageChangePreview.SetText(spawnLoad);
                m_cpuGaugePreview.SetAddedUsage(spawnLoad);
            }
        }
        else{
            m_cpuUsageChangePreview.SetText(0);
            m_cpuGaugePreview.SetAddedUsage(0f);
        }
    }

    /// <summary>
    /// レイキャスト処理
    /// </summary>
    private void GetRaycastResult(){
        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);

        m_rayResult.Clear();

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;
        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData , m_rayResult);
    }

    /// <summary>
    /// レイキャストの結果を処理
    /// </summary>
    /// <returns>今ホバーしているカードがあるかどうか</returns>
    private bool ProcessRaycastResult(){
        foreach (RaycastResult result in m_rayResult)
        {
            Card card = result.gameObject.GetComponent<Card>();
            if(!card)continue;

            //すでにホバーしているならこのままに
            if(m_nowHovering == card){
                card.m_hovered = true;
                return true;
            }

            //ホバーしていないならホバーにする
            if(!m_nowHovering){
                card.m_hovered = true;
                m_nowHovering = card;
                //選択中カードを一番上に表示
                card.transform.SetAsLastSibling(); 
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 他のカードをホバーしていない状態にする
    /// </summary>
    private void SetCardsHoveredState(){
        foreach(Card card in GetComponentsInChildren<Card>()){
            if(card == m_nowHovering)continue;
            card.m_hovered = false;
        }
    }
}
