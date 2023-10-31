using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPGauge : MonoBehaviour
{
    [SerializeField] private Image m_hpGauge;

    [SerializeField] private Monster m_monster;

    private RectTransform m_rectTransform;

    private void Start() {
        //m_hpGauge = GetComponent<Image>();
        SetGaugeFill();
        m_rectTransform = GetComponent<RectTransform>();
        //this.gameObject.SetActive(false);
        //Debug.Log("false");
    }

    /// <summary>
    /// HPゲージを更新
    /// </summary>
    public void SetGaugeFill(){
        MonsterParamerter mp = m_monster.GetParamerter();
        if(mp.maxHp == 0)return;
        float amount = mp.hp / mp.maxHp;
        m_hpGauge.fillAmount = amount;
    }

    // private void Update() {
    //     Vector3 rot = m_rectTransform.rotation.eulerAngles;
    //     rot.z = -1 * m_monster.transform.rotation.eulerAngles.y;
    //     m_rectTransform.rotation = Quaternion.Euler(rot);
    // }
}
