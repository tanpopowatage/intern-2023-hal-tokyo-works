using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class MonsterCard : Card
{

    /// <summary>
    /// プレビューオブジェクト
    /// </summary> <summary>
    /// 
    /// </summary>
    private GameObject m_previewObject;

    private AudioSource m_audioSource;


    /// <summary>
    /// プレビュー用攻撃範囲
    /// </summary>
    private GameObject m_spawnPreviewRange;
    private float m_attackdistance;
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

        //イメージを表示に設定
        m_image.enabled = true;
        if(m_cpuUpText)m_cpuUpText.gameObject.SetActive(true);

        if(m_pressed)
        {
            //プレビュー表示
            if(m_previewObject == null)
            {
                m_previewObject = m_instantiateManager.InstantiateMonsterPreview(m_cardID,new Vector3(0,1.0f,0.0f), Quaternion.identity);
                m_previewObject.GetComponent<PlayerMonster>().SetPreview(true);
                m_previewObject.GetComponent<Collider>().enabled = false;
                m_attackdistance = m_previewObject.GetComponent<Monster>().GetParamerter().attackDistance * 2;
                //攻撃範囲
                if(m_spawnPreviewRange == null)
                {
                    m_spawnPreviewRange = Instantiate((GameObject)Resources.Load ("PreViewRange"));
                    m_spawnPreviewRange.transform.localScale = new Vector3(1 * m_attackdistance,0.5f,1 * m_attackdistance);  
                }
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f,1 << LayerMask.NameToLayer("Floor")) && m_previewObject != null)
            {
                
                m_previewObject.transform.position = hit.point;
                m_spawnPreviewRange.transform.position = hit.point;
            }
            else{
                //画面上に出ないように
                m_previewObject.transform.position = new Vector3(0, -50f, 0);
                m_spawnPreviewRange.transform.position = new Vector3(0, -50f, 0);
            }
            
            //カードがフィールド上にあれば非表示
            if(m_spawnFied == true)
            {
                //image非表示に設定
                m_image.enabled = false;
                if(m_cpuUpText)m_cpuUpText.gameObject.SetActive(false);
            }
        }
        else
        {
            //プレビュー非表示
            if(m_previewObject != null)
            {   
                //Destroy(m_previewObject);
                m_previewObject.GetComponent<PlayerMonster>().SetPreview(false);
                m_previewObject.GetComponent<PlayerMonster>().m_parameter.DestroyLoad.raiseRate = 0;
                m_previewObject.GetComponent<PlayerMonster>().PreviewDeath();
                //m_instantiateManager.DestroyMonster(m_previewObject);
                m_previewObject = null;

                Destroy(m_spawnPreviewRange);
            }
        }
    }

     //効果発動
    protected override void CardEffect(RaycastHit hit)
    {
        //デバッグ用
        // CPULoad cpuLoad;
        // cpuLoad.raiseRate = 10.0f;
        // cpuLoad.impactTime = 7.0f;
        // CpuMain.Instance.UsageRegister(cpuLoad);

        GameObject monsterObj = m_instantiateManager.InstantiateMonster(m_cardID, hit.point + new Vector3(0,0.5f,0.0f), Quaternion.identity);
        if(monsterObj.GetComponent<Collider>().enabled == false){
            monsterObj.GetComponent<Collider>().enabled = true;
        }
        m_audioSource.Play();
        m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
    }

    protected override void SetCardInfoText()
    {
        MonsterParamerter mp = CardMonsterDictionary.Instance.GetMonsterParamerter(m_cardID);
        //カードテキスト表示
        m_cardInfoUI.GetComponent<CardInfo>().SetVisibleCardInfo(true, m_cardName, mp.hp.ToString(), mp.attack.ToString(), mp.speed.ToString(), m_cardText);
    }
}
