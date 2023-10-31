using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// カメラの可視範囲内にあるオブジェクトリストを管理するクラス
/// </summary>
public class VisibleList : MonoBehaviour
{
    /// <summary>
    /// 可視リスト
    /// </summary>
    private List<GameObject> m_visibleObjects;

    /// <summary>
    /// 可視リスト内の空き要素のリスト
    /// </summary>
    private List<int> m_emptyIndex = new List<int>(); //空いているスロットのリスト

    private void Awake(){
        m_visibleObjects = new List<GameObject>();
    }

    /// <summary>
    /// 可視リストを取得
    /// </summary>
    /// <returns>可視リスト</returns>
    public List<GameObject> GetVisibleList(){
        return m_visibleObjects;
    }

    /// <summary>
    /// VisibleObjectに追加
    /// </summary>
    /// <param name="gameObject">オブジェクト</param>
    /// <returns>リストにいるインデックス</returns>
    public int AddVisibleObject(GameObject gameObject){
        //空いているところあったらそこに挿入
        if(m_emptyIndex.Count != 0){
            int lastEmptyIndex = m_emptyIndex[m_emptyIndex.Count-1];
            //本当に空いているかチェック
            Assert.IsNull(m_visibleObjects[lastEmptyIndex]);
            m_visibleObjects[lastEmptyIndex] = gameObject;
            m_emptyIndex.RemoveAt(m_emptyIndex.Count-1); //最後尾からの処理で速度がO(1)かな？
            return lastEmptyIndex;
        }
        //空いていないなら最後に挿入
        m_visibleObjects.Add(gameObject);
        return m_visibleObjects.Count - 1;
    }

    /// <summary>
    /// VisibleObjectから削除
    /// </summary>
    /// <param name="index">オブジェクトが持っているリストインデックス</param>
    public void RemoveVisibleObject(int index){
        if(m_visibleObjects[index] == null)return;
        m_visibleObjects[index] = null;
        //indexを空いているスロットに登録
        m_emptyIndex.Add(index);
    }

}
