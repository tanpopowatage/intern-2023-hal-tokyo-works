using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterObject : EffectMonster
{
    /// <summary>
    /// ダメージを与える距離
    /// </summary>
    [Tooltip("ダメージを与える範囲")]
    [SerializeField] private float m_damageRange = 3.0f;
    
    /// <summary>
    /// 敵に与えるダメージ量
    /// </summary>
    [Tooltip("ダメージ量")]
    [SerializeField] private float m_damage = 3.0f;

    /// <summary>
    /// 爆発エフェクト
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField] private GameObject m_explosion;
    // Start is called before the first frame update
    protected override void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if(obj.tag == "Enemy")
            {
                //距離で比較
                float distance = Vector3.Distance(obj.transform.position,transform.position);
                if(distance < m_damageRange)
                {
                    obj.GetComponent<EnemyMonster>().ChangeHP(m_damage);
                }
            }
        }

        Instantiate(m_explosion,transform.position,Quaternion.identity);

        Death();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
