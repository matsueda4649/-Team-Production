using ControllerSystem;
using ManagerSystem;
using UnityEngine;

namespace ItemSystem
{
    /// <summary>
    /// 通常の弾
    /// </summary>
    public class NormalBullet : MonoBehaviour, IUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] BaseBulletData m_data = default;

        /// <summary>
        /// データの取得
        /// </summary>
        public BaseBulletData GetData { get => m_data; }

        /// <summary>
        /// 経過時間
        /// </summary>
        public float ElapsedTime { get; private set; }

        /// <summary>
        /// 移動方向
        /// </summary>
        public Vector2 MoveDirection{ get; set; }

        [SerializeField] SpriteRenderer m_renderer = default;

        public SpriteRenderer GetSpriteRenderer { get => m_renderer; }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize(){}

        public void UpdateMe()
        {
            // 移動処理
            MovementBullet();

            if ((ElapsedTime += Time.deltaTime) > GetData.GetLimitTime)
            {
                ElapsedTime = 0f;
                this.gameObject.Deactovation();
            }
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        protected virtual void MovementBullet()
        {
            // 移動ベクトル
            Vector2 moveVector = MoveDirection * GetData.GetMoveSpeed;

            this.transform.Translate(moveVector * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var playerController = collision.gameObject.GetComponent<BasePlayerController>();
            if(!playerController.IsNull()) { TriggerWithPlayer(playerController); }
        }

        protected virtual void TriggerWithPlayer(BasePlayerController playerController)
        {
            playerController.DeathMotion();
            this.gameObject.Deactovation();
        }

        public virtual void OnEnable()
        {
            UpdateManager.AddUpdatable(this);
        }

        public virtual void OnDisable()
        {
            UpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}