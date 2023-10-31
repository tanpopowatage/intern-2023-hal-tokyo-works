using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArrows : MonoBehaviour
{
    private Hands m_hands;

    private int m_maxArrows;

    // Start is called before the first frame update
    void Start()
    {
        m_hands = GameObject.Find("Hands").GetComponent<Hands>();
        m_hands.OnCardCountChanged += SetArrows;
        m_maxArrows = m_hands.GetMaxCount();
        foreach(Transform arrow in transform){
            arrow.gameObject.SetActive(false);
        }
    }

    public void SetArrows(int amount){
        for(int i = 0; i < m_maxArrows; i++){
            transform.GetChild(i).gameObject.SetActive(i < amount);
        }
    }
}
