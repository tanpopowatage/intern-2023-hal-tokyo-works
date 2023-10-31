using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Windows風シーン遷移
/// </summary>
public class WindowChangeFade : MonoBehaviour
{
    /// <summary>
    /// フェードのモード（イン・アウト）
    /// </summary>
    public enum FADE_MODE{
        WINDOW_IN,
        WINDOW_OUT,
        WINDOW_OUT_BLACK_IN,
        BLACK_OUT_WINDOW_IN,
        FADE_NONE,
    }

    [Header("普通のフェードインから入るならつけなくてOK")]
    [SerializeField] private Image m_blackOverlay;

    /// <summary>
    /// 最初からフェードするか
    /// </summary>
    public bool m_fadeAtStart = true;

    #region private
    /// <summary>
    /// フェード前のα値
    /// </summary>
    private float m_prevAlpha;

    /// <summary>
    /// フェード後のα値
    /// </summary>
    private float m_targetAlpha;

    /// <summary>
    /// フェードの時間
    /// </summary>
    private float m_fadeTime;

    /// <summary>
    /// フェード中現在の時間
    /// </summary>
    private float m_nowFadeTime = 0;

    /// <summary>
    /// スクリーン
    /// </summary>
    private RectTransform m_screen;

    /// <summary>
    /// フェード中かどうか
    /// </summary>
    private bool m_isFading;

    /// <summary>
    /// 次のシーン名<para>終了ならばシーン名を"ENDGAME"に</para>
    /// </summary>
    private string m_nextSceneName;

    /// <summary>
    /// フェードモード
    /// </summary>
    private FADE_MODE m_fadeMode;

    /// <summary>
    /// フェードインする画面は黒画面なのか
    /// </summary>
    private bool m_blackIn = false;
    #endregion


    private void Start()
    {
        // Color color = Color.black;
        // color.a = 0f;
        // m_blackOverlay.color = color;
        if(m_blackOverlay){
            m_blackOverlay.gameObject.SetActive(false);
        }
        m_screen = GetComponent<RectTransform>();
        //m_image = GetComponent<Image>();
        if(m_fadeAtStart){
            SetFadeIn(0.5f);
        }
    }

    private void FixedUpdate()
    {
        //シーンの最初数フレームのunscaledDeltaTimeが異常に大きいのでフェードの処理をしない
        //閾値はわりと適当（普通な場合なら超えないだろうな値）
        if(Time.unscaledDeltaTime > 0.3f)return;

        //フェード中であれば
        if(m_isFading){
            //フェード（CrossFadeAlphaが上手くできないので直接に画像のα値を変更）
            float nowSize = Mathf.Lerp(m_prevAlpha, m_targetAlpha, m_nowFadeTime);
            m_screen.localScale = new Vector3(nowSize, nowSize, nowSize);
            foreach(Image image in GetComponentsInChildren<Image>()){
                //Debug.Log(image.gameObject.name);
                if(m_blackIn)break;
                Color color = image.color;
                color.a = Mathf.Lerp(m_prevAlpha, m_targetAlpha, m_nowFadeTime);
                image.color = color;
            }
            m_nowFadeTime += Time.unscaledDeltaTime / m_fadeTime;
            //m_image.color = color;

            //フェードアウト
            if(m_fadeMode == FADE_MODE.WINDOW_OUT){
                //シーン遷移
                if(m_screen.localScale.x <= 0f){
                    if(m_blackIn){
                        //m_blackOverlay.color = Color.black;
                        m_blackOverlay.gameObject.SetActive(true);
                        SetFadeIn(m_fadeTime);
                        return;
                    }
                    if(m_nextSceneName == "ENDGAME"){
                        EndGame();
                        return;
                    }
                    else{
                        PoolManager.Instance.ReleaseAllGameObjects();
                        SceneManager.LoadScene(m_nextSceneName);
                    }
                }
            }
            //フェードイン
            else if(m_fadeMode == FADE_MODE.WINDOW_IN){
                if(m_screen.localScale.x >= 1f){
                    if(m_blackIn){
                        PoolManager.Instance.ReleaseAllGameObjects();
                        SceneManager.LoadScene(m_nextSceneName);
                    }
                    m_screen.localScale = new Vector3(1f, 1f, 1f);
                    m_fadeMode = FADE_MODE.FADE_NONE;
                    m_isFading = false;
                }
            }
        }
    }

    /// <summary>
    /// フェードインを設定
    /// </summary>
    /// <param name="duration">フェードのかかる秒数</param>
    public void SetFadeIn(float duration){
        m_fadeMode = FADE_MODE.WINDOW_IN;
        m_screen.localScale = Vector3.zero;
        m_prevAlpha = 0f;
        StartFade(1f, duration);
    }

    /// <summary>
    /// フェードアウトを設定
    /// </summary>
    /// <param name="duration">フェードのかかる秒数</param>
    /// <param name="sceneName">次のシーン名</param>
    public void SetFadeOut(float duration, string sceneName){
        m_fadeMode = FADE_MODE.WINDOW_OUT;
        m_screen.localScale = new Vector3(1f, 1f, 1f);
        m_nextSceneName = sceneName;
        m_prevAlpha = 1f;
        StartFade(0f, duration);
    }

    /// <summary>
    /// フェードアウトしてから黒画面インする
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="sceneName"></param>
    public void SetWindowOutBlackIn(float duration, string sceneName){
        m_blackIn = true;
        SetFadeOut(duration, sceneName);
    }

    /// <summary>
    /// フェードの共通処理
    /// </summary>
    /// <param name="targetAlpha">フェード後のα値</param>
    /// <param name="fadeTime">フェードのかかる秒数</param>
    private void StartFade(float targetAlpha, float fadeTime){
        m_targetAlpha = targetAlpha;
        m_fadeTime = fadeTime;
        m_nowFadeTime = 0f;
        m_isFading = true;
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    private void EndGame(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
