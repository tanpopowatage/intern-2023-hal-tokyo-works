using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiraWallCard : SpellCard
{
    /// <summary>
    /// 範囲
    /// </summary>
    [SerializeField] private GameObject m_rangeObject;

    /// <summary>
    /// 生成した範囲オブジェクト
    /// </summary> <summary>
    /// 
    /// </summary>
    private GameObject m_spawRangeObject;
    
    /// <summary>
    /// 生成したフラグ
    /// </summary>
    private bool m_spawnFlag = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(m_pressed && !m_spawnFlag)
        {
            m_spawRangeObject = Instantiate(m_rangeObject);
            m_spawnFlag = true;
        }
        else if(m_pressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f,1 << LayerMask.NameToLayer("Floor")))
            {
                m_spawRangeObject.transform.position = hit.point;
            }

        }
        else
        {
            if(m_spawRangeObject)
            {
                m_spawnFlag = false;
                Destroy(m_spawRangeObject);
            }
        }
    }
}
