using UnityEngine;
using WindowSystem;

namespace ManagerSystem
{
    /// <summary>
    /// オプション画面を管理
    /// </summary>
    public class OptionManager : SingletonMonoBehaviour<OptionManager>
    {
        #region 変数宣言

        /// <summary>
        /// キャンバス
        /// </summary>
        [SerializeField] Canvas m_canvas = default;

        /// <summary>
        /// トップウインドウ
        /// </summary>
        [SerializeField] GameObject m_topWindow = default;

        /// <summary>
        /// オーディオウインドウ
        /// </summary>
        [SerializeField] AudioWindow m_audioWindow = default;

        /// <summary>
        /// クレジットウインドウ
        /// </summary>
        [SerializeField] GameObject m_creditWindow = default;

        #endregion

        #region メソッド

        protected override void Awake()
        {
            base.Awake();

            m_canvas.enabled = false;
        }

        /// <summary>
        /// Topウインドウを開く
        /// </summary>
        public void OpenTopWindow()
        {
            m_canvas.enabled = true; ;

            SetActiveWindow(top: true);
        }

        /// <summary>
        /// ウインドウを閉じる
        /// </summary>
        private void CloseWindow()
        {
            m_canvas.enabled = false;

            SetActiveWindow();
        }

        /// <summary>
        /// オーディオウインドウを開く
        /// </summary>
        private void OpenAudioWindow()
        {
            SetActiveWindow(audio: true);
        }

        /// <summary>
        /// クレジットウインドウを開く
        /// </summary>
        private void OpenCreditWindow()
        {
            SetActiveWindow(credit: true);
        }

        /// <summary>
        /// ウインドウの表示管理
        /// </summary>
        /// <param name="top">    トップ     </param>
        /// <param name="audio">  オーディオ </param>
        /// <param name="credit"> クレジット </param>
        private void SetActiveWindow(bool top = false, bool audio = false, bool credit = false)
        {
            // トップ
            m_topWindow.gameObject.SetActive(top);

            // オーディオ
            m_audioWindow.gameObject.SetActive(audio);
            if (audio) {  m_audioWindow.Initialize(); }

            // クレジット
            m_creditWindow.gameObject.SetActive(credit);
        }

        /// <summary>
        /// ボタンクリック時
        /// </summary>
        /// <param name="button"> ボタン </param>
        public void OnClickButton(RectTransform button)
        {
            switch (button.name)
            {
                // オプションボタン
                case "OptionButton":
                    AudioManager.Instance.PlaySe(AudioSe.Decision);
                    OpenTopWindow();
                    break;

                // 閉じるボタン
                case "CloseButton":
                    AudioManager.Instance.PlaySe(AudioSe.Close);
                    CloseWindow();
                    break;

                // オーディオボタン
                case "AudioButton":
                    AudioManager.Instance.PlaySe(AudioSe.Decision);
                    OpenAudioWindow();
                    break;

                // クレジットボタン
                case "CreditButton":
                    AudioManager.Instance.PlaySe(AudioSe.Decision);
                    OpenCreditWindow();
                    break;
            }
        }

        /// <summary>
        /// トップウインドウに戻る
        /// </summary>
        public void ReturnToTopWindow()
        {
            // SEを再生
            AudioManager.Instance.PlaySe(AudioSe.Close);

            OpenTopWindow();
        }

        #endregion
    }
}