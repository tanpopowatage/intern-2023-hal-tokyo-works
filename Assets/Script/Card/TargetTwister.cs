using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTwister : SpellCard
{
    /// <summary>
    /// ドロー回数
    /// </summary>
    [SerializeField] protected int m_drawCount = 2;
    
    /// <summary>
    /// ドローするカード
    /// </summary>
    [SerializeField] GameObject m_drawCard;
    protected override void  Start()
    {
        base.Start();
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    //効果発動
    protected override void CardEffect(RaycastHit hit)
    {
        for(int i = 0; i < m_drawCount;i++)
        {
            m_hands.GetComponent<Hands>().TargetTwisterDraw(m_drawCard);
        }
        m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
    }
}
