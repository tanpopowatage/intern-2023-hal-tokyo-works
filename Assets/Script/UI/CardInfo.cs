using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    //モンスター用
    [SerializeField] Sprite monsterUI;
    //呪文用
    [SerializeField] Sprite spellUI;
    public RectTransform a;
    // Start is called before the first frame update
    void Start()
    {
        SetVisibleCardInfo(false,"","");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //カード情報の視認切り替え
    public void SetVisibleCardInfo(bool f,string cardName,string cardText)
    {
        if(f)
        {
            GetComponent<Image>().sprite = spellUI;
            GetComponent<Image>().color = new Color(255,255,255,0.5f);
            transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cardName;
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = cardText;
            transform.GetChild(4).gameObject.GetComponent<RectTransform >().anchoredPosition  = new Vector3(-61,-309,0);
        }
        else
        {
            GetComponent<Image>().color = new Color(255,255,255,0.0f);
            transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    //カード情報の視認切り替え
    public void SetVisibleCardInfo(bool f,string cardName, string cardHp, string cardAtk, string cardSpd, string cardText)
    {
        if(f)
        {
            GetComponent<Image>().sprite = monsterUI;
            GetComponent<Image>().color = new Color(255,255,255,0.5f);
            transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cardName;
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = cardHp;
            transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = cardAtk;
            transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = cardSpd;
            transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = cardText;
            transform.GetChild(4).gameObject.GetComponent<RectTransform >().anchoredPosition  = new Vector3(-61,-430,0);
        }
        else
        {
            GetComponent<Image>().color = new Color(255,255,255,0.0f);
            transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
