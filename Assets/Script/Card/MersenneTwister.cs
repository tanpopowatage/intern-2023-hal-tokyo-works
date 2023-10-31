using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MersenneTwister : SpellCard
{
    /// <summary>
    /// ドロー回数
    /// </summary>
    [SerializeField] private int m_drawCount = 2;
    
    // Start is called before the first frame update
    protected override void  Start()
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
        for(int i = 0; i < m_drawCount;i++)
        {
            m_hands.GetComponent<Hands>().MersenneTwisterDraw();
        }
        m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
    }
}
