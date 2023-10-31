using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FireWallObject : EffectMonster
{
    /// <summary>
    /// 効果が切れるまでの時間
    /// </summary>
    [Tooltip("自滅するまでの時間")]
    [SerializeField] private float m_lifeTime = 10;
    
    /// <summary>
    /// ダメージを与える間隔
    /// </summary>
    [Tooltip("ダメージ間隔")]
    [SerializeField] private float m_damageInterval = 2;

    /// <summary>
    /// ダメージ量
    /// </summary> <summary>
    /// 
    /// </summary>
    [Tooltip("ダメージ量")]
    [SerializeField] private float m_damage = 2;

    /// <summary>
    /// ダメージを与える敵リスト
    /// </summary> <summary>
    /// 
    /// </summary>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    List<GameObject> m_targetEnemys = new List<GameObject>();

    /// <summary>
    /// ターゲットの初期スピード
    /// </summary>
    /// <typeparam name="float"></typeparam>
    /// <returns></returns>
    List<float> m_targetInitSpeed = new List<float>();
    
    /// <summary>
    /// ダメージを与えるフラグ
    /// </summary>
    private bool m_damageFlag = false;

    /// <summary>
    /// 点滅させるため
    /// </summary>
    private MeshRenderer m_meshRenderer;

    /// <summary>
    /// 点滅させるため
    /// </summary>
    private GameObject m_FlashObject;

    /// <summary>
    /// 生成から何秒後に点滅し始めるか
    /// </summary>
    [SerializeField] private float m_flashTime = 8.0f;

    /// <summary>
    /// 点滅する間隔
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField] private float m_FlashInterval = 0.1f;
    protected override void Start()
    {
        base.Start();

        m_meshRenderer = GetComponent<MeshRenderer>();
        m_FlashObject = transform.GetChild(0).gameObject;
        
        Invoke("Death", m_lifeTime);
        InvokeRepeating("Flash",m_flashTime,m_FlashInterval);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!m_targetEnemys.Contains(other.gameObject))
            {
                ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

                //ターゲットに追加
                m_targetEnemys.Add(other.gameObject);
                
                //ターゲット
                EnemyMonster em = other.gameObject.GetComponent<EnemyMonster>();

                //速度を0にする
                MonsterParamerter par = em.GetParamerter();
                m_targetInitSpeed.Add(par.speed);
                par.speed = 0;
                em.SetParamerter(par);
                
                if (!m_damageFlag)
                {
                    m_damageFlag = true;
                    InvokeRepeating("Damage", m_damageInterval, m_damageInterval);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Enemy")
        {
            if (!m_targetEnemys.Contains(other.gameObject))
            {
                m_targetEnemys.Remove(other.gameObject);
                ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
            }
        }
    }

    private void Damage()
    {
        foreach (GameObject obj in m_targetEnemys) 
        {
            //ターゲットにダメージ
            if(obj != null)
            {
                obj.GetComponent<EnemyMonster>().ChangeHP(m_damage);
            }
        }
    }

    public override void Death()
    {
        int i = 0;

        foreach(GameObject obj in m_targetEnemys)
        {
            EnemyMonster em = obj.GetComponent<EnemyMonster>();
            MonsterParamerter par = em.GetParamerter();
            par.speed = m_targetInitSpeed[i];
            em.SetParamerter(par);

            i++;
        }

        Destroy(this.gameObject);
    }

    /// <summary>
    /// 点滅
    /// </summary>
    public void Flash()
    {
        m_FlashObject.SetActive(!m_FlashObject.activeSelf);
    }
}