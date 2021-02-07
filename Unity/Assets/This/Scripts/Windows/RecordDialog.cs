using System;
using DG.Tweening;
using ManagerSystem;
using TMPro;
using UnityEngine;
using static ExtensionDOTween;

namespace WindowSystem
{
    /// <summary>
    /// ゲームクリア時のダイヤログ
    /// </summary>
    public class RecordDialog : BaseDialog
    {
        #region 変数宣言

        /// <summary>
        /// 新記録更新時に表示するテキスト
        /// </summary>
        [SerializeField] TextMeshProUGUI m_newRecordText = default;

        /// <summary>
        /// クリア時間を表示するテキスト
        /// </summary>
        [SerializeField] TextMeshProUGUI m_scoreText = default;

        /// <summary>
        /// クリック時のコールバック
        /// </summary>
        private Action m_onClickCallback; 

        private Tween m_tween;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="newRecord">新記録更新判定</param>
        /// <param name="clearTime">クリア時間</param>
        /// <param name="callback">Closeボタンクリック時のコールバック</param>
        public void Initialize(string message, bool newRecord, float clearTime, Action callback)
        {
            Initialize(1001, message);

            if (!m_newRecordText.IsNull())
            {
                m_newRecordText.enabled = newRecord;
                m_tween = NewRecordAnimation(m_newRecordText.rectTransform, 20f, 0.4f);
            }

            if (!m_scoreText.IsNull()) { m_scoreText.text = $"TIME : {clearTime:F1}"; }

            m_onClickCallback = callback;

            Open();
        }

        /// <summary>
        /// Yesボタンのクリック時
        /// </summary>
        public void OnClickYesButton()
        {
            // ポップ中は受け付けない
            if (!PopFlag)
            {
                m_onClickCallback?.Invoke();

                // SEを再生
                AudioManager.Instance.PlaySe(AudioSe.Decision);

                Close();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Kill(m_tween);
        }

        #endregion
    }
}