using System.Collections.Generic;
using ManagerSystem;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using static DefNum;

namespace WindowSystem
{
    /// <summary>
    /// スクロールビュー
    /// </summary>
    public class ScrollView : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// ビューのウインドウ
        /// </summary>
        [SerializeField] Image m_viewWindow = default;

        /// <summary>
        /// コンテンツ
        /// </summary>
        [SerializeField] RectTransform m_content = default;

        /// <summary>
        /// ノード
        /// </summary>
        [SerializeField] StageNode m_questNode = default;

        #endregion

        #region メソッド

        /// <summary>
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="initId">最初に生成されるノードの初期ID</param>
        /// <param name="dataStageList">ステージデータのリスト</param>
        /// <param name="saveData">セーブデータ</param>
        public void Initialize(int index, int initId, List<DataStage> dataStageList, SaveData saveData)
        {
            // データのリストがなければ
            if (dataStageList.IsEmpty()) 
            {
                DebugManager.Error("StageData = Null");
                return;
            }

            // 背景を設定
            m_viewWindow.sprite = dataStageList[initId].icon;

            // クエストの数だけノードを生成する
            for (int i = 0, count = dataStageList.Count ; i < count; ++i)
            {
                int id = dataStageList[i].id;

                // 解放済みクエストのリストにIDがなければ
                if (!saveData.openStageIds.Contains(id)){ continue; }

                // IDが初期ID未満
                if (id < STAGE_INIT_NUMBER) { continue; }

                if (index == (id / STAGE_INIT_NUMBER))
                {
                    // ノードを生成し、初期化
                    StageNode questNode = Instantiate(m_questNode, m_content);
                    questNode.transform.SetLocalScale(Vector3.one);

                    if (!questNode.IsNull())
                    {
                        // データを初期化
                        questNode.Initialize(dataStageList[i], saveData);
                    }
                }
                else if (index < (id / STAGE_INIT_NUMBER))
                {
                    break;
                }
            }
        }

        #endregion
    }
}