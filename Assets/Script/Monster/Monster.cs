using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
public enum Status
{
    idle,
    move,
    attack,
    stop,
    death,
    //ここから下はユニバーサルクロス用
    ucm,
    uca
}

public enum AttackType
{
    //通常の攻撃
    near,
    //自分を基準とした範囲攻撃
    middle,
    //ターゲットを基準とした範囲攻撃
    far
}

//CPU負荷単位
[System.Serializable]
public struct  CPULoad
{
    //上昇率
    [Tooltip("上昇率")]
    [SerializeField] public float raiseRate;
    //影響時間
    [Tooltip("影響時間")]
    [SerializeField] public float impactTime;
}

[System.Serializable]
public struct  MonsterParamerter
{
    //体力
    [Tooltip("体力")]
    [SerializeField] public float hp;
    //体力上限
    [Tooltip("体力最大値（ここはいじらない）")]
    public float maxHp;
    //速度
    [Tooltip("移動速度")]
    [SerializeField] public float speed;
    //ID
    [Tooltip("識別番号")]
    [SerializeField] public int monsterID;
    //攻撃力
    [Tooltip("攻撃力")]
    [SerializeField] public float attack;
    //攻撃距離
    [Tooltip("攻撃範囲")]
    [SerializeField] public float attackDistance;
    //攻撃間隔
    [Tooltip("クールタイム")]
    [SerializeField] public float attackInterval;

    //CPU系
    //常時
    [Tooltip("常時CPU影響")]
    [SerializeField] public CPULoad constantLoad;
    //出現
    [Tooltip("出現時CPU影響")]
    [SerializeField] public CPULoad spawnLoad;
    //攻撃
    [Tooltip("攻撃時CPU影響")]
    [SerializeField] public CPULoad attackLoad;
    //消失
    [Tooltip("消失時CPU影響")]
    [SerializeField] public CPULoad DestroyLoad;


}

public class Monster : MonoBehaviour
{
    //モンスターのパラメーター
    [Tooltip("モンスターのパラメーター")]
    [SerializeField] public MonsterParamerter m_parameter;
    //モンスターのステータス
    [Tooltip("モンスターのステータス")]
    [SerializeField] public Status m_status;
    //攻撃しているかのフラグ
    [Tooltip("攻撃しているか")]
    protected bool m_attackFlag;
    //攻撃するターゲット
    [Tooltip("攻撃ターゲット")]
    public GameObject m_target;
    //ターゲットの距離
    protected float m_targetDistance;
     //デバッグ用ダメージ演出オブジェクト
     [Tooltip("デバッグ用ダメージ演出オブジェクト")]
    [SerializeField] protected GameObject m_damageText;

    [SerializeField] protected MonsterHPGauge m_monsterHPGauge;

    protected int m_showHPGaugeCoroutineCount = 0;

    [SerializeField] protected GameObject m_model;
    [Header("モデルの前方向への回転補正")]
    [SerializeField] protected float m_rotationOffset = 0f;
    
    //初期マテリアル
    public Material m_initMaterial;
    //デバッグ用ダメージマテリアル
    [SerializeField] Material m_debugMaterial;

    //ViewList(画面に映っているモンスター)のインデックス
    protected int m_visibleListIndex; //映っていない場合は-1
    protected bool m_visibleFlag = false;
    protected VisibleList m_visibleList;
    public VisibleList visibleList
    {
        get {return m_visibleList;}
        set { m_visibleList = value; }
    }

    protected CpuMain m_cpuMain;
    public CpuMain cpuMain
    {
        get {return m_cpuMain;}
        set { m_cpuMain = value; }
    }
    
    protected InstantiateManager m_instantiateManager;
    public InstantiateManager instantiateManager
    {
        get {return m_instantiateManager;}
        set { m_instantiateManager = value; }
    }
    
    protected bool m_isDead;
    public bool isDead{
        get{return m_isDead;}
        set{m_isDead = value;}
    }

    protected bool m_preview = false;

    protected Coroutine m_coroutine = null;

    /// <summary>
    /// 攻撃種類
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField]protected AttackType m_attackType;

    /// <summary>
    /// 範囲攻撃管理
    /// </summary>
    [SerializeField]protected RangeAttackZone m_rangeAttackZone;

    private bool m_prevRangeAttackFlag;

        /// <summary>
    /// チャージエフェクト
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField] protected GameObject m_chargeEffect;

    /// <summary>
    /// 生成したチャージエフェクト
    /// </summary>
    protected GameObject m_spawnedChargeEffect;

        /// <summary>
    /// 攻撃エフェクト
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField] protected GameObject m_attackEffect;
    
    /// <summary>
    /// 生成した攻撃エフェクト
    /// </summary>
    [SerializeField] protected GameObject m_spawnAttackEffect;

    protected AudioSource m_audioSource;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        m_initMaterial = GetComponent<Renderer>().material;
        if(m_visibleList == null){
            m_visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
        }
        if(cpuMain == null){
            cpuMain = GameObject.Find("Managers").GetComponent<CpuMain>();
        }

        m_parameter.maxHp = m_parameter.hp;
        //Debug.Log("maxHP = " + m_parameter.maxHp);
        m_audioSource = GetComponent<AudioSource>();
        //cpuMain.UsageRegister(m_parameter.constantLoad);
        OnBecameVisibleFromCamera();
    }

    protected void OnEnable() {
        if(m_visibleList == null){
            m_visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
        }
        if(cpuMain == null){
            cpuMain = GameObject.Find("Managers").GetComponent<CpuMain>();
        }

        //m_parameter.maxHp = m_parameter.hp;
        //Debug.Log("maxHP = " + m_parameter.maxHp);
        if(m_monsterHPGauge == null)return;
        m_monsterHPGauge.gameObject.SetActive(false);
        m_showHPGaugeCoroutineCount = 0;
        if(m_visibleListIndex < 0){
            OnBecameVisibleFromCamera();
        }
        //cpuMain.UsageRegister(m_parameter.constantLoad);
        // OnBecameVisibleFromCamera();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //CheckVisible();
    }

    public virtual void Action()
    {
        
    }

    public virtual void ChangeHP(float val)
    {
        m_parameter.hp -= val;
        ShowHPGauge();

        //デバッグ用ダメージ演出
        GameObject spawnText = Instantiate(m_damageText,transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = val.ToString();

        if(m_parameter.hp <= 0)
        {
            m_parameter.hp = 0;
            Death();
        }

        //デバッグ用
        GetComponent<Renderer>().material = m_debugMaterial;
        Invoke("ResetMaterial",0.25f);
        
    }

    //マテリアルを戻す
    void ResetMaterial()
    {
        GetComponent<Renderer>().material = m_initMaterial;
    }

    public virtual void Death()
    {

        OnBecameInvisibleFromCamera();
        //Destroy(this.gameObject);
    }

    /// <summary>
    /// カメラが映らなくなる時の処理
    /// </summary>
    protected virtual void OnBecameVisibleFromCamera() {
        m_visibleListIndex = m_visibleList.AddVisibleObject(this.gameObject);
        //Debug.Log("my index = " + visibleListIndex);
    }

    /// <summary>
    /// カメラが映るようになる時の処理
    /// </summary>
    protected virtual void OnBecameInvisibleFromCamera() {
        //不具合ですでに-1になっている時は処理しない
        if(m_visibleListIndex < 0)return;

        m_visibleList.RemoveVisibleObject(m_visibleListIndex);
        m_visibleFlag = false;
        m_visibleListIndex = -1;
    }

    /// <summary>
    /// カメラが映っているかチェック
    /// </summary>
    /// <returns></returns>
    protected bool IsVisibleFromCamera(){
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds);
    }

    protected virtual void CheckVisible(){
        if(m_visibleFlag == IsVisibleFromCamera())return;

        if(m_visibleFlag){
            //OnBecameInvisibleの処理
            OnBecameInvisibleFromCamera();
        }
        else{
            //OnBecameVisibleの処理
            OnBecameVisibleFromCamera();
        }
        //visibleFlagの状態を保存
        m_visibleFlag = !m_visibleFlag;
    }

    protected void OnDestroy() {
        //VisibleListから自分を削除
        if(m_visibleFlag){
            OnBecameInvisibleFromCamera();
        }
    }
        
    /// <summary>
    /// モンスターの能力上昇
    /// </summary>
    /// <param name="var"></param>
    public void UpHP(float var)
    {
        m_parameter.hp += var;

        if(m_parameter.hp > m_parameter.maxHp)
        {
            m_parameter.hp = m_parameter.maxHp;
        }
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(0,255,0,1);
    }
    
    public void UpSpeed(float var)
    {
        m_parameter.speed += var;
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(0,0,255,1);
    }
    
    public void UpAttack(float var)
    {
        m_parameter.attack += var;
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "+"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(255,0,0,1);
    }
    
    public void UpCoolTime(float var)
    {
        m_parameter.attackInterval -= var;

        if(m_parameter.attackInterval < 1)
        {
            m_parameter.attackInterval = 1;
        }
        //デバッグ用演出
        GameObject spawnText = Instantiate(m_damageText,gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = "-"+ var.ToString();
        spawnText.GetComponent<TextMeshPro>().color = new Color(255,255,0,1);
    }

    public void SetStatus(Status st)
    {
        m_status = st;
    }


    public void SetTarget(GameObject obj)
    {
        m_target = obj;
    }

    /// <summary>
    /// Monsterをプレビュー状態にする
    /// </summary> <summary>
    /// 
    /// </summary>
    public void SetPreview(bool b)
    {
        m_preview = b;
    }

    public bool GetPreview(){return m_preview;}

    //パラメーター取得
    public MonsterParamerter GetParamerter()
    {
        return m_parameter;
    }

    //パラメーター設定
    public void SetParamerter(MonsterParamerter par)
    {
        m_parameter = par;
    }

    protected virtual  void ShowHPGauge(){
        if(gameObject.activeSelf)
        {
            StartCoroutine(ShowHPGaugeCoroutine(2f));
        }
    }

    IEnumerator ShowHPGaugeCoroutine(float time){
        //Debug.Log("before adding count = " + m_showHPGaugeCoroutineCount);
        if(m_showHPGaugeCoroutineCount <= 0){
            m_monsterHPGauge.gameObject.SetActive(true);
            m_showHPGaugeCoroutineCount = 0;
        }
        m_showHPGaugeCoroutineCount++;
        m_monsterHPGauge.SetGaugeFill();
        //yield return LagTimer.Get(time);
        yield return new WaitForSecondsRealtime(time);
        m_showHPGaugeCoroutineCount--;
        if(m_showHPGaugeCoroutineCount <= 0){
            m_monsterHPGauge.gameObject.SetActive(false);
        }
        //Debug.Log("before subbing count = " + m_showHPGaugeCoroutineCount);
        
        if(m_showHPGaugeCoroutineCount < 0) m_showHPGaugeCoroutineCount = 0;
    }

    //通常攻撃
    protected void NearAttack()
    {
        if(m_audioSource){
            m_audioSource.Play();
        }
        m_target.GetComponent<Monster>().ChangeHP(m_parameter.attack);

        if(m_attackEffect)
        {
            Instantiate(m_attackEffect,m_target.transform.position,Quaternion.identity);
        }
    }

    /// <summary>
    /// 自分を基準とした範囲攻撃
    /// </summary>
    /// <param name="tag"></param> <summary>
    /// Playerか敵を判断するタグ
    /// </summary>
    /// <param name="tag"></param>
    protected void MiddleAttack(string tag)
    {
        if(m_audioSource){
            m_audioSource.Play();
        }
        Collider collider = m_rangeAttackZone.GetComponent<Collider>();
        m_rangeAttackZone.transform.position = this.gameObject.transform.position;
        
        if(m_prevRangeAttackFlag == false && transform.gameObject.activeSelf)
        {
            collider.enabled = true;
            m_prevRangeAttackFlag = true;

            if(m_attackEffect != null)
            {
                Instantiate(m_attackEffect,transform.position,transform.rotation);
            }

            m_coroutine = StartCoroutine(ResetColliderEnable());   
        }
    }

    /// <summary>
    /// ターゲットを基準とした範囲攻撃
    /// </summary>
    /// <param name="tag"></param> <summary>
    /// Playerか敵を判断するタグ
    /// </summary>
    /// <param name="tag"></param>
    protected void FarAttack(string tag)
    {
        Collider collider = m_rangeAttackZone.GetComponent<Collider>();
        m_rangeAttackZone.transform.position = m_target.transform.position;
        if(m_prevRangeAttackFlag == false)
        {
            collider.enabled = true;
            m_prevRangeAttackFlag = true;
            StartCoroutine(ResetColliderEnable());
        }
    }

    private IEnumerator ResetColliderEnable()
    {
        yield return new WaitForSeconds(0.2f);
        Collider collider = m_rangeAttackZone.GetComponent<Collider>();

        collider.enabled = false;
        m_prevRangeAttackFlag = false;

        yield break;
    }

    /// <summary>
    /// 攻撃エフェクト生成
    /// </summary> <summary>
    /// 
    /// </summary>
    protected void SpawnAttackEffect()
    {
        if(m_spawnAttackEffect != null)
        {
            return;
        }
        if(!gameObject.activeSelf)return;
        
        m_spawnAttackEffect = Instantiate(m_attackEffect,transform.position,Quaternion.identity);

        if(m_target != null)
        {
            m_spawnAttackEffect.transform.LookAt(m_target.transform);
        }
        
        if(gameObject.tag == "Player")
        {
            StartCoroutine( AttackCoroutine(1.0f, "Enemy"));
        }
        else
        {
            StartCoroutine( AttackCoroutine(1.0f, "Player"));
        }
    }

    private IEnumerator AttackCoroutine(float time,string tag)
    {
        switch(m_attackType)
        {
            case AttackType.near:
            new WaitForSeconds(time);
            NearAttack();
            yield break;

            case AttackType.middle:
            new WaitForSeconds(time);
            MiddleAttack(tag);
            yield break;
            case AttackType.far:
            if(m_audioSource){
                m_audioSource.Play();
            }
            FarAttack(tag);
            yield return new WaitForSeconds(time);
            //FarAttack(tag);
            yield break;
        }
    }
}
