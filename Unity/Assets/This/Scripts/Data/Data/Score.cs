using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ステージのスコアのデータ
/// </summary>
[System.Serializable]
public class Score : ISerializationCallbackReceiver
{
    #region 変数宣言

    public Dictionary<int, float> scoreDic = new Dictionary<int, float>(48);

    /// <summary>
    /// Keyのリスト
    /// </summary>
    [SerializeField] List<int> m_keyList;

    /// <summary>
    /// Valueのリスト
    /// </summary>
    [SerializeField] List<float> m_valueList;

    #endregion

    #region メソッド

    /// <summary>
    /// スコアのテキスト表示
    /// </summary>
    /// <param name="id">ステージID</param>
    /// <param name="isClear">クリアしているかどうか</param>
    /// <returns></returns>
    public string ScoreText(int id, bool isClear)
    {
        string text = "";
        if (isClear) 
        {
            text = string.Format("{0:F1}", scoreDic[id]);
            text = $"BEST TIME : {text.PadLeft(5)}"; 
        }
        return text;
    }

    /// <summary>
    /// スコアの更新
    /// </summary>
    /// <param name="id">ステージID</param>
    /// <param name="newClearTime">新しいクリア時間</param>
    /// <returns></returns>
    public bool UpdateScore(int id, float newClearTime)
    {
        // 更新したかどうか
        bool update = false;

        if(scoreDic.ContainsKey(id))
        {
            if (scoreDic[id] > newClearTime)
            {
                update = true;
                scoreDic[id] = newClearTime;
            }
        }
        else
        {
            update = true;
            scoreDic.Add(id, newClearTime);
        }

        return update;
    }

    /// <summary>
    /// Json化時に実行
    /// </summary>
    public void OnBeforeSerialize()
    {
        m_keyList = scoreDic.Keys.ToList();
        m_valueList = scoreDic.Values.ToList();
    }

    /// <summary>
    /// Jsonからデシリアライズされた後に実行
    /// </summary>
    public void OnAfterDeserialize()
    {
        scoreDic = m_keyList.Select((id, index) =>
        {
            var value = m_valueList[index];
            return new { id, value };
        }).ToDictionary(x=> x.id, x=> x.value);

        m_keyList.Clear();
        m_valueList.Clear();
    }

    #endregion
}
