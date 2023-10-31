using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : Card
{

    /// <summary>
    /// 強調オブジェクト
    /// </summary>
    [SerializeField] private GameObject m_targetEmphasisObject;

    /// <summary>
    /// 生成した強調オブジェクト
    /// </summary>
    private GameObject m_spawnEmpasis;

    private AudioSource m_audioSource;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_image.sprite = m_sprite;
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(m_spawnFied && m_pressed)
        {
            m_image.enabled = false;
            if(m_cpuUpText)m_cpuUpText.gameObject.SetActive(false);
        }
        else
        {
            m_image.enabled = true;
            if(m_cpuUpText)m_cpuUpText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 効果発動
    /// </summary>
    /// <param name="hit"></param>
    protected override void CardEffect(RaycastHit hit)
    {
       //スポーン
        // GameObject.Find("InstantiateManager").GetComponent<InstantiateManager>().
        // InstantiateMonster(cardID, hit.point, Quaternion.identity);
        m_instantiateManager.InstantiateMonster(m_cardID, hit.point, Quaternion.identity);
        if(m_audioSource){
            m_audioSource.Play();
        }
        m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
    }

    protected override void SetCardInfoText()
    {
        //カードテキスト表示
        m_cardInfoUI.GetComponent<CardInfo>().SetVisibleCardInfo(true,m_cardName,m_cardText);
    }

        //ターゲットモンスター強調
    protected void EmphasisTarget(GameObject target)
    {
        if(m_spawnEmpasis == null)
        {
            m_spawnEmpasis = Instantiate(m_targetEmphasisObject,target.transform.position,Quaternion.identity);
        }
        else
        {
            m_spawnEmpasis.transform.position = target.transform.position;
        }
    }

    //ターゲットモンスター強調
    protected void UnEmphasisTarget()
    {
        if(m_spawnEmpasis != null)
        {
            Destroy(m_spawnEmpasis); 
        }
    }

    protected void CheckEmphasis()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool playerHit = false;

        if (Physics.Raycast(ray, out hit, 100.0f,m_layerMask) && m_pressed)
        {
            Debug.Log(hit.collider.gameObject);

            if(hit.collider.gameObject.GetComponent<PlayerMonster>() != null)
            {
                //ターゲット強調
                EmphasisTarget(hit.collider.gameObject);

                playerHit = true;
            }
        }
        
        if(playerHit == false && m_pressed == true)
        {
            //強調解除
            UnEmphasisTarget();
        }
    }
}