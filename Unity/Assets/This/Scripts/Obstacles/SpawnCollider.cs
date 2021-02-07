using ControllerSystem;
using UnityEngine;

namespace ItemSystem 
{
    /// <summary>
    /// オブジェクトをスポーンさせる
    /// </summary>
    public class SpawnCollider : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// スポ―ン対象
        /// </summary>
        [SerializeField] GameObject m_spawnTarget = default;

        /// <summary>
        /// 非表示にするかどうか
        /// </summary>
        [SerializeField] bool m_hide = true;

        /// <summary>
        /// Player落下時も判定するかどうか
        /// </summary>
        [SerializeField] bool m_isDown = true;

        /// <summary>
        /// Trigger
        /// </summary>
        [SerializeField] Collider2D m_spawnTrigger = default;

        private SpriteRenderer m_renderer;

        #endregion

        #region メソッド

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            this.SetComponent(ref m_renderer);

            if (m_spawnTarget.IsNull()) { Destroy(this); return; }

            m_spawnTarget.Deactovation();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var playerController = collision.GetComponent<BasePlayerController>();
            if (!playerController.IsNull())
            {
                if (m_isDown || !playerController.GetRb2D.IsDown(false))
                {
                    m_renderer.color = Color.gray;
                    m_spawnTarget.Activation();

                    this.gameObject.SetActive(!m_hide);
                    m_spawnTrigger.enabled = false;
                }
            }
        }

        #endregion
    }
}