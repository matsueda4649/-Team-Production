using Cysharp.Threading.Tasks;
using ManagerSystem;
using UnityEngine;

namespace WindowSystem
{
    /// <summary>
    /// ポーズウインドウ
    /// </summary>
    public class PauseWindow : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// ポーズ中のダイアログ
        /// </summary>
        private ChoicesDialogWindow m_pauseDialog = null;

        /// <summary>
        /// ポーズのダイヤログを取得
        /// </summary>
        public ChoicesDialogWindow GetPauseDialog { get => m_pauseDialog; }

        /// <summary>
        /// ボタンのクリック許可
        /// </summary>
        private bool m_allowOnClickButton = true;

        #endregion

        #region

        /// <summary>
        /// ポーズフェーズ
        /// </summary>
        public async UniTask PausePhase()
        {
            if (!m_allowOnClickButton) { return; }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_pauseDialog.IsNull())
                {
                    await RetireStage();
                }
                else
                {
                    m_pauseDialog.Close();
                }
            }
        }

        /// <summary>
        /// ステージ選択画面に戻る
        /// </summary>
        public void OnClickPauseButton()
        {
            AudioManager.Instance.PlaySe(AudioSe.Decision);
            RetireStage().Forget();
        }

        /// <summary>
        /// 現在のステージをリタイアする
        /// </summary>
        private async UniTask RetireStage()
        {
            m_pauseDialog = UIManager.Instance.ShowChoiceDialog(1001, "一時停止中", "リタイアする", "戻る", async () => {

                m_pauseDialog = null;

                m_allowOnClickButton = false;

                // 現在のステージをリタイアする
                await GameSceneManager.Instance.LoadScene("StageSelectScene");

            }, ()=> { m_pauseDialog = null; });

            await UniTask.Delay(DefTime.DELAY_TIME);
        }
        #endregion
    }
}