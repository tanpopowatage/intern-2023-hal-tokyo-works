using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成マネージャ")]
    [SerializeField] private InstantiateManager m_instantiateManager;

    [Header("エネミーマネージャ")]
    [SerializeField] private EnemyManager m_enemyManager;
    
    /// <summary>
    /// 一体の敵をスポーン
    /// </summary>
    /// <param name="monsterId">敵のID</param>
    public void SpawnEnemy(int monsterId){
        Vector3 pos = GetRandomPosition(GetComponent<Collider>().bounds);
        GameObject monster = m_instantiateManager.InstantiateMonster(monsterId, pos, Quaternion.identity);
        monster.GetComponent<EnemyMonster>().m_enemyManager = m_enemyManager;
        m_enemyManager.RegisterEnemy();
    }

    /// <summary>
    /// 複数の敵を同時にスポーン
    /// </summary>
    /// <param name="monsterId">敵のID</param>
    /// <param name="loopCount">生成する敵の数</param>
    public void SpawnEnemies(int monsterId, int loopCount){
        for(int i = 0; i < loopCount; i++){
            SpawnEnemy(monsterId);
        }
    }

    /// <summary>
    /// 自分のコライダーの範囲内のランダムな位置を返す（高さは一番低い方）
    /// </summary>
    /// <param name="bounds">コライダーのバウンド</param>
    /// <returns>コライダー内のランダムな位置</returns>
    private Vector3 GetRandomPosition(Bounds bounds){
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0.5f,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    // private void Start() {
    //     //Debug.Log(GetComponent<SignalReceiverWithTwoInt>());
    //     //Debug.Log(GetComponent<SignalReceiverWithInt>());
    // }
}
