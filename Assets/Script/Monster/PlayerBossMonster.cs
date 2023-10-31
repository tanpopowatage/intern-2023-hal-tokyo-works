using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBossMonster : PlayerMonster
{
    /// <summary>
    /// ダメージ持続時間（-1:永続的）
    /// </summary>
    [Header("ダメージ持続時間（-1:永続的）")]
    [SerializeField]private float m_damageTime = -1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
       CheckVisible();
    }

    //ターゲットへ攻撃
    public override void Action()
    {
       
    }

    public override void Death()
    {
        Destroy(this.gameObject);
    }

    public override void ChangeHP(float val)
    {
        //base.ChangeHP(val);
        //デバッグ用ダメージ演出
        GameObject spawnText = Instantiate(m_damageText,transform.position + new Vector3( 0.0f, 1.0f, 0.0f), Quaternion.identity);
        spawnText.GetComponent<TextMeshPro>().text = val.ToString();
        CPULoad cpuLoad = new CPULoad{raiseRate = val, impactTime = m_damageTime};
        cpuMain.UsageRegister(cpuLoad);
    }

    protected override void ShowHPGauge()
    {

    }
}
