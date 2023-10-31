using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
    /// <summary>
    /// 最大ステージ数
    /// </summary>
    [Header("最大ステージ数")]
    public int m_maxStage = 3;
    private int m_currentStage;
    /// <summary>
    /// 現在のステージ
    /// </summary>
    public int currentStage
    {
        get { return m_currentStage; }
        set { m_currentStage = value; }
    }

    new private void Awake() {
        base.Awake();
        string nowScene = SceneManager.GetActiveScene().name;
        char stageNum = nowScene[nowScene.Length - 1];
        m_currentStage = stageNum - '0';
    }

    /// <summary>
    /// 次のステージ数を返す
    /// </summary>
    /// <returns></returns>
    public int GetNextStage(){
        return m_currentStage + 1;
    }

    /// <summary>
    /// 最終ステージなのか（カスタムとランダムもtrueになる）
    /// </summary>
    /// <returns></returns>
    public bool IsLastStage(){
        return m_currentStage >= m_maxStage;
    }
}
