using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    /// <summary>
    /// カード位置
    /// </summary>
    private List<Vector2> m_cardPositions = new List<Vector2>(3);

    /// <summary>
    /// 最大枚数
    /// </summary>
    [SerializeField] int maxCount = 5;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    //指定したカードのポジション取得
    public Vector2 GetCardPosition(int n)
    {
        if(n > maxCount)
        {
            return m_cardPositions[n];
        }
        //エラー
        else
        {
            return new Vector2(-1,-1);
        }
        
    }

    //カードスロット追加
    public void AddCardSlot(Vector2 n)
    {
        if(m_cardPositions.Count < maxCount)
        {
            m_cardPositions.Add(n);
        }
    }

    //カードスロット減少
    public void DecCardSlot(int n)
    {
        if(m_cardPositions.Count > 0)
        {
            m_cardPositions.RemoveAt(n);
        }
    }
}
