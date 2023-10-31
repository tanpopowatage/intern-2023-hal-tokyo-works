using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;


public class Card : MonoBehaviour
{
    /// <summary>
    /// カード名
    /// </summary>
    [SerializeField] protected string m_cardName;
    
    /// <summary>
    /// カードの詳細テキスト
    /// </summary>
    [SerializeField] protected string m_cardText;

    /// <summary>
    /// スプライト
    /// </summary>
    [SerializeField] protected Sprite m_sprite;

    /// <summary>
    /// カードID
    /// </summary>
    [SerializeField] protected int m_cardID;
    public int CardId{ get{return m_cardID;} }
    
    /// <summary>
    /// 押されているフラグ
    /// </summary>
    protected bool m_pressed = false;
    
    /// <summary>
    /// マウスがカード上
    /// </summary>
    public bool m_hovered = false;
    
    /// <summary>
    /// マウス位置
    /// </summary>
    protected Vector2 m_mousePos;

    /// <summary>
    /// 初期位置
    /// </summary>
    protected Vector2 m_initPos;
    
    /// <summary>
    /// カードのImage
    /// </summary>
    protected UnityEngine.UI.Image m_image;

    /// <summary>
    /// imageの初期サイズ
    /// </summary>
    Vector2 m_imageInitSize;

    /// <summary>
    /// imageのホバーしているときのサイズ
    /// </summary>
    Vector2 m_imageHoverSize;
    
    
    /// <summary>
    /// デバッグ用演出
    /// </summary>
    [SerializeField] protected GameObject m_damageText;

    [SerializeField]protected InstantiateManager m_instantiateManager;
    
    /// <summary>
    /// 手札
    /// </summary>
    protected GameObject m_hands;
    
    /// <summary>
    /// 手札の中の何枚目のカードか
    /// </summary>
    protected int m_handsCardNum;
    
    // Start is called before the first frame update

    /// <summary>
    /// カード情報まとめ
    /// </summary>
    protected GameObject m_cardInfoUI;

    /// <summary>
    /// レイキャストに反応するレイヤー
    /// </summary>
    protected int m_layerMask;
    
    /// <summary>
    /// カードがフィールド上にあるか
    /// </summary> <summary>
    /// 
    /// </summary>
    protected bool m_spawnFied;
    
    /// <summary>
    /// カードがゴミ箱状にあるか
    /// </summary>
    protected bool m_trashFlag = false;

    protected Canvas m_canvas;
    
    /// <summary>
    /// ゴミ箱
    /// </summary>
    protected GameObject m_TrashBox;


    //CPU上昇テキスト
    protected RectTransform m_cpuUpText;
    
    //初期位置
    protected Vector3 m_InitCpuUpPos;
    protected virtual void Start()
    {
        m_initPos = GetComponent<RectTransform>().anchoredPosition;
        m_image = GetComponent<UnityEngine.UI.Image>();


        m_imageInitSize = GetComponent<RectTransform>().sizeDelta;
        m_imageHoverSize = GetComponent<RectTransform>().sizeDelta * 2;
        m_instantiateManager = GameObject.Find("Managers").GetComponent<InstantiateManager>();
        
        //手札
        m_hands = GameObject.Find ("Hands");
        //カード情報UI
        m_cardInfoUI = GameObject.Find("CardInfo");
        //透明部分をレイキャストに当たらない（スプライトから「Read\Write」をチェックする）
        m_image.alphaHitTestMinimumThreshold = 0.5f;
        //m_layerMask = LayerMask.GetMask("Floor");
        m_layerMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("PlayerMonster"));
        //キャンバス取得
        m_canvas = FindObjectOfType<Canvas>();
        //ゴミ箱取得
        m_TrashBox = GameObject.Find("TrashBox");

        
        if(transform.childCount > 0)
        {
            m_cpuUpText = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            m_InitCpuUpPos = m_cpuUpText.anchoredPosition;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //一枚でもホバーしているか
        bool oneceHorvered = false;
        if(m_hands != null)
        {
            foreach (Card obj in m_hands.GetComponent<Hands>().GetHandsCard())
            {
                if(obj.m_handsCardNum == m_handsCardNum)continue;
                if(obj.m_hovered == true)
                {
                    oneceHorvered = true;
                }
            }
        }

        //マウスがどのUIの上にいるか確認
        DetectUIUnderMouse();
        if(m_TrashBox){
            if(m_trashFlag)
            {
                //ゴミ箱拡大
                m_TrashBox.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
            }
            else
            {
                m_TrashBox.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
            }
        }
        //マウスがカードの上にあるか判断
        //horverd = CheckMouseOnCard();
        //Debug.Log(gameObject.name + " : " + horverd);

        if(m_hovered == true && oneceHorvered == false)
        {
           //画像の大きさ変更
           GetComponent<RectTransform>().sizeDelta = m_imageHoverSize;
           if(m_cpuUpText != null)
           {
                m_cpuUpText.anchoredPosition = m_InitCpuUpPos * 2;
                m_cpuUpText.localScale = new Vector3(2,2,2);
           }

            if(Input.GetMouseButton(0))
            {
                press();
            }
            else if(m_pressed)
            {
                release();
            }

            //カードテキスト表示
            //cardInfoUI.GetComponent<CardInfo>().SetVisibleCardInfo(true,cardName,cardText);
            SetCardInfoText();
        }
        else
        {
            //画像の大きさ変更
            GetComponent<RectTransform>().sizeDelta = m_imageInitSize;
            if(m_cpuUpText != null)
           {
                m_cpuUpText.anchoredPosition = m_InitCpuUpPos;
                m_cpuUpText.localScale = new Vector3(1,1,1);
           }

            //一枚でもホバーしているか
            oneceHorvered = false;
            if(m_hands != null)
            {
                foreach (Card obj in m_hands.GetComponent<Hands>().GetHandsCard())
                {
                    if(obj.m_handsCardNum == m_handsCardNum)continue;
                    if(obj.m_hovered)
                    {
                        oneceHorvered = true;
                    }
                }
            }

            if(oneceHorvered == false)
            {
                //Debug.Log(GameObject.Find("CardInfo"));
                //カードテキスト非表示
                m_cardInfoUI.GetComponent<CardInfo>().SetVisibleCardInfo(false,"","");
            }
           
        }
        
        

        if(m_pressed)
        {
            //CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
            float scale = (float)Screen.width / 1920;
            //Debug.Log(scale);
            GetComponent<RectTransform>().anchoredPosition = Input.mousePosition / scale;
        }

    }

     //効果発動
    protected virtual  void CardEffect(RaycastHit hit)
    {
        ;
    }

    /// <summary>
    /// 押されたら
    /// </summary>
    private void press()
    {
        m_pressed = true;
    }

    /// <summary>
    /// 離したら
    /// </summary>
    private void release()
    {
        m_pressed = false;

        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);

        List<RaycastResult> rayResult = new List<RaycastResult>();

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;
        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData ,rayResult);

        //フラグ関連
        bool cardEffectFlag = false;
        m_trashFlag = false;
        bool deckFlag = false;
        GameObject hitObject = null;

        foreach(RaycastResult result in rayResult)
        {
            //カード効果
            if(result.gameObject.name == "SpawnField")
            {
                cardEffectFlag = true;

                break;
            }

            //削除
            if(result.gameObject.name == "TrashBox")
            {
                m_trashFlag = true;

                break;
            }

            if(result.gameObject.tag == "Deck")
            {
                deckFlag = true;
                hitObject = result.gameObject;

                break;
            }
        }

        if(cardEffectFlag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, m_layerMask))
            {
                CardEffect(hit);

                //デバッグヒットしたオブジェクトの名前  
                //Debug.Log(hit.collider.gameObject.name);
            }
        }

        //削除
        if(m_trashFlag)
        {
            m_hands.GetComponent<Hands>().RemoveCard(m_handsCardNum);
        }

        //デッキ登録
        if(deckFlag && hitObject != null)
        {
            hitObject.GetComponent<UnityEngine.UI.Image>().sprite = m_sprite;
            hitObject.GetComponent<DeckSlot>().SetSlot(this.gameObject);
        }

        GetComponent<RectTransform>().anchoredPosition = m_initPos;
    }

    /// <summary>
    /// 何枚目か設定
    /// </summary>
    /// <param name="n"></param>
    public void SetHandsCardNum(int n)
    {
        m_handsCardNum = n;
    }

    /// <summary>
    /// 初期位置変更
    /// </summary>
    /// <param name="pos"></param>
    public void SetInitPos(Vector2 pos)
    {
        m_initPos = pos;
    }

    protected virtual void SetCardInfoText(){}
    
    /// <summary>
    /// カードの上にマウスがあるか判断
    /// </summary>
    /// <returns></returns>
    private bool CheckMouseOnCard()
    {
        Vector2 CardPos = GetComponent<RectTransform>().anchoredPosition;
        m_mousePos = Input.mousePosition;
        //Debug.Log(mousePos);
        var CardSize = GetComponent<RectTransform>().sizeDelta;

        if(CardPos.x + CardSize.x > m_mousePos.x && CardPos.x - CardSize.x < m_mousePos.x &&
        CardPos.y + CardSize.y > m_mousePos.y && CardPos.y - CardSize.y < m_mousePos.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// //マウスがどのUIオブジェクトの上にあるか確認
    /// </summary>
    GameObject DetectUIUnderMouse()
    {
        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        List<RaycastResult> rayResult = new List<RaycastResult>();

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;
        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData ,rayResult);

        m_spawnFied = false;
        m_trashFlag = false;

        GameObject hitObject = null;

        foreach(RaycastResult result in rayResult)
        {

            //カード効果
            if(result.gameObject.name == "SpawnField")
            {
                m_spawnFied = true;
                hitObject = result.gameObject;
    
                break;
            }

            //削除
            if(result.gameObject.name == "TrashBox")
            {
                m_trashFlag = true;
                hitObject = result.gameObject;
                break;
            }
        }

        return hitObject;
    }

}
