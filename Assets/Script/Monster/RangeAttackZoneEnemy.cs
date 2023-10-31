using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 範囲攻撃を受けるエネミーモンスターを管理するクラス
/// </summary>
public class RangeAttackZoneEnemy : MonoBehaviour
{
    [SerializeField]private EnemyMonster m_enemyMonster;

    private List<EnemyMonster> m_enemyMonstersInRange = new List<EnemyMonster>();

    /// <summary>
    /// 範囲内のEnemyMonsterのリストを返す
    /// </summary>
    /// <returns></returns>
    public List<EnemyMonster> GetEnemyMonstersInRange(){
        return m_enemyMonstersInRange;
    }

    private void OnEnable() {
        m_enemyMonstersInRange = new List<EnemyMonster>();
        AddToList(m_enemyMonster);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Enemy")){
            EnemyMonster em = other.GetComponent<EnemyMonster>();
            if(em){
                AddToList(em);
                ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Enemy")){
            EnemyMonster em = other.GetComponent<EnemyMonster>();
            if(em){
                RemoveFromList(em);
                ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
            }
        }
    }

    /// <summary>
    /// EnemyMonsterをリストに追加
    /// </summary>
    /// <param name="enemyMonster"></param>
    private void AddToList(EnemyMonster enemyMonster){
        m_enemyMonstersInRange.Add(enemyMonster);
    }

    /// <summary>
    /// EnemyMonsterをリストから削除
    /// </summary>
    /// <param name="enemyMonster"></param>
    /// <returns>削除が成功したかどうか</returns>
    private bool RemoveFromList(EnemyMonster enemyMonster){
        return m_enemyMonstersInRange.Remove(enemyMonster);
    }
}
