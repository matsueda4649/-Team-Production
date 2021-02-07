using ControllerSystem;
using UnityEngine;
using static ExtensionMathf;

namespace ItemSystem
{
    /// <summary>
    /// 抵抗力を設定する障害物
    /// </summary>
    public class SetDragObstacle : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] SetDragObstacleData m_data = default;

        /// <summary>
        /// 初期抵抗
        /// </summary>
        private float m_initDrag = 0f;

        /// <summary>
        /// 自身のサイズ
        /// </summary>
        private Vector2 m_selfSize = Vector2.zero;

        /// <summary>
        /// 設定抵抗
        /// </summary>
        private float m_settingDrag = 0f;

        /// <summary>
        /// Playerのコントローラー
        /// </summary>
        private BasePlayerController m_playerController = null;

        #endregion

        #region メソッド

        private void Awake()
        {
            m_selfSize = this.GetComponent<SpriteRenderer>().size;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_playerController.IsNull())
            {
                collision.SetComponent(ref m_playerController);
                SetPlayerDrag();
            }
        }

        /// <summary>
        /// 抵抗力の設定
        /// </summary>
        private void SetPlayerDrag()
        {
            if (!m_playerController.IsNull())
            {
                m_initDrag = m_playerController.GetRb2D.drag;

                var gravityScale = m_playerController.GetRb2D.gravityScale;
                var velocity = m_playerController.GetRb2D.velocity;
                m_settingDrag = CalculateDrag(m_data, velocity, m_selfSize) * gravityScale;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            // コライダーに触れいている間
            if (!m_playerController.IsNull())
            {
                // 抵抗力を設定する
                m_playerController.GetRb2D.drag = m_settingDrag;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // コライダーから離れたとき
            if (!m_playerController.IsNull())
            {
                // 初期抵抗力に戻す
                m_playerController.GetRb2D.drag = m_initDrag;
            }
        }

        #endregion
    }
}