using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaemonOrbObject : EffectMonster
{
    /// <summary>
    /// 移動速度
    /// </summary>
    [Tooltip("移動速度")]
    [SerializeField] public float m_speed = 2;
    /// <summary>
    /// 敵にヒットしたときに与えるダメージ量
    /// </summary>
    [Tooltip("攻撃力")]
    [SerializeField] public float m_damage = 20;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        //移動
        transform.position += m_speed * new Vector3(0,0,1) * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyMonster em = collision.gameObject.GetComponent<EnemyMonster>();
        if(em != null)
        {
            //ダメージ
            em.ChangeHP(m_damage);
            
            Death();
        }
    }
}
