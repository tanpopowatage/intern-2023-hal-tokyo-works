using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using UnityEngine.XR;

/// <summary>
/// ゲームマネージャ
/// </summary>
public class GameManager : MonoBehaviour
{
    #region public/serialized
    /// <summary>
    /// CPU使用量とラグ間隔の関係カーブ
    /// </summary>
    [Header("CPU-ラグ間隔の関係カーブ")]
    public AnimationCurve m_cpuUsage_LagIntervalCurve;

    /// <summary>
    /// CPUメイン
    /// </summary>
    [Header("CPUメイン")]
    [SerializeField] CpuMain m_cpuMain;

    /// <summary>
    /// SceneChangeを持っているオブジェ（現在はCanvas）
    /// </summary>
    [Header("Canvas")]
    [SerializeField] SceneChange m_canvas;

    /// <summary>
    /// ゲームオーバーのシーン名
    /// </summary>
    [Header("ゲームオーバーのシーン名")]
    [SerializeField] string m_gameoverSceneName;

    /// <summary>
    /// ゲームクリアのシーン名
    /// </summary>
    [Header("ゲームクリアのシーン名")]
    [SerializeField] string m_clearSceneName;

    [Header("エネミーマネージャ")]
    [SerializeField] EnemyManager m_enemyManager;

    [Header("UIタイマー")]
    [SerializeField] UITimer m_uiTimer;

    [Header("CPUベース")]
    [SerializeField] Transform m_cpuBase;

    [Header("ゲームオーバー時ベースの爆発エフェクト")]
    [SerializeField] GameObject m_gameoverExplosion;

    [Header("ゲームオーバー時ステージの爆発エフェクト")]
    [SerializeField] GameObject m_gameoverExplosionOnStage;

    [Header("ステージ")]
    [SerializeField] Transform m_stage;
    #endregion
    
    #region private
    /// <summary>
    /// クリアしたかどうか
    /// </summary>
    private bool m_clearFlag = false;

    /// <summary>
    /// ラグ間隔
    /// </summary>
    private float m_lagInterval = 0.0f;

    /// <summary>
    /// ラグを模擬するコルーチン
    /// </summary>
    Coroutine m_lagCoroutine = null;

    Vector3 m_stageOrigin;
    #endregion


    private void Start()
    {
        Time.timeScale = 1f;
        if(m_cpuMain){
            m_cpuMain.OnUsageFull += GameOver;
        }
        if(m_enemyManager){
            m_enemyManager.OnAllEnemyCleared += GameClear;
        }
        if(m_uiTimer){
            m_uiTimer.OnTimerZero += GameClear;
        }
        m_lagCoroutine = StartCoroutine(LagSimulate());
        if(m_stage){
            m_stageOrigin = m_stage.position;
        }
    }

    private void Update()
    {
        if(m_clearFlag){
            StageShake(0.3f);
            return;
        }
        LagManager.Instance.lagInterval = EvaluateLagInterval(m_cpuMain.Usage);
        
    }

    /// <summary>
    /// カーブからラグ間隔を計算
    /// </summary>
    /// <param name="cpu">CPU使用量</param>
    float EvaluateLagInterval(float cpu){
        //m_lagInterval = m_cpuUsage_LagIntervalCurve.Evaluate(cpu / (float)100);
        return m_cpuUsage_LagIntervalCurve.Evaluate(cpu / (float)100);
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver(){
        StopCoroutine(m_lagCoroutine);
        //シーン遷移を一回だけ呼ぶ
        if(!m_clearFlag){
            StartCoroutine(PerformGameOverExplosion());
        }
        m_clearFlag = true;
    }

    /// <summary>
    /// ゲームクリア処理
    /// </summary>
    public void GameClear(){
        StopCoroutine(m_lagCoroutine);
        //シーン遷移を一回だけ呼ぶ
        if(!m_clearFlag){
            m_canvas.FadeChangeScene(m_clearSceneName);
        }
        m_clearFlag = true;
    }

    /// <summary>
    /// ラグのシミュレーション
    /// </summary>
    /// <returns></returns>
    IEnumerator LagSimulate(){
        while(true){
            //0なら処理しない
            if(m_lagInterval <= 0.0f){
                yield return null;
                continue;
            }
            //止まったり動いたりする
            Time.timeScale = 0f;
            //WaitForSecondsをキャッシュして使う
            yield return LagTimer.Get(m_lagInterval); //+ランダムノイズでも？
            Time.timeScale = 1f;
            yield return LagTimer.Get(m_lagInterval);
        }
    }

    IEnumerator PerformGameOverExplosion(){
        StartCoroutine(GameoverExplosionsOnStage());
        Time.timeScale = 0.5f;
        Vector3 pos = m_cpuBase.position;
        pos.y += 2f;
        Instantiate(m_gameoverExplosion, pos, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        m_canvas.FadeChangeScene(m_gameoverSceneName);
    }

    IEnumerator GameoverExplosionsOnStage(){
        for(int i = 0; i < 10; i++){
            float scale = Random.Range(0.75f, 2f);
            Transform explosion = Instantiate(m_gameoverExplosionOnStage, RandomPositionOnStage(), Quaternion.identity).transform.GetChild(0);
            foreach(Transform effect in explosion){
                effect.localScale *= scale;
            }
            yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.7f));
        }
    }

    private Vector3 RandomPositionOnStage(){
        Vector3 pos;
        pos.x = Random.Range(-11f, 14f);
        pos.y = 0f;
        pos.z = Random.Range(-3f, 12f);
        return pos;
    }

    private void StageShake(float range){
        Vector3 newPos;
        newPos.x = m_stageOrigin.x + Random.Range(-1*range, range);
        newPos.y = m_stageOrigin.y + Random.Range(-1*range, range);
        newPos.z = m_stageOrigin.z + Random.Range(-1*range, range);
        m_stage.position = newPos;
    }
}
