using System;
using System.Collections.Generic;
using DG.Tweening;
using ManagerSystem;
using TMPro;
using UnityEngine;

namespace WindowSystem
{
    /// <summary>
    /// ダイヤログを管理
    /// </summary>
    public class ChoicesDialogWindow : BaseDialog
    {
        #region 変数宣言

        /// <summary>
        /// YESボタンのテキスト
        /// </summary>
        [SerializeField] TextMeshProUGUI m_yesText = default;
        /// <summary>
        /// NOボタンのテキスト
        /// </summary>
        [SerializeField] TextMeshProUGUI m_noText  = default;

        /// <summary>
        /// YESボタンのコールバック
        /// </summary>
        private Action m_yesCallback = default;
        /// <summary>
        /// Noボタンのコールバック
        /// </summary>
        private Action m_noCallback = default;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="sortingOrder"> ソーティングレイヤーのオーダー順 </param>
        /// <param name="message"> メッセージ </param>
        /// <param name="yesText"> Yesボタンのテキスト </param>
        /// <param name="noText">  Noボタンのテキスト</param>
        /// <param name="yesCallback"> Yesボタンのクリック時のコールバック </param>
        /// <param name="noCallback">  Noボタンのクリック時のコールバック  </param>
        public void Initialize(int sortingOrder, string message, string yesText, string noText
            , Action yesCallback, Action noCallback)
        {
            Initialize(sortingOrder, message);

            if (!m_yesText.IsNull()) { m_yesText.text = yesText; }
            if (!m_noText.IsNull())  { m_noText.text  = noText;  }

            // コールバックを登録
            m_yesCallback = yesCallback;
            m_noCallback  = noCallback;

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
                m_yesCallback?.Invoke();

                // SEを再生
                AudioManager.Instance.PlaySe(AudioSe.Decision);

                Close();
            }
        }

        /// <summary>
        /// Noボタンのクリック時
        /// </summary>
        public void OnClickNoButton()
        {
            // ポップ中は受け付けない
            if (!PopFlag)
            {
                m_noCallback?.Invoke();

                // SEを再生
                AudioManager.Instance.PlaySe(AudioSe.Close);

                Close();
            }
        }

        #endregion
    }
}