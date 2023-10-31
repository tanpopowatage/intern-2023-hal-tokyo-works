using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// モンスターの生成を管理するクラス
/// </summary>
public class InstantiateManager : MonoBehaviour
{
    /// <summary>
    /// 画面の範囲内にあるオブジェクトのリスト
    /// </summary>
    [Header("可視リスト")]
    [SerializeField]private VisibleList m_visibleList;

    /// <summary>
    /// CPUメイン
    /// </summary>
    [Header("CPUメイン")]
    [SerializeField]private CpuMain m_cpuMain;

    private void Awake(){
        //m_visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
    }

    /// <summary>
    /// モンスターを生成する
    /// </summary>
    /// <param name="cardId">カードID</param>
    /// <param name="position">スポーン位置情報</param>
    /// <param name="rotation">スポーン時の回転情報</param>
    /// <returns>生成されたモンスター</returns>
    public GameObject InstantiateMonster(int cardId, Vector3 position, Quaternion rotation){
        GameObject monsterPrefab = CardMonsterDictionary.Instance.GetMonsterPrefab(cardId);
        GameObject monsterObj = PoolManager.Instance.GetGameObject(monsterPrefab, position, rotation);
        Monster m = monsterObj.GetComponent<Monster>();
        m.visibleList = m_visibleList;
        m.cpuMain = m_cpuMain;
        m.instantiateManager = this;
        m.m_parameter = CardMonsterDictionary.Instance.GetMonsterParamerter(cardId);
        m.m_parameter.maxHp = m.m_parameter.hp;
        m.isDead = false;
        //追加　ID設定
        m.m_parameter.monsterID = cardId;
        m_cpuMain.UsageRegister(m.m_parameter.constantLoad);
        m_cpuMain.UsageRegister(m.m_parameter.spawnLoad);
        //Debug.Log("生成 : " + m.m_parameter.spawnLoad.raiseRate);
        return monsterObj;
    }

    /// <summary>
    /// モンスターのプレビューを生成する
    /// </summary>
    /// <param name="cardId">カードID</param>
    /// <param name="position">スポーン位置情報</param>
    /// <param name="rotation">スポーン時の回転情報</param>
    /// <returns>生成されたモンスター</returns>
    public GameObject InstantiateMonsterPreview(int cardId, Vector3 position, Quaternion rotation){
        GameObject monsterPrefab = CardMonsterDictionary.Instance.GetMonsterPrefab(cardId);
        GameObject monsterObj = PoolManager.Instance.GetGameObject(monsterPrefab, position, rotation);
        
        Monster m = monsterObj.GetComponent<Monster>();
        m.visibleList = m_visibleList;
        m.cpuMain = m_cpuMain;
        m.instantiateManager = this;
        m.m_parameter = CardMonsterDictionary.Instance.GetMonsterParamerter(cardId);
        m.isDead = false;
        //追加　ID設定
        m.m_parameter.monsterID = cardId;
        //m_cpuMain.UsageRegister(m.m_parameter.spawnLoad);
        //Debug.Log("プレビュー生成 : ");
        return monsterObj;
    }

    /// <summary>
    /// モンスターを削除（オブジェクトプールにリリース）
    /// </summary>
    /// <param name="monster">モンスターのインスタンス</param>
    public void DestroyMonster(GameObject monster){
        //PoolManager.Instance.ReleaseGameObject(monster);
        m_cpuMain.UsageRegister(monster.GetComponent<Monster>().m_parameter.DestroyLoad);
        PoolManager.Instance.ReleaseGameObject(monster);
        //StartCoroutine(DestroyMonsterCoroutine(monster));
        //Debug.Log("消失 : " + monster.GetComponent<Monster>().paramerter.DestroyLoad.raiseRate);
    }

    /// <summary>
    /// モンスターが消える時のコルーチン<para>直接DisableするとOnTriggerExitが正常に呼び出せないので</para>
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    IEnumerator DestroyMonsterCoroutine(GameObject monster){
        monster.transform.position = new Vector3(500, -100, 500);
        yield return null;
        PoolManager.Instance.ReleaseGameObject(monster);
    }

    /// <summary>
    /// エフェクトの生成
    /// <para>ここでやるかは要検討</para>
    /// </summary>
    /// <param name="effectId">エフェクトID</param>
    /// <param name="position">スポーン位置情報</param>
    /// <param name="rotation">スポーン時の回転情報</param>
    public void InstantiateEffect(int effectId, Vector3 position, Quaternion rotation){
        //エフェクトの生成
    }
}
