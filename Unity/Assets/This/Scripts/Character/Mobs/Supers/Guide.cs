using UISystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// ガイド
    /// </summary>
    public abstract class Guide : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// ガイド用のテキスト
        /// </summary>
        [SerializeField] GuideText m_guideText = default;

        /// <summary>
        /// ガイド用のテキストを取得
        /// </summary>
        public GuideText GetGuideText { get => m_guideText; }

        #endregion

        #region メソッド

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// ガイドのメッセージの変更
        /// </summary>
        /// <param name="guideMessage">ガイドのメッセージ</param>
        public void ChangeGuideText(string guideMessage)
        {
            m_guideText.ChangeText(guideMessage);
        }

        protected abstract void OnBecameVisible();

        protected abstract void OnBecameInvisible();

        #endregion
    }
}