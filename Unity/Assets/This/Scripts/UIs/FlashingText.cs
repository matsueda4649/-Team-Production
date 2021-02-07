using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UISystem
{
    /// <summary>
    /// ガイド用のテキスト
    /// </summary>
    public class FlashingText : GuideText
    {
        #region 変数宣言

        /// <summary>
        /// テキスト
        /// </summary>
        private RectTransform m_rect;

        private Tween m_tween;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            this.SetComponent(ref m_rect);
        }

        /// <summary>
        /// テキストの協調
        /// </summary>
        public void JumpAnimation()
        {
            // 移動高さ
            var y = 0.5f;

            // アニメーションの時間
            var duration = 0.5f;

            m_tween = ExtensionDOTween.JumpAnimation(m_rect, y, duration);
        }

        private void OnDisable()
        {
            ExtensionDOTween.Kill(m_tween);
        }

        #endregion
    }
}