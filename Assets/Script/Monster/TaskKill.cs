using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskKill : EffectMonster
{
    private CpuMain m_cpu;

    /// <summary>
    /// CPUパラメーター
    /// </summary>
    [SerializeField]CPULoad m_cpuLoad;

    private List<PlayerMonster> m_playerMonsterList = new List<PlayerMonster>();

    protected override void Start()
    {
        m_cpu = GameObject.Find("Managers").GetComponent<CpuMain>();

        m_playerMonsterList.Clear();

        foreach (GameObject obj in GameObject.Find("Managers").GetComponent<VisibleList>().GetVisibleList())
        {
            if(obj == null)continue;
            if(obj.CompareTag("Player"))
            {
                //プレビューはいらない
                if(obj.activeSelf){
                    m_playerMonsterList.Add(obj.GetComponent<PlayerMonster>());
                }
            }
            else if(obj.gameObject.name == "FireWall"){
                obj.GetComponent<FireWallObject>().Death();
                //Destroy(obj);
            }
        }

        //一気に消滅
        foreach(PlayerMonster pm in m_playerMonsterList){
            pm.Death();
        }

        //CPU削減
        m_cpu.UsageRegister(m_cpuLoad);

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
