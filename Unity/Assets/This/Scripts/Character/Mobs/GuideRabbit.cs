using UISystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// 案内ウサギ
    /// </summary>
    public class GuideRabbit : Guide
    {
        #region 変数宣言

        /// <summary>
        /// テキスト
        /// </summary>
        private FlashingText m_flashingText = default;

        /// <summary>
        /// Textの初期位置
        /// </summary>
        private Vector2 m_initTextPos;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            GetGuideText.SetComponent(ref m_flashingText);
            m_flashingText.Initialize();
            m_initTextPos = m_flashingText.transform.localPosition;
        }

        protected override void OnBecameVisible()
        {
            m_flashingText.gameObject.Activation();
            m_flashingText.JumpAnimation();
        }

        protected override void OnBecameInvisible()
        {
            m_flashingText.transform.SetLocalPosition(m_initTextPos);
            m_flashingText.gameObject.Deactovation();
        }

        #endregion
    }
}