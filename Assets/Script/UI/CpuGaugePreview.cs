using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CpuGaugePreview : MonoBehaviour
{
    [SerializeField]private CpuMain m_cpuMain;

    private Image m_image;

    private float m_addedUsage;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        m_addedUsage = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_addedUsage > 0f){
            float usage = m_cpuMain.Usage;
            if(usage > 100f)usage = 100f;
            m_image.fillAmount = (m_cpuMain.Usage + m_addedUsage) / 100f;
        }
        else{
            m_image.fillAmount = 0f;
        }
    }

    public void SetAddedUsage(float addedUsage){
        m_addedUsage = addedUsage;
    }
}
