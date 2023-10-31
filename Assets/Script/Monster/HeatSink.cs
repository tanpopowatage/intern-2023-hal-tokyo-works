using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSink : EffectMonster
{
    /// <summary>
    /// CPU
    /// </summary>
    private CpuMain m_cpu;

    /// <summary>
    /// CPU消費量
    /// </summary> <summary>
    /// 
    /// </summary>
    [SerializeField] private CPULoad m_cpuLoad;

    /// <summary>
    /// ダウン間隔
    /// </summary>
    [SerializeField] private float m_cpuDownInterval = 1;

    [SerializeField] private float m_lifeTime = 10;
    // Start is called before the first frame update
    protected override void Start()
    {
        m_cpu = GameObject.Find("Managers").GetComponent<CpuMain>();

        InvokeRepeating("CPUDown",0,m_cpuDownInterval);
        Invoke("Death",m_lifeTime);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    private void CPUDown()
    {
        m_cpu.UsageRegister(m_cpuLoad);
    }

}
