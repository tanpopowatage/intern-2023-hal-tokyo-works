using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// モンスターの行動パターン 
/// </summary>
public enum EnemyMonsterType
{
    A,
    B
}

public class EnemyMonster : Monster
{
    /// <summary>
    /// 敵の行動パターン
    /// </summary>
    [Tooltip("敵の行動パターン")]
    public EnemyMonsterType m_enemyMonsterType;

    /// <summary>
    /// 範囲攻撃受ける時生成する爆風オブジェクト
    /// </summary>
    [SerializeField]private GameObject m_explosion;

    /// <summary>
    /// メインカメラ
    /// </summary>
    private Camera m_mainCamera;
    
    /// <summary>
    /// 画面内にいるPlayerMonster
    /// </summary>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    [SerializeField] private List<GameObject> m_objectsInView = new List<GameObject>();

    /// <summary>
    /// CPU
    /// </summary>
    private CpuMain m_cpumain;

    /// <summary>
    /// エネミーマネージャ
    /// </summary>
    public EnemyManager m_enemyManager;

    /// <summary>
    /// 攻撃範囲内に入ったらすぐ攻撃するか
    /// </summary>
    [Header("攻撃範囲内に入ったらすぐ攻撃するか")]
    [SerializeField] private bool m_canFirstAttack = false;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_mainCamera = Camera.main;

        m_status = Status.idle;

        m_cpumain = GameObject.Find("Managers").GetComponent<CpuMain>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(LagManager.Instance.canUpdate == false)return;
        base.Update();

        if(m_preview)return;
        
        switch(m_status)
        {
            
            case Status.idle:
            Idle();
            break;
            
            case Status.move:
            Move();
            break;

            case Status.attack:
            Attack();
            break;
        }
        
    }

    public override void Action()
    {
        m_attackFlag = false;
        if(!gameObject.activeSelf)return;
        if(m_target == null)return;
        if(m_target.activeSelf == false)return;
        
        if(m_target != null && m_parameter.attackDistance >= m_targetDistance)
        {
            /* if(m_parameter.attackDistance < 5.99f || m_target.GetComponent<EnemyBossMonster>() != null){
                m_target.GetComponent<EnemyMonster>().ChangeHP(m_parameter.attack);
            }
            else{
                m_target.GetComponent<EnemyMonster>().ChangeHPInRange(m_parameter.attack);
            } */

            switch(m_attackType)
            {
                //通常
                case AttackType.near:
                NearAttack();
                break;

                //自分を基準とした範囲攻撃
                case AttackType.middle:
                
                MiddleAttack("Player");
                break;

                //ターゲットを基準とした範囲攻撃
                case AttackType.far:
                if(m_chargeEffect != null && m_spawnAttackEffect == null)
                {
                    m_spawnedChargeEffect = Instantiate(m_chargeEffect,transform.position,Quaternion.identity);
                    Invoke("SpawnAttackEffect",0.8f);
                }
                else if(m_chargeEffect == null)
                {
                    FarAttack("Player");
                }
                break;
            }
            
            cpuMain.UsageRegister(m_parameter.attackLoad);
            //Debug.Log("攻撃 : " + paramerter.attackLoad.raiseRate);

            m_target = null;
        }
        else if(m_status != Status.ucm)
        {
            m_status = Status.idle;
        }
    }

    public override void Death()
    {
        if(isDead)return;
        base.Death();
        // if(m_visibleFlag){
        //     OnBecameInvisibleFromCamera();
        // }
        isDead = true;
        cpuMain.UsageRegister(m_parameter.DestroyLoad);
        //Debug.Log("消失 : " + paramerter.DestroyLoad.raiseRate);
        CPULoad constant = new CPULoad{raiseRate = -1 * m_parameter.constantLoad.raiseRate, impactTime = -1};
        cpuMain.UsageRegister(constant);
        m_enemyManager.UnregisterEnemy();
        m_monsterHPGauge.gameObject.SetActive(false);
        m_showHPGaugeCoroutineCount = 0;
        m_instantiateManager.DestroyMonster(this.gameObject);
        //Destroy(this.gameObject);
        //InstantiateManager.Instance.DestroyMonster(this.gameObject);

        //m_coroutine.Yield();
    }

    //AタイプのUpdate
    private void UpdateTypeA()
    {
        if(DetectEnemiesInScreen())
        {
            m_target = GetClosestBossPlayerMonster();

            //進行方向
            Vector3 moveVec = m_target.transform.position - transform.position;
            moveVec.y = 0;

            //進行方向へ回転
            m_model.transform.rotation = Quaternion.LookRotation(-moveVec,Vector3.up);
            m_model.transform.Rotate(new Vector3(0f, m_rotationOffset, 0f));
            moveVec = moveVec.normalized;

            //ターゲットの距離
            //m_targetDistance = Vector3.Distance(m_target.transform.position,transform.position);
            //距離を計算
            Vector3 closestPoint = m_target.GetComponent<BoxCollider>().ClosestPointOnBounds(transform.position);
            m_targetDistance = Vector2.Distance(new Vector2(closestPoint.x,closestPoint.z),new Vector2(transform.position.x,transform.position.z));
            if(m_parameter.attackDistance < m_targetDistance)
            {
                //ターゲット移動
                transform.position += m_parameter.speed * moveVec * Time.deltaTime;
                
            }
            //攻撃中じゃなければ攻撃
            else if(m_attackFlag == false &&m_parameter.attackDistance >= m_targetDistance)
            {                
                m_status = Status.attack;
            }
           

        }
        else
        {
            //前進
            transform.position -= m_parameter.speed * transform.forward * Time.deltaTime;
        }
    }

    //BタイプのUpdate
    private void UpdateTypeB()
    {
         if(DetectEnemiesInScreen())
        {
            m_target = GetClosestObject();

            //Debug.Log(target.gameObject.name);
            //進行方向
           Vector3 moveVec = m_target.transform.position - transform.position;
           moveVec.y = 0;

           //進行方向へ回転
            m_model.transform.rotation = Quaternion.LookRotation(-moveVec,Vector3.up);
            m_model.transform.Rotate(new Vector3(0f, m_rotationOffset, 0f));
           moveVec = moveVec.normalized;

             //ターゲットの距離
            //float targetDistance = Vector3.Distance(m_target.transform.position,transform.position);
            //距離を計算
            Vector3 closestPoint = m_target.GetComponent<BoxCollider>().ClosestPointOnBounds(transform.position);
            m_targetDistance = Vector2.Distance(new Vector2(closestPoint.x,closestPoint.z),new Vector2(transform.position.x,transform.position.z));
            if(m_parameter.attackDistance < m_targetDistance)
            {
                //ターゲット移動
                transform.position += m_parameter.speed * moveVec * Time.deltaTime;
            }
            //攻撃中じゃなければ攻撃
            else if(m_attackFlag == false &&m_parameter.attackDistance >= m_targetDistance)
            {                
                m_status = Status.attack;
            }
           

        }
        else
        {
            //前進
            transform.position -= m_parameter.speed * transform.forward * Time.deltaTime;
        }
    }

    //---------------------------------------------------ここから下は仮後でマネージャーにまとめる
    //画面内に敵がいるかチェック
    //いなければfalse
    private bool DetectEnemiesInScreen()
    {
        bool view = false;
        m_objectsInView.Clear();
        //Debug.Log("visible list = " + visibleList.GetVisibleList().Count);
        foreach(GameObject obj in visibleList.GetVisibleList()){
            if(obj == null)continue;
            if(obj.GetComponent<PlayerMonster>()){
                m_objectsInView.Add(obj);
                view = true;
            }
        }
        return view;
    }

    //一番近いオブジェクトを取得
    private GameObject GetClosestObject()
    {
        //一番近いプレイヤーモンスター
        GameObject closestPlayerMonster = GetClosestPlayerMonster();
        //一番近いボスプレイヤーモンスター
        GameObject closestBossPlayerMonster = GetClosestBossPlayerMonster();

        //通常のプレイヤーモンスターがいなければボスをターゲット
        if(closestPlayerMonster == null)
        {
            //Debug.Log("no player monster");
            return closestBossPlayerMonster;
        }
        //通常のプレイヤーモンスターがいれば一番近い通常の敵をターゲット
        else
        {
            //Debug.Log("player monster");
            return closestPlayerMonster;
        }
    }

    //一番近いPlatyerMonster取得
    private GameObject GetClosestPlayerMonster()
    {
        //Debug.Log(objectsInView.Count);
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity; // 最初は無限大として設定
        foreach (GameObject obj in m_objectsInView)
        {
            if(obj == null) continue;
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            
            if (distance < shortestDistance && obj.gameObject.tag == "Player")
            {
                PlayerMonster pm = obj.GetComponent<PlayerMonster>();
                //モンスターがプレビュー状態だったら
                if(!pm.GetPreview())
                {
                    closestObject = obj;
                    shortestDistance = distance;
                }
            }
        }
        return closestObject;
    }

    //一番近いBossEnemy取得
    private GameObject GetClosestBossPlayerMonster()
    {
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity; // 最初は無限大として設定
        foreach (GameObject obj in m_objectsInView)
        {
            if(obj == null) continue;
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < shortestDistance && obj.GetComponent<PlayerBossMonster>())
            {
                closestObject = obj;
                shortestDistance = distance;
            }
        }
        return closestObject;
    }

    //待機
    private void Idle()
    {
        m_status = Status.move;
    }

    //移動
    private void Move()
    {
        switch(m_enemyMonsterType)
        {
            //Aタイプ
            case EnemyMonsterType.A:
            UpdateTypeA();
            break;

            //Bタイプ
            case EnemyMonsterType.B:
            UpdateTypeB();
            break;
        }
    }

    //攻撃
    private void Attack()
    {
        if(DetectEnemiesInScreen())
        {
            m_target = GetClosestObject();
            
            //進行方向
           Vector3 moveVec = m_target.transform.position - transform.position;
           moveVec = moveVec.normalized;

            //ターゲットの距離
            //m_targetDistance = Vector3.Distance(m_target.transform.position,transform.position);
            //距離を計算
            Vector3 closestPoint = m_target.GetComponent<BoxCollider>().ClosestPointOnBounds(transform.position);
            m_targetDistance = Vector2.Distance(new Vector2(closestPoint.x,closestPoint.z),new Vector2(transform.position.x,transform.position.z));
            if(m_attackFlag == false &&m_parameter.attackDistance >= m_targetDistance)
            {                
                if(m_canFirstAttack){Action();}
                Invoke("Action",m_parameter.attackInterval);

                m_attackFlag = true;
            }
            else
            {
                m_status = Status.move;
            }

           

        }
        else
        {
            m_status = Status.idle;
        }
    }

    /// <summary>
    /// 範囲内の敵もダメージを食らう
    /// </summary>
    /// <param name="val">ダメージ量</param>
    public void ChangeHPInRange(float val){
        /* if(m_rangeAttackZone){
            Instantiate(m_explosion, transform.position, Quaternion.identity);
            foreach(EnemyMonster em in m_rangeAttackZoneEnemy.GetEnemyMonstersInRange()){
                em.ChangeHP(val);
            }
        }
        else{
            this.ChangeHP(val);
        } */
    }
}
