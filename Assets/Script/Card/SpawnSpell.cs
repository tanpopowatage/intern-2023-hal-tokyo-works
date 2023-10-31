using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpell : SpellCard
{
    

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

    //効果発動
    protected override void CardEffect(RaycastHit hit)
    {
        //スポーン
        GameObject.Find("InstantiateManager").GetComponent<InstantiateManager>().
        InstantiateMonster(m_cardID, hit.point, Quaternion.identity);

        m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
    }
}
