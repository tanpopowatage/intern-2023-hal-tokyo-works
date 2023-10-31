using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSlot : MonoBehaviour
{
    /// <summary>
    /// スロットには登録したカード
    /// </summary>
    [SerializeField] private GameObject m_card;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSlot(GameObject card)
    {
        m_card = card;
    }

    /// <summary>
    /// 登録したカードを取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetSlot()
    {
        return m_card;
    }
}
