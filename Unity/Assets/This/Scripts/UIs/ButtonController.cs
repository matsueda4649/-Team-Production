using DG.Tweening;
using UnityEngine;

namespace UISystem
{
    /// <summary>
    /// ボタンのコントローラー
    /// </summary>
    public class ButtonController : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// 強調
        /// </summary>
        [SerializeField] bool m_emphasiz = true;

        /// <summary>
        /// ボタンの座標
        /// </summary>
        [SerializeField] RectTransform m_button = default;

        /// DOTween m_button
        /// </summary>
        private Tween m_tween = null;

        #endregion

        #region メソッド

        private void Start()
        {
            AnimationButton();
        }

        /// <summary>
        /// ボタンのアニメーション
        /// </summary>
        private void AnimationButton()
        {
            if (m_emphasiz) { m_tween = EmphasizeButton(m_button); }
        }

        /// <summary>
        /// ボタンを強調するアニメーション
        /// </summary>
        private static Tween EmphasizeButton(RectTransform rect)
        {
            // 目標サイズ
            float endValue = 0.9f;

            // 時間
            var duration = 1.5f;

            return ExtensionDOTween.EmphasizeButton(rect, endValue, duration);
        }

        private void OnDisable()
        {
            ExtensionDOTween.Kill(m_tween);
        }

        #endregion
    }
}