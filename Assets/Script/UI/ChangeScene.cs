using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 使用されない：SceneChangeを使う
/// </summary>
public class ChangeScene : MonoBehaviour
{
    //遷移先
    [SerializeField] string changeScene;

    [SerializeField] private CustomDeck m_customDeck;

    [SerializeField] private GameObject m_notSetDeckWarning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPushButton()
    {
        if(m_customDeck){
            if(m_customDeck.canEnterStage){
                PoolManager.Instance.ReleaseAllGameObjects();
                StageManager.Instance.currentStage = 4;
                Debug.Log(StageManager.Instance.currentStage);
                SceneManager.LoadScene(changeScene);
            }
            else{
                m_notSetDeckWarning.SetActive(true);
            }
        }
        else{
            PoolManager.Instance.ReleaseAllGameObjects();
            StageManager.Instance.currentStage = 4;
            Debug.Log(StageManager.Instance.currentStage);
            SceneManager.LoadScene(changeScene);
        }
    }
}
