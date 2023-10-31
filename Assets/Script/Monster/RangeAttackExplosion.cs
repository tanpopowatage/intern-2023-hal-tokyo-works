using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆風オブジェクトを制御するクラス
/// </summary>
public class RangeAttackExplosion : MonoBehaviour
{
    /// <summary>
    /// 生存時間（秒）
    /// </summary>
    [SerializeField]private float m_lifeTime = 1.5f;
    void Start()
    {
        Destroy(this.gameObject, m_lifeTime);
    }
}
