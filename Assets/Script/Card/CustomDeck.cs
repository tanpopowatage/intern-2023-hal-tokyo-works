using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDeck : MonoBehaviour
{
    /// <summary>
    /// オリジナルのデッキの中身
    /// </summary>
    [SerializeField] private List<string> m_customDeckCard;

    [SerializeField] List<DeckSlot> m_customDeckSlot;

    [SerializeField] private CustomDeckInfo m_customDeckInfo;

    [SerializeField] private GameObject m_emptyDeckWarning;

    private bool m_canEnterStage;
    public bool canEnterStage
    {
        get { return m_canEnterStage; }
        set { m_canEnterStage = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        canEnterStage = false;
        for(int i = 0;i < m_customDeckSlot.Count;i++)
        {
            m_customDeckCard.Add("");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < 3;i++)
        {   
            if(m_customDeckSlot[i].GetSlot() == null)
            {
        
            }
            else
            {
                m_customDeckCard[i] = m_customDeckSlot[i].GetSlot().name.Remove(m_customDeckSlot[i].GetSlot().name.Length - 7);
            }
        } 
    }

    public void Setting()
    {
        //Debug.Log("Setting");
        bool canSetDeck = true;
        foreach(string st in m_customDeckCard){
            if(st.Length == 0){canSetDeck = false;}
        }
        if(canSetDeck){
            CustomDeckInfo.Instance.SetCustomDeck(m_customDeckCard);
            canEnterStage = true;
        }
        else{
            m_emptyDeckWarning.SetActive(true);
        }
    } 
}
