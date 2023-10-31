using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIArrows : MonoBehaviour
{
    [SerializeField] private RectTransform m_leftButtons;

    [SerializeField] private RectTransform m_rightButtons;

    [SerializeField] private GameObject m_leftArrow;

    [SerializeField] private GameObject m_rightArrow;

    [SerializeField] private float m_movePerFrame = 10f;

    private float m_moveAmount;

    private void Start() {
        m_leftArrow.SetActive(false);
        m_rightButtons.gameObject.SetActive(false);
    }

    public void StartMovingButton(bool toLeft){
        if(toLeft){
            m_moveAmount = -1 * m_movePerFrame;
            m_rightButtons.gameObject.SetActive(true);
        }
        else{
            m_moveAmount = m_movePerFrame;
            m_leftButtons.gameObject.SetActive(true);
        }
        m_leftArrow.SetActive(false);
        m_rightArrow.SetActive(false);
        StartCoroutine(MoveButtonCoroutine(toLeft));
    }

    IEnumerator MoveButtonCoroutine(bool toLeft){
        if(toLeft){
            while(m_rightButtons.anchoredPosition.x > 0){
                Vector2 leftPos = m_leftButtons.anchoredPosition;
                leftPos.x += m_moveAmount;
                m_leftButtons.anchoredPosition = leftPos;

                Vector2 rightPos = m_rightButtons.anchoredPosition;
                rightPos.x += m_moveAmount;
                m_rightButtons.anchoredPosition = rightPos;

                yield return new WaitForFixedUpdate();
            }

            m_rightButtons.anchoredPosition = Vector2.zero;

            m_leftArrow.SetActive(true);

            m_leftButtons.gameObject.SetActive(false);

            foreach(Button b in m_rightButtons.GetComponentsInChildren<Button>()){
                b.interactable = true;
            }
        }
        else{
            while(m_leftButtons.anchoredPosition.x < 0){
                Vector2 leftPos = m_leftButtons.anchoredPosition;
                leftPos.x += m_moveAmount;
                m_leftButtons.anchoredPosition = leftPos;

                Vector2 rightPos = m_rightButtons.anchoredPosition;
                rightPos.x += m_moveAmount;
                m_rightButtons.anchoredPosition = rightPos;

                yield return new WaitForFixedUpdate();
            }

            m_leftButtons.anchoredPosition = Vector2.zero;

            m_rightArrow.SetActive(true);

            m_rightButtons.gameObject.SetActive(false);

            foreach(Button b in m_leftButtons.GetComponentsInChildren<Button>()){
                b.interactable = true;
            }
        }
    }
}
