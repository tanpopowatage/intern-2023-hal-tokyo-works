using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnEffect : MonoBehaviour
{
    /// <summary>
    /// 表示するオブジェクト（位置）
    /// </summary>
    private GameObject m_target = null;

    /// <summary>
    /// 自動で消える時間
    /// </summary>
    [SerializeField] private float m_lifeTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Death",m_lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_target != null)
        {
            transform.position = m_target.transform.position;
        }
        else
        {
            //ターゲットが死んだら自滅
            Destroy(this.gameObject);
        }
    }

    public void SetTarget(GameObject obj)
    {
        m_target = obj;
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }


}
