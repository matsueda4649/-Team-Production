using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DefsWindow;
using static ExtensionDOTween;

namespace WindowSystem
{
    /// <summary>
    /// ダイヤログのベース
    /// </summary>
    public abstract class BaseDialog : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// ウィンドウ
        /// </summary>
        [SerializeField] RectTransform m_window = default;
        /// <summary>
        /// キャンバス
        /// </summary>
        [SerializeField] Canvas m_canvas = default;

        /// <summary>
        /// キャンバスの取得
        /// </summary>
        public Canvas GetCanvas { get => m_canvas; }

        /// <summary>
        /// 背景
        /// </summary>
        [SerializeField] Image m_background = default;

        /// <summary>
        /// メッセージ
        /// </summary>
        [SerializeField] TextMeshProUGUI m_message = default;

        /// <summary>
        /// ポップのフラグ
        /// </summary>
        public bool PopFlag { get; private set; }

        /// <summary>
        /// ダイヤログ 表示時
        /// </summary>
        private List<Tween> m_tweenList = new List<Tween>(2);

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="sortingOrder"></param>
        /// <param name="message"></param>
        public void Initialize(int sortingOrder, string message)
        {
            m_canvas.sortingOrder = sortingOrder;

            // テキスト設定
            if (!m_message.IsNull()) { m_message.text = message; }

            // 最小サイズに設定
            m_window.localScale = Vector3.one * MIN_SIZE;
        }

        /// <summary>
        /// ウインドウの表示
        /// </summary>
        public virtual void Open()
        {
            Time.timeScale = DefTime.STOP;

            PopFlag = true;

            m_background.enabled = true;

            // 拡大
            m_tweenList.Add(DOScale(m_window, Vector2.one * MAX_SIZE, POP_OPEN_SPEED, () => {

                // ポップを終了
                PopFlag = false;
            }));
        }

        /// <summary>
        /// ウインドウを閉じる
        /// </summary>
        public virtual void Close()
        {
            PopFlag = true;

            m_background.enabled = false;

            // 縮小
            m_tweenList.Add(DOScale(m_window, Vector2.one * MIN_SIZE, POP_CLOSE_SPEED, () => {

                Time.timeScale = DefTime.PLAY;

                // 終わったら自身を破棄
                Destroy(gameObject);
            }));
        }

        protected virtual void OnDisable()
        {
            Kill(m_tweenList);
        }

        #endregion
    }
}