using TMPro;
using UnityEngine;

namespace UISystem 
{
    /// <summary>
    /// ガイド用のテキスト
    /// </summary>
    public class GuideText : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// テキスト
        /// </summary>
        [SerializeField] TextMeshProUGUI m_guideText = default;

        #endregion

        #region メソッド

        /// <summary>
        /// テキストの変更
        /// </summary>
        /// <param name="changeMessage">変更するメッセージ</param>
        public void ChangeText(string changeMessage)
        {
            if (!m_guideText.IsNull())
            {
                m_guideText.text = changeMessage;
            }
        }

        #endregion
    }
}