using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// カードIDの変換を行うクラス
/// </summary>
public class CardMonsterDictionary : SingletonMonoBehaviour<CardMonsterDictionary>
{
    /// <summary>
    /// カード一覧のCSVファイル
    /// </summary>
    [Header("カード一覧のCSVファイル")]
    public TextAsset m_cardMonsterCSV;

    /// <summary>
    /// (int)カードID　⇔　(GameObject)モンスタープレハブのリスト
    /// </summary>
    private Dictionary<int, GameObject> m_cardMonsterDict = new Dictionary<int, GameObject>();

    /// <summary>
    /// (int)カードID　⇔　(MonsterParameter)モンスターパラメータのリスト
    /// </summary>
    private Dictionary<int, MonsterParamerter> m_cardMonsterParamDict = new Dictionary<int, MonsterParamerter>();

    new void Awake(){
        //シングルトン
        base.Awake();
        //CSV読み込む
        ReadCardMonsterCSV();
    }

    /// <summary>
    /// カードIDを対応しているモンスターのプレハブを取り出す
    /// </summary>
    /// <param name="cardId">カードID</param>
    /// <returns>(GameObject)モンスタープレハブ</returns>
    public GameObject GetMonsterPrefab(int cardId){
        return m_cardMonsterDict[cardId];
    }

    /// <summary>
    /// カードIDを対応しているモンスターのパラメータを取り出す
    /// </summary>
    /// <param name="cardId">カードID</param>
    /// <returns>(MonsterParameter)モンスターパラメータ</returns>
    public MonsterParamerter GetMonsterParamerter(int cardId){
        if(m_cardMonsterParamDict.ContainsKey(cardId)){
            return m_cardMonsterParamDict[cardId];
        }
        else{return new MonsterParamerter();}
    }

    /// <summary>
    /// CSVを読み込む
    /// </summary>
    private void ReadCardMonsterCSV(){
        //StringReaderを作成
        StringReader strReader = new StringReader(m_cardMonsterCSV.text);
        bool eof = false;
        //項目名の行を省略
        strReader.ReadLine();
        //ファイルの最後まで読み込む
        while(!eof){
            string data_str = strReader.ReadLine();
            if(data_str == null){
                eof = true;
                break;
            }
            //一行の要素を配列に分割
            var values = data_str.Split(',');
            //読み込んだペアをDictionaryに追加                    //CSVファイル各列の項目
            m_cardMonsterDict.Add(Int32.Parse(values[0]),         //1. ID
             Resources.Load<GameObject>(values[1].ToString())   //2. ファイル名
             );
            MonsterParamerter mp = new MonsterParamerter{
                hp = Int32.Parse(values[2]),                    //3. HP
                speed = float.Parse(values[3]),                 //4. スピード
                attack = Int32.Parse(values[4]),                //5. 攻撃力
                attackDistance = float.Parse(values[5]),        //6. 攻撃距離
                attackInterval = float.Parse(values[6]),        //7. 攻撃CT
                constantLoad = new CPULoad{
                    raiseRate = float.Parse(values[7]),         //8. 常時上昇率
                    impactTime = -1
                    },
                spawnLoad = new CPULoad{
                    raiseRate = float.Parse(values[8]),         //9. 出現上昇（率）
                    impactTime = float.Parse(values[9])         //10.出現上昇（時間）
                    },
                attackLoad = new CPULoad{
                    raiseRate = float.Parse(values[10]),        //11.攻撃上昇（率）
                     impactTime = float.Parse(values[11])       //12.攻撃上昇（時間）
                     },
                DestroyLoad = new CPULoad{
                    raiseRate = float.Parse(values[12]),        //13.消失上昇（率）
                    impactTime = float.Parse(values[13])        //14.消失上昇（時間）
                    }
            };
            m_cardMonsterParamDict.Add(Int32.Parse(values[0]), mp);
        }
    }
}
