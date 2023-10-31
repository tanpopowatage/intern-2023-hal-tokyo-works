using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDeckInfo : SingletonMonoBehaviour<CustomDeckInfo>
{
    [SerializeField] private List<string> m_customDeckCard;

    private bool m_useCustomDeck;
    public bool useCustomDeck
    {
        get { return m_useCustomDeck; }
        set { m_useCustomDeck = value; }
    }
    

    public void SetCustomDeck(List<string> deck)
    {
        m_customDeckCard = deck;
    }

    public List<string> GetCustomDeck()
    {
        return m_customDeckCard;
    }
}
