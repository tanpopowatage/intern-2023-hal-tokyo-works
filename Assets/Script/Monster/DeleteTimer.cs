using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTimer : MonoBehaviour
{
    [SerializeField]private float m_lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Death",m_lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}
