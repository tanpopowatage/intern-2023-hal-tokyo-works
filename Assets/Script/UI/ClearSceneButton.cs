using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSceneButton : MonoBehaviour
{
    [SerializeField] private GameObject m_retryButton;
    [SerializeField] private GameObject m_nextButton;
    void Start()
    {
        if(StageManager.Instance.IsLastStage()){
            m_retryButton.SetActive(true);
            m_nextButton.SetActive(false);
        }
        else{
            m_retryButton.SetActive(false);
            m_nextButton.SetActive(true);
        }
    }
}
