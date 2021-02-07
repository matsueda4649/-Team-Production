using UnityEngine;
using System.Diagnostics;
using ControllerSystem;
using UnityEngine.UI;

namespace WindowSystem
{
    /// <summary>
    /// ジャンプコントローラー
    /// </summary>
    public class JumpController : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// プレイヤーコントローラー
        /// </summary>
        private BasePlayerController m_playerController;

        /// <summary>
        /// ボアtんのイメージ
        /// </summary>
        private Image m_buttonImage = null;

        /// <summary>
        /// 初期カラー
        /// </summary>
        private Color m_initColor;

        /// <summary>
        /// ボタン押下時の色
        /// </summary>
        private Color m_enterColor;

        #endregion

        #region メソッド

        [Conditional("UNITY_EDITOR")]
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                m_playerController.AllowJump = true;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="playerController"> プレイヤー </param>
        public void Initialize(BasePlayerController playerController)
        {
            m_playerController = playerController;
            this.SetComponent(ref m_buttonImage);

            m_initColor = m_buttonImage.color;
            m_enterColor = m_initColor * Color.gray;
        }

        /// <summary>
        /// ジャンプボタンクリック時
        /// </summary>
        public void OnClickJumpButton()
        {
            m_playerController.AllowJump = true;
            m_buttonImage.color = m_enterColor;
        }

        /// <summary>
        /// ボタンを離したとき
        /// </summary>
        public void PointerUp()
        {
            m_buttonImage.color = m_initColor;
        }

        #endregion
    }
}