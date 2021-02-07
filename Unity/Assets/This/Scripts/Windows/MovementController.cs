using UnityEngine;
using CharacterSystem;
using ManagerSystem;
using ControllerSystem;
using UnityEngine.UI;

namespace WindowSystem
{
    /// <summary>
    /// 移動コントローラー
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// ボタン
        /// </summary>
        [SerializeField] Image[] m_buttonImage = new Image[2];

        /// <summary>
        /// プレイヤーコントローラー
        /// </summary>
        private BasePlayerController m_playerController;

        /// <summary>
        /// 移動方向
        /// </summary>
        private Vector2 m_direction = Vector2.zero;

        /// <summary>
        /// フェーズ
        /// </summary>
        private Phase m_phase = Phase.Init;

        /// <summary>
        /// クリックしているボタン
        /// </summary>
        private Image m_clickButton = null;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="playerController"> キャラクター </param>
        public void Initialize(BasePlayerController playerController)
        {
            m_playerController = playerController;
        }

        /// <summary>
        /// フェーズの更新
        /// </summary>
        public void UpdatePhase()
        {
            switch(m_phase)
            {
                // ゲームプレイ中はコントローラーを使う
                case Phase.Play:
                    InputDirection();
                    break;

                // 入力を無しにする
                default:
                    m_playerController.InputDirection = 0f;
                    break;
            }
        }

        /// <summary>
        /// 移動方向の入力
        /// </summary>
        private void InputDirection()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            m_direction.x = InputGetKey();
#endif
            m_playerController.InputDirection = Mathf.Clamp(m_direction.x, -1, 1);
        }

        /// <summary>
        /// キーボード入力を取得
        /// </summary>
        /// <returns></returns>
        private static float InputGetKey()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                return -1f;
            }
            else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                return 1f;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 左入力
        /// </summary>
        public void InputLeftDirection()
        {
            // 左方向に設定
            m_direction = Vector2.left;
            m_buttonImage[0].color = Color.gray;
            m_clickButton = m_buttonImage[0];
        }

        /// <summary>
        /// 右入力
        /// </summary>
        public void InputRightDirection()
        {
            // 右方向に設定
            m_direction = Vector2.right;
            m_buttonImage[1].color = Color.gray;
            m_clickButton = m_buttonImage[1];
        }

        /// <summary>
        /// 前に押していたボタンから離れたとき
        /// </summary>
        public void PointerExit()
        {
            m_direction = Vector2.zero;
            m_clickButton.color = Color.white;
        }

        /// <summary>
        /// フェーズの更新
        /// </summary>
        /// <param name="phase">設定するフェーズ</param>
        public void UpdatePhase(Phase phase)
        {
            m_phase = phase;
        }

#endregion
    }
}