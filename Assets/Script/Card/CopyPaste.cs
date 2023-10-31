using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPaste : SpellCard
{
    /// <summary>
    ///生成される距離
    /// </summary>
    [Tooltip("半径")]
    [SerializeField] private float m_spawnRange = 2;

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
        if(hit.collider.gameObject.GetComponent<PlayerMonster>() != null)
        {
            
            Vector3 direction;
            Vector3 spawnPosition = hit.point;

            direction.x = Random.Range(-1.0f,1.0f);
            direction.z = Random.Range(-1.0f,1.0f);
            direction.y = 0;

            spawnPosition += direction * m_spawnRange;

            //スポーン
            GameObject.Find("Managers").GetComponent<InstantiateManager>().
            InstantiateMonster(hit.collider.gameObject.GetComponent<Monster>().m_parameter.monsterID, spawnPosition, Quaternion.identity);

            m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);

            //強調テキスト削除
            UnEmphasisTarget();
        }
    }
}
