using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCopyPaste : EffectMonster
{
    /// <summary>
    /// 生成する距離
    /// </summary>
    [Tooltip("生成する半径")]
    [SerializeField] private float m_spawnRange = 2;

    /// <summary>
    /// 生成マネージャー
    /// </summary> <summary>
    /// 
    /// </summary>
    private InstantiateManager m_InstantiateManager;
    // Start is called before the first frame update
    protected override void Start()
    {
        m_InstantiateManager = GameObject.Find("Managers").GetComponent<InstantiateManager>();

        //シーン上のPlayerMonster収集&生成
        foreach(GameObject obj in GameObject.Find("Managers").GetComponent<VisibleList>().GetVisibleList())
        {
            if(obj == null)continue;
            if(obj.tag == "Player")
            {
                Spawn(obj,obj.GetComponent<PlayerMonster>().m_parameter.monsterID);
            }
        }

        Death();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    private void Spawn(GameObject obj,int id)
    {
        Vector3 direction;
        Vector3 spawnPosition = obj.transform.position;
        //ランダムに方向設定
        direction.x = Random.Range(-1.0f,1.0f);
        direction.z = Random.Range(-1.0f,1.0f);
        direction.y = 0;

        spawnPosition += direction * m_spawnRange;

        //スポーン
        m_InstantiateManager.InstantiateMonster(id, spawnPosition, Quaternion.identity);
    }
}
