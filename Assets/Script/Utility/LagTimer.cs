using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WaitForSecondsRealTimeのキャッシュを保存するクラス（メモリ消費を減らすため）
/// </summary>
public static class LagTimer
{
    /// <summary>
    /// 保存しているWaitForSecondsRealTimeのリスト
    /// </summary>
    private static Dictionary<float, WaitForSecondsRealtime> m_timeInterval = new Dictionary<float, WaitForSecondsRealtime>(100);
 
    private static WaitForEndOfFrame m_endOfFrame = new WaitForEndOfFrame();
    /// <summary>
    /// 1フレーム分の時間を待つ
    /// </summary>
    public static WaitForEndOfFrame OneFrame {
        get{ return m_endOfFrame;}
    }
 
    private static WaitForFixedUpdate m_fixedUpdate = new WaitForFixedUpdate();
    /// <summary>
    /// FixedUpdate分の時間を待つ
    /// </summary>
    public static WaitForFixedUpdate FixedUpdate{
        get{ return m_fixedUpdate; }
    }
 
    /// <summary>
    /// 待つ秒数を取得
    /// </summary>
    /// <param name="seconds">秒数</param>
    /// <returns></returns>
    public static WaitForSecondsRealtime Get(float seconds){
        if(!m_timeInterval.ContainsKey(seconds))
            m_timeInterval.Add(seconds, new WaitForSecondsRealtime(seconds));
        return m_timeInterval[seconds];
    }
}
