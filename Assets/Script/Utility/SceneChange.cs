using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移を行うするクラス
/// </summary>
public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// フェードのスクリプト
    /// </summary>
    [Header("フェードのスクリプト")]
    [SerializeField] private Fade m_fade;

    /// <summary>
    /// Windows風シーン遷移
    /// </summary>
    [SerializeField] private WindowChangeFade m_windowChangeFade;

    /// <summary>
    /// 終了確認パネル
    /// </summary>
    [Header("終了確認パネル")]
    [SerializeField] private GameObject m_confirmPanel;

    private void Awake(){
        if(m_fade != null){
            m_fade.gameObject.SetActive(true);
        }
        if(m_confirmPanel != null){
            m_confirmPanel.SetActive(false);
        }
    }

    /// <summary>
    /// シーン遷移を設定（ウインドウ閉じるような）
    /// </summary>
    /// <param name="sceneName">次のシーン名</param>
    public void WindowChangeScene(string sceneName){
        //m_fade.SetFadeOut(1f, sceneName);
        m_windowChangeFade.SetFadeOut(0.5f, sceneName);
    }

    /// <summary>
    /// シーン遷移を設定（フェード）
    /// </summary>
    /// <param name="sceneName">次のシーン名</param>
    public void FadeChangeScene(string sceneName){
        m_fade.SetFadeOut(1f, sceneName);
    }

    /// <summary>
    /// ステージ選択
    /// </summary>
    /// <param name="sceneName">ステージ数を含んだシーン名</param>
    public void GotoStage(string sceneName){
        string nextStageName = sceneName;
        StageManager.Instance.currentStage = nextStageName[nextStageName.Length-1] - '0';
        //m_windowChangeFade.SetFadeOut(0.5f, nextStageName);
        m_windowChangeFade.SetWindowOutBlackIn(0.5f, nextStageName);
    }

    /// <summary>
    /// 「次のステージへ」
    /// </summary>
    /// <param name="sceneName">ステージ数をのぞいたシーン名</param>
    public void GotoNextStage(string sceneName){
        int nextStageNum = StageManager.Instance.GetNextStage();
        string nextStageName = sceneName + nextStageNum.ToString();
        StageManager.Instance.currentStage = nextStageNum;
        m_fade.SetFadeOut(1f, nextStageName);
    }

    /// <summary>
    /// 「リトライ」
    /// </summary>
    /// <param name="sceneName">ステージ数をのぞいたシーン名</param>
    public void RetryStage(string sceneName){
        string nextStageName = sceneName + StageManager.Instance.currentStage.ToString();
        m_fade.SetFadeOut(1f, nextStageName);
    }

    /// <summary>
    /// 終了確認パネルを有効・無効化
    /// </summary>
    /// <param name="b"></param>
    public void SetConfirmation(bool b){
        m_confirmPanel.SetActive(b);
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void EndGame(){
        //m_fade.SetFadeOut(1f, "ENDGAME");
        m_windowChangeFade.SetFadeOut(0.5f, "ENDGAME");
    }
}
