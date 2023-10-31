using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// オブジェクトプールを管理するクラス
/// </summary>
public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
    /// <summary>
    /// 各プレハブに対応するオブジェクトプール
    /// </summary>
    private Dictionary<string, ObjectPool<GameObject>> m_poolDict = new Dictionary<string, ObjectPool<GameObject>>();
    //ObjectPool<GameObject> pool;

    /// <summary>
    /// 参照しているプレハブ
    /// </summary>
    public GameObject m_prefab {get; private set;}

    new void Awake()
    {
        base.Awake();
        //pool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    /// <summary>
    /// プールに取り出せるものがない時の生成処理
    /// </summary>
    /// <returns></returns>
    GameObject OnCreatePooledObject(){
        //return Instantiate(m_prefab, m_parent.transform);
        return Instantiate(m_prefab);
    }

    /// <summary>
    /// プールから取り出す時
    /// </summary>
    /// <param name="obj"></param>
    void OnGetFromPool(GameObject obj){
        if(obj == null)return;
        obj.SetActive(true);
    }

    /// <summary>
    /// プールにリリースする時
    /// </summary>
    /// <param name="obj"></param>
    void OnReleaseToPool(GameObject obj){
        obj.SetActive(false);
    }

    /// <summary>
    /// よくわからない
    /// </summary>
    /// <param name="obj"></param>
    void OnDestroyPooledObject(GameObject obj){
        Destroy(obj);
    }

    /// <summary>
    /// プールからオブジェクトを取得する
    /// </summary>
    /// <param name="prefab">取り出したいプレハブ</param>
    /// <param name="position">生成位置</param>
    /// <param name="rotation">生成回転</param>
    /// <returns>プールされたインスタンス</returns>
    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation){
        if(!m_poolDict.ContainsKey(prefab.name)){
            ObjectPool<GameObject> newPool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
            m_poolDict.Add(prefab.name, newPool);
        }
        m_prefab = prefab;
        GameObject obj = m_poolDict[prefab.name].Get();
        obj.name = prefab.name;
        Transform tf = obj.transform;
        tf.position = position;
        tf.rotation = rotation;
        return obj;
    }

    /// <summary>
    /// インスタンスをプールにリリース
    /// </summary>
    /// <param name="obj">インスタンス</param>
    public void ReleaseGameObject(GameObject obj){
        //既にリリースされたら処理しない
        if(!obj.activeSelf)return;
        if(m_poolDict.ContainsKey(obj.name)){
            m_poolDict[obj.name].Release(obj);
        }
        //pool.Release(obj);
    }

    public void ReleaseAllGameObjects(){
        foreach(string key in m_poolDict.Keys){
            m_poolDict[key].Clear();
        }
        m_poolDict.Clear();
    }

    // デバッグ用
    // private void Update() {
    //     foreach (string key in m_poolDict.Keys) {
    //         Debug.Log (key);
    //     }
    // }
}
