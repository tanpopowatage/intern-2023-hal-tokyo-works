using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBurnEffect : MonoBehaviour
{
    /// <summary>
    /// 炎エフェクト（FireEffects）
    /// </summary>
    [SerializeField] private Transform m_fireEffects;

    [SerializeField] private CpuMain m_cpuMain;

    private int m_nowCpuStep;

    void Start()
    {
        m_cpuMain.OnUsageChanged += SetFireStep;

        m_nowCpuStep = 0;

        foreach(Transform fire in m_fireEffects){
            fire.gameObject.SetActive(false);
        }
    }

    public void SetFireStep(float usage){
        int step = Mathf.FloorToInt(usage/25);
        if(step > 4)step = 4;
        //100%超えたら中央にデカい炎が発生
        if(step == 4){
            foreach(Transform fire in m_fireEffects){
                if(fire.gameObject.activeSelf == false){
                    fire.gameObject.SetActive(true);
                }
            }
            m_nowCpuStep = step;
            return;
        }
        else{
            m_fireEffects.GetChild(m_fireEffects.childCount-1).gameObject.SetActive(false);
        }

        //必須：0になったらこれやらないと下の処理で無限ループになる
        if(step <= 0){
            foreach(Transform fire in m_fireEffects){
                fire.gameObject.SetActive(false);
            }
            m_nowCpuStep = step;
            return;
        }

        //Debug.Log("step = " + step);
        //着火する場所を一個増やす
        if(step > m_nowCpuStep){
            int addFire = Mathf.Abs(step - m_nowCpuStep);
            //Debug.Log("addFire = " + addFire);
            //return;
            int safeCnt = 20;
            while(addFire > 0 && safeCnt > 0){
                GameObject nowFire = m_fireEffects.GetChild(Random.Range(0,m_fireEffects.childCount-2)).gameObject;
                if(nowFire.activeSelf == false){
                    nowFire.SetActive(true);
                    addFire--;
                }
                safeCnt--;
            }
            m_nowCpuStep = step;
            return;
        }
        //着火する場所を一個減る
        if(step < m_nowCpuStep){
            int minusFire = Mathf.Abs(m_nowCpuStep - step);
            //Debug.Log("minusFire = " + minusFire);
            //return;
            int safeCnt = 20;
            while(minusFire > 0 && safeCnt > 0){
                Debug.Log("minusFire = " + minusFire);
                GameObject nowFire = m_fireEffects.GetChild(Random.Range(0,m_fireEffects.childCount-2)).gameObject;
                if(nowFire.activeSelf == true){
                    nowFire.SetActive(false);
                    minusFire--;
                }
                safeCnt--;
            }
            m_nowCpuStep = step;
            return;
        }
    }
}
