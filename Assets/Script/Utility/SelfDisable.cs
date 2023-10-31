using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisable : MonoBehaviour
{
    /// <summary>
    /// 消えるまでかかる時間
    /// </summary>
    [SerializeField] private float m_disableTime = 2f;

    private void OnEnable() {
        Invoke("DisableThis", m_disableTime);
    }

    private void DisableThis(){
        this.gameObject.SetActive(false);
    }
}
