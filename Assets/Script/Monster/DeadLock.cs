using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLock : EffectMonster
{
    /// <summary>
    /// 効果が切れるまでの時間
    /// </summary> <summary>
    /// 
    /// </summary>
    [Tooltip("自滅するまでの時間")]
    [SerializeField] private float m_lifeTime = 5;

    /// <summary>
    /// 敵に与えるダメージ量
    /// </summary>
    [Tooltip("ダメージ量")]
    [SerializeField] private float m_damage = 20;

    /// <summary>
    /// ロックした敵
    /// </summary> <summary>
    /// 
    /// </summary>
    private GameObject m_lockEnemy;
    
    /// <summary>
    /// ロックしているかのフラグ
    /// ロックしていたらTRUE
    /// </summary>
    bool m_lockFlag = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyMonster>() != null && m_lockFlag == false)
        {
            //ロックした敵設定
            m_lockEnemy = other.gameObject;
            m_lockFlag = true;

            m_lockEnemy.GetComponent<EnemyMonster>().SetStatus(Status.stop);

            //ダメージ
            m_lockEnemy.GetComponent<EnemyMonster>().ChangeHP(m_damage);

            Invoke("Death",m_lifeTime);
        }
    }

    public override void Death()
    {
        if(m_lockEnemy != null)
        {
            m_lockEnemy.GetComponent<EnemyMonster>().SetStatus(Status.move);
        }
        
        Destroy(this.gameObject);
    }
}
