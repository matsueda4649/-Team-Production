using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WindowSystem;

namespace ManagerSystem
{
    /// <summary>
    /// タイトル画面管理
    /// </summary>
    public class TitleManager : BaseScene, IUpdatable
    {
        /// <summary>
        /// DOTweenを適用させるもの
        /// </summary>
        private enum Index
        {
            TitleWindow = 0,
            TopTitleText,
            OptionButton,
        }

        /// <summary>
        /// プラットフォーム
        /// </summary>
        private enum PlatformName
        {
            Android = 0,
            PC,
        }

        /// <summary>
        /// シーン名
        /// </summary>
        public static readonly string SceneName = "TitleScene";

        #region 変数宣言

        /// <summary>
        /// 読み込むシーン名
        /// </summary>
        [SerializeField] string m_loadSceneName = default;

        /// <summary>
        /// タイトルウインドウ
        /// </summary>
        [SerializeField] TitleWindow m_titleWindow = default;

        /// <summary>
        /// コンテニューボタン
        /// </summary>
        [SerializeField] Button m_continueButton = default;

        /// <summary>
        /// ニューゲームボタン
        /// </summary>
        [SerializeField] Button m_newGameButton = default;

        /// <summary>
        /// 操作説明用のダイヤログの配列
        /// </summary>
        [SerializeField] OperationExplanationWindow[] m_operationExplanationWindowArray = default;

        /// <summary>
        /// セーブデータ
        /// </summary>
        private SaveData m_saveData;

        /// <summary>
        /// ボタンのクリック許可
        /// </summary>
        private bool m_allowOnClickButton = false;

        #endregion

        #region メソッド

        public override string GetSceneName()
        {
            return SceneName;
        }

        public override async UniTask Initialize()
        {
            m_saveData = SaveManager.Instance.GetSaveData;

            m_continueButton.SetActive(!m_saveData.onNewGame);

            // タイトルウィンドウの初期設定をする
            m_titleWindow.ShowTitleWindow((int)Index.TitleWindow, (int)Index.TopTitleText);

            // BGMを再生する
            AudioManager.Instance.PlayBgm(AudioBgm.Title);

            await UniTask.Delay(DefTime.DELAY_TIME);

            m_allowOnClickButton = true;
        }

        public void UpdateMe()
        {
            if (!m_allowOnClickButton) { return; }

            DebugManager.EndApp();
        }

        /// <summary>
        /// Continueボタンをクリックした時
        /// </summary>
        public void OnClickContinueButton()
        {
            m_allowOnClickButton = false;

            // SEを再生
            AudioManager.Instance.PlaySe(AudioSe.Decision);

            GameSceneManager.Instance.LoadScene(m_loadSceneName).Forget();
        }

        /// <summary>
        /// NewGameButtonクリックしたとき
        /// </summary>
        public void OnClickNewGameButton()
        {
            m_newGameButton.interactable = false;

            // SEを再生
            AudioManager.Instance.PlaySe(AudioSe.Decision);

            UIManager.Instance.ShowChoiceDialog(1000, "はじめから始めますか.", "はい", "いいえ", async () =>
            {
                // Yesのとき
                m_allowOnClickButton = false;

                // セーブデータを削除し、新規データを読み込む
                SaveManager.Instance.ResetData();

                // ムービを再生
                await GameSceneManager.Instance.LoadScene("MovieScene");
            }, ()=> {
                m_newGameButton.interactable = true;
            });
        }

        /// <summary>
        /// ボタンクリック時
        /// </summary>
        /// <param name="button"></param>
        public void OnClickButton(RectTransform button)
        {
            // SEを再生
            AudioManager.Instance.PlaySe(AudioSe.Decision);

            switch (button.name)
            {
                case "OptionButton":
                    OpenOperationExplanationWindow(button);
                    break;
                case "AllStageOpenButton":
                    OpenAllStage(m_saveData);
                    break;
            }
        }

        /// <summary>
        /// 操作説明のダイヤログの表示
        /// </summary>
        /// <param name="button">ボタン</param>
        private void OpenOperationExplanationWindow(RectTransform button)
        {
            m_titleWindow.RotateObject(button);

            OperationExplanationWindow dialog;

#if UNITY_EDITOR
            dialog = UIManager.CreateBaseDialog(m_operationExplanationWindowArray[(int)PlatformName.PC], this.transform);
#elif UNITY_STANDALONE
            dialog = UIManager.CreateBaseDialog(m_operationExplanationWindowArray[(int)PlatformName.PC], this.transform);
#elif UNITY_WEBGL
            dialog = UIManager.CreateBaseDialog(m_operationExplanationWindowArray[(int)PlatformName.PC], this.transform);
#elif UNITY_ANDROID
            dialog = UIManager.CreateBaseDialog(m_operationExplanationWindowArray[(int)PlatformName.Android], this.transform);
#endif

            dialog.Initialize(1000);
        }

        /// <summary>
        /// すべてのステージを新規開放
        /// </summary>
        private static void OpenAllStage(SaveData saveData)
        {
            var stageData = DataManager.Instance.GetDataStages;
            var count = stageData.GetList.Count;
            for(int i = 0; i < count; ++i)
            {
                var dataStage = stageData.GetList[i];

                if (dataStage.id > 0)
                {
                    if(!saveData.newStageIds.Contains(dataStage.id))
                    {
                        saveData.newStageIds.Add(dataStage.id);
                    }
                    if(!saveData.openStageIds.Contains(dataStage.id))
                    {
                        saveData.openStageIds.Add(dataStage.id);
                    }
                }
            }

            UIManager.Instance.ShowYesDialog(1001, "ステージをすべて\nオープンしました.", "Close", () => {
                SaveManager.Save(saveData);
            });
        }

        public void OnEnable()
        {
            UpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            UpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}