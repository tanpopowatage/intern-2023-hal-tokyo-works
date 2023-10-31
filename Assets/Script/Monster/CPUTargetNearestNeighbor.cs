using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUTargetNearestNeighbor : EffectMonster
{
    // Start is called before the first frame update

    /// <summary>
    /// 対象となっているモンスター
    /// </summary> <summary>
    /// 
    /// </summary>
    /// <typeparam name="PlayerMonster"></typeparam>
    /// <returns></returns>
    private List<PlayerMonster> m_monsters = new List<PlayerMonster>();
    
    /// <summary>
    /// 上昇する間隔
    /// </summary>
    private float m_interval = 1;
    
    /// <summary>
    /// 効果が切れる時間
    /// </summary>
    private float m_lifeTime = 5;

    /// <summary>
    /// 上昇するパラメータ
    /// </summary>
    private UpType m_type;

    /// <summary>
    /// パラメーター上昇量
    /// </summary>
    private float m_value;
    protected override void Start()
    {
        foreach (GameObject obj in GameObject.Find("Managers").GetComponent<VisibleList>().GetVisibleList())
        {
            PlayerMonster pm = obj.GetComponent<PlayerMonster>();
            if(pm != null)
            {
                m_monsters.Add(pm);
            }
        }

        InvokeRepeating("PowerUp",0,m_interval);
        Invoke("Death",m_lifeTime);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    /// <summary>
    /// パラメーター上昇
    /// </summary> <summary>
    /// 
    /// </summary>
    private void PowerUp()
    {
        foreach(PlayerMonster pm in m_monsters)
        {
            //パラメータによって演出を変更
            switch(m_type)
            {
                case UpType.HP:
                pm.UpHP(m_value);
                break;

                case UpType.Speed:
                pm.UpSpeed(m_value);
                break;

                case UpType.Attack:
                pm.UpAttack(m_value);
                break;

                case UpType.CoolTime:
                pm.UpCoolTime(m_value);
                break;

            }
        }
    }

    /// <summary>
    /// 効果の対象と上昇量設定
    /// </summary>
    /// <param name="t"></param>
    /// <param name="v"></param>
    public void SetParamerter(UpType t,float v)
    {
        m_type = t;
        m_value = v;
    }
}
