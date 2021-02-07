using Cysharp.Threading.Tasks;
using ManagerSystem;
using TMPro;
using UnityEngine;

namespace UISystem
{
    /// <summary>
    /// クエストノード
    /// </summary>
    public class StageNode : BaseNode
    {
        #region 変数宣言

        /// <summary>
        /// ステージデータ
        /// </summary>
        private DataStage m_dataStage;

        private SaveData m_saveData;

        /// <summary>
        /// ステージの進行状況を表示するText
        /// </summary>
        [SerializeField] TextMeshProUGUI m_stateText = default;

        /// <summary>
        /// スコアを表示するText
        /// </summary>
        [SerializeField] TextMeshProUGUI m_scoreText = default;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="dataStage"> ステージデータ </param>
        /// <param name="saveData">   セーブデータ     </param>
        public void Initialize(DataStage dataStage, SaveData saveData)
        {
            m_saveData = saveData;

            // パラメータ設定
            m_dataStage = dataStage;

            // タイトル名の設定
            GetTitle.text = m_dataStage.name;

            // Textの設定
            bool isClear = saveData.clearStageIds.Contains(m_dataStage.id);
            m_stateText.color = isClear ? Colors.StrongGreen : Color.red;
            m_stateText.text  = isClear ? "Clear" : "Next";
            m_scoreText.text = saveData.score.ScoreText(dataStage.id, isClear);
        }

        /// <summary>
        /// ノードをクリックしたとき
        /// </summary>
        public override void OnClickNode()
        {
            base.OnClickNode();

            // セーブデータにIDを設定
            SaveManager.Instance.GetSaveData.questId = m_dataStage.id;
            SaveManager.Save(m_saveData);

            GameSceneManager.Instance.LoadScene(m_dataStage.name).Forget();
        }
        

        #endregion
    }
}