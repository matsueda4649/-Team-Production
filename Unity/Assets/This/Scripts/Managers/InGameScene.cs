using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ManagerSystem
{
    /// <summary>
    /// ゲームシーンを管理
    /// </summary>
    public class InGameScene : MonoBehaviour
    {
        /// <summary>
        /// 移動方向
        /// </summary>
        private enum MoveDirection
        {
            /// <summary>
            /// 縦
            /// </summary>
            Vertical,
            /// <summary>
            /// 横
            /// </summary>
            Horizontal,
        };

        #region 変数宣言

        /// <summary>
        /// 移動方向
        /// </summary>
        [SerializeField] MoveDirection m_direction = MoveDirection.Horizontal;

        /// <summary>
        /// 背景
        /// </summary>
        [SerializeField] GameObject m_backGround = default;

        /// <summary>
        /// ラープ速度
        /// </summary>
        [SerializeField, Range(0f, 5f)]
        private float m_lerpSpeed = 1f;

        /// <summary>
        /// コントローラーウインドウ
        /// </summary>
        [SerializeField] GameObject m_controller = default;

        /// <summary>
        /// 再読み込みするシーン
        /// </summary>
        private string m_reloadSceneName;

        /// <summary>
        /// コントローラーのサイズ
        /// </summary>
        private Vector2 m_initScale;

        /// <summary>
        /// 背景の遠さ
        /// </summary>
        private float m_farAway;

        private SaveData m_saveData;

        public SaveData GetSaveData { get => m_saveData; }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期設定
        /// </summary>
        public void Initialize()
        {
            // 現在のシーン名を取得
            m_reloadSceneName = SceneManager.GetSceneAt(1).name;

            m_initScale = m_controller.transform.localScale;

            m_farAway = Mathf.Abs(m_backGround.transform.localPosition.z);

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            m_controller.transform.SetLocalScale(Vector2.zero);
#endif

        }

        /// <summary>
        /// セーブデータの取得
        /// </summary>
        public DataStage GetDataStage()
        {
            // データを取得
            m_saveData = SaveManager.Instance.GetSaveData;
            var questData = DataManager.Instance.GetDataStages;

            // クエストデータがあれば
            if (!questData.GetList.IsEmpty())
            {
                DataStage data;

                // セーブデータがある
                if (questData.GetDictionary.ContainsKey(m_saveData.questId))
                {
                    data = questData.GetDictionary[m_saveData.questId];
                    if (data.name != m_reloadSceneName)
                    {
                        data = GetData(m_saveData, questData.GetList, m_reloadSceneName);
                    }
                }
                else
                {
                    data = GetData(m_saveData, questData.GetList, m_reloadSceneName);
                }

                return data;
            }

            DebugManager.Error("StageID = Null");
            return null;
        }

        /// <summary>
        /// ステージデータの取得
        /// </summary>
        /// <param name="saveData">セーブデータ</param>
        /// <param name="list">ステージデータのリスト</param>
        /// <param name="thisSceneName">現在のシーン名</param>
        /// <returns></returns>
        private static DataStage GetData(SaveData saveData, IReadOnlyList<DataStage> list, string thisSceneName)
        {
            // 現在のシーン名から必要なデータを取得する
            for (int i = 0, count = list.Count; i < count; ++i)
            {
                // 現在のシーン名と設定されているシーン名が同じなら
                if (thisSceneName == list[i].name)
                {
                    saveData.questId = list[i].id;
                    SaveManager.Save(saveData);
                    return list[i];
                }
            }

            DebugManager.Error("StageID = Null");
            return null;
        }

        /// <summary>
        /// 背景をスクロール
        /// </summary>
        public void ScrollBackGround(Vector2 direction)
        {
            var start = m_backGround.transform.localPosition;
            start.z = start.z != 0 ? (start.z > 0 ? start.z : 1f / m_farAway) : 1f;

            // 座標保管
            var x = start.x;
            var y = start.y;

            // 移動方向の設定
            switch (m_direction)
            {
                case MoveDirection.Vertical:
                    y = direction.y / start.z;
                    break;
                case MoveDirection.Horizontal:
                    x = direction.x / start.z;
                    break;
            }

            var end = new Vector3(x, y, start.z);
            m_backGround.transform.localPosition = Vector3.Lerp(start, end, m_lerpSpeed);
        }

        /// <summary>
        /// ゲームオーバーのダイヤログを表示
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void ShowOverDialog(string message)
        {
            // ダイヤログを出す
            UIManager.Instance.ShowChoiceDialog(100, message, "Continue", "Quit", () =>
            {
                // Continueを押したら

                // 現在のシーンを再読み込みする
                GameSceneManager.Instance.LoadScene(m_reloadSceneName).Forget();

            }, () => {

                // Quitを押したら

                // 設定したシーンに戻る
                GameSceneManager.Instance.LoadScene("StageSelectScene").Forget();
            });
        }

        /// <summary>
        /// ウインドウの表示設定
        /// </summary>
        /// <param name="control"> コントローラーウインドウ</param>
        public void SetWindow(bool control = false)
        {
            // コントローラーウインドウ
            Vector2 scale = control ? m_initScale : Vector2.zero;
            m_controller.transform.localScale = scale;
        }

        #endregion
    }
}