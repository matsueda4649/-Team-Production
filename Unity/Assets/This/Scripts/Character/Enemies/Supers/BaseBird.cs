using ControllerSystem;
using ManagerSystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// Birdのベース
    /// </summary>
    public abstract class BaseBird : MonoBehaviour, IFixedUpdatable
    {
        #region 変数宣言

        [SerializeField] MovingEnemyData m_data = default;

        /// <summary>
        /// Dataを取得
        /// </summary>
        public MovingEnemyData GetData
        {
            get => m_data;
        }

        private Rigidbody2D m_rb;

        /// <summary>
        /// Rigidbody2Dを取得
        /// </summary>
        public Rigidbody2D GetRb
        {
            get => m_rb;
        }

        private SpriteRenderer m_renderer;

        /// <summary>
        /// SpriteRendererを取得
        /// </summary>
        public SpriteRenderer GetRenderer
        {
            get => m_renderer;
        }

        private BasePlayerController m_playerController;

        /// <summary>
        /// プレイヤーを取得
        /// </summary>
        public BasePlayerController PlayerController { get => m_playerController; }

        /// <summary>
        /// 非表示を可能かどうか
        /// </summary>
        private bool m_allowHiding = false;

        #endregion

        #region メソッド

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected virtual void Initialize()
        {
            this.SetComponent(ref m_rb);
            this.SetComponent(ref m_renderer);
        }

        public void FixedUpdateMe()
        {
            MoveMotion();
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        protected abstract void MoveMotion();

        /// <summary>
        /// Playerに追加ベクトルを与える
        /// </summary>
        /// <param name="playerController">プレイヤー</param>
        /// <param name="distance">移動距離</param>
        protected static void SetVelocityOfPlayer(BasePlayerController playerController, Vector2 distance)
        {
            if (!playerController.IsNull())
            {
                // キー入力を取得
                var input = Mathf.Abs(playerController.InputDirection);

                // ベクトルの計算
                var x = playerController.GetRb2D.velocity.x * input;
                var y = playerController.GetRb2D.velocity.y;

                // 速度の計算
                var speedX = distance.x / Time.deltaTime;
                x = input == 0 ? x + speedX : x;

                playerController.GetRb2D.velocity = new Vector2(x, y);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            collision.SetComponent(ref m_playerController);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (!m_playerController.IsNull())
            {
                m_playerController.GetRb2D.velocity = Vector2.zero;
                CollisionWithPlayer(m_playerController);
            }
        }

        /// <summary>
        /// Playerとの衝突処理
        /// </summary>
        /// <param name="playerController"></param>
        protected abstract void CollisionWithPlayer(BasePlayerController playerController);

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            m_playerController = null;
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            m_playerController = null;
        }

        public void OnEnable()
        {
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            FixedUpdateManager.RemoveUpdatable(this);
        }

        private void OnBecameVisible()
        {
            m_allowHiding = true;
        }

        private void OnBecameInvisible()
        {
            this.SetActive(!m_allowHiding);
        }

        #endregion
    }
}