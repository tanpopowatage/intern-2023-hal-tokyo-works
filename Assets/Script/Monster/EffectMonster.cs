using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMonster : Monster
{
    protected override void Start()
    {
        base.Start();
    }
    
    protected override void Update()
    {
        
    }

    public override void Action()
    {
        
    }

    public override void Death()
    {
        cpuMain.UsageRegister(m_parameter.DestroyLoad);
        CPULoad constant = new CPULoad{raiseRate = -1 * m_parameter.constantLoad.raiseRate, impactTime = -1};
        cpuMain.UsageRegister(constant);
        
        Destroy(this.gameObject);
    }
}
