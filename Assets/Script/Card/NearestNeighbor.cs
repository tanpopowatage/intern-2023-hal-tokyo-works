using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//カード名:ニアレストレイバー
//カード効果:味方モンスター1体の攻撃力+2

public enum UpType
{
    HP,
    Speed,
    Attack,
    CoolTime
}

public class NearestNeighbor : SpellCard
{

    /// <summary>
    /// 上昇対象
    /// </summary>
    [Tooltip("上昇対象")]
    [SerializeField] private UpType m_type;

    /// <summary>
    /// 上昇量
    /// </summary>
    [Tooltip("上昇量")]
    [SerializeField] private float m_value;

    // Start is called before the first frame update
    protected override void  Start()
    {
        base.Start();
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //ターゲット強調
        CheckEmphasis();

    }

    //効果発動
    protected override void CardEffect(RaycastHit hit)
    {
        if(hit.collider.gameObject.GetComponent<PlayerMonster>() != null && hit.collider.gameObject.GetComponent<PlayerBossMonster>() == null)
        {
            //パラメータによって演出を変更
            switch(m_type)
            {
                case UpType.HP:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpHP(m_value);
                break;

                case UpType.Speed:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpSpeed(m_value);
                break;

                case UpType.Attack:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpAttack(m_value);
                break;

                case UpType.CoolTime:
                hit.collider.gameObject.GetComponent<PlayerMonster>().UpCoolTime(m_value);
                break;

            }
            m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);

            //強調削除
            UnEmphasisTarget();
        }
        else if(hit.collider.gameObject.GetComponent<PlayerBossMonster>() != null)
        {
            //CPU対象
            //obj生成＆パラメータ設定（種類、上昇量）
            GameObject obj =  m_instantiateManager.InstantiateMonster(11, hit.point, Quaternion.identity);
            obj.GetComponent<CPUTargetNearestNeighbor>().SetParamerter(m_type,m_value);
            m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);

            //強調削除
            UnEmphasisTarget();
        }
    }
}
