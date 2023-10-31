using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagManager : SingletonMonoBehaviour<LagManager>
{
    private float m_lagInterval;
    public float lagInterval
    {
        get { return m_lagInterval; }
        set { m_lagInterval = value; }
    }

    private bool m_canUpdate;
    /// <summary>
    /// ラグがないかどうか<para>T: 止まっていない　F: 止まっている</para>
    /// </summary>
    public bool canUpdate
    {
        get { return m_canUpdate; }
        set { m_canUpdate = value; }
    }
    
    
    Coroutine m_lagCoroutine = null;

    new private void Awake() {
        base.Awake();
        m_lagInterval = 0f;
        m_lagCoroutine = StartCoroutine(LagSimulate());
    }

    private void OnDestroy() {
        if(m_lagCoroutine != null){
            StopCoroutine(m_lagCoroutine);
        }
    }

    IEnumerator LagSimulate(){
        while(true){
            //0なら処理しない
            if(m_lagInterval <= 0.0f){
                m_canUpdate = true;
                yield return null;
                continue;
            }
            //止まったり動いたりする
            m_canUpdate = false;
            //WaitForSecondsをキャッシュして使う
            yield return LagTimer.Get(m_lagInterval); //+ランダムノイズでも？
            m_canUpdate = true;
            yield return LagTimer.Get(m_lagInterval);
        }
    }

}
