using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    private int m_enemyNum;
    /// <summary>
    /// 場面上にある敵の数
    /// </summary>
    public int enemyNum
    {
        get { return m_enemyNum; }
        set { m_enemyNum = value; }
    }

    private bool m_isCleared;
    /// <summary>
    /// 既にクリアしたか
    /// </summary>
    public bool isCleared
    {
        get { return m_isCleared; }
        set { m_isCleared = value; }
    }

    private bool m_isAllEnemySpawned;
    /// <summary>
    /// 全部の敵が生成されたか
    /// </summary>
    public bool isAllEnemySpawned
    {
        get { return m_isAllEnemySpawned; }
        set { m_isAllEnemySpawned = value; }
    }
    
    
    /// <summary>
    /// 敵を全滅する時呼ぶイベント
    /// </summary>
    public event Action OnAllEnemyCleared = delegate{};

    /// <summary>
    /// 敵の数を1増やす
    /// </summary>
    public void RegisterEnemy(){
        m_enemyNum++;
    }
    
    /// <summary>
    /// 敵の数を1減らす、0になったらクリア処理
    /// </summary>
    public void UnregisterEnemy(){
        m_enemyNum--;
        //まだ出し切っていない時全滅してもクリアにならない
        if(!isAllEnemySpawned)return;
        //一回だけ呼ぶ
        if(m_enemyNum <= 0){
            if(m_isCleared)return;
            OnAllEnemyCleared();
            m_isCleared = true;
        }
    }

    /// <summary>
    /// 敵が全部生成された時呼ぶ関数
    /// </summary>
    public void SetAllEnemySpawned(){
        m_isAllEnemySpawned = true;
    }

    private void Start() {
        m_enemyNum = 0;
        m_isAllEnemySpawned = false;
        m_isCleared = false;
    }
}
