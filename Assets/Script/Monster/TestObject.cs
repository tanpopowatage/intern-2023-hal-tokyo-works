using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使っていない：テスト用オブジェクト
/// </summary>
public class TestObject : MonoBehaviour
{
    private int m_LifeTime;
    // Start is called before the first frame update
    void Start()
    {
        m_LifeTime = 120;
    }

    void Update(){
        m_LifeTime--;
        if(m_LifeTime < 0){
            DestroyThis();
        }
    }

    void DestroyThis(){
        //InstantiateManager.Instance.DestroyMonster(this.gameObject);
    }
}
