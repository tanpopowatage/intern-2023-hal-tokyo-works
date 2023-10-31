using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCrossCard : SpellCard
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 効果発動
    /// </summary>
    /// <param name="hit"></param>
    protected override void CardEffect(RaycastHit hit)
    {
        //CPUかエネミー
        if(hit.collider.gameObject.GetComponent<PlayerBossMonster>() != null || hit.collider.gameObject.GetComponent<EnemyMonster>() != null)
        {
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                PlayerMonster pm = obj.GetComponent<PlayerMonster>();

                if(pm != null)
                {
                    pm.SetStatus(Status.ucm);
                    pm.SetTarget(hit.collider.gameObject);
                }
            }
            m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
        }

        
    }
}
