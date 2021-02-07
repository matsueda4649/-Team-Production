using ControllerSystem;
using ManagerSystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// 一定方向を向くと早くなる敵
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class AcceleratingEnemyController : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// 敵のデータ
        /// </summary>
        [SerializeField] AcceleratingEnemyData m_data = default;

        /// <summary>
        /// 右方向に移動する
        /// </summary>
        [SerializeField] bool m_rightDirection = false;

        private Rigidbody2D m_rb = null;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期設定
        /// </summary>
        protected override void Initialize()
        {
            this.SetComponent(ref m_rb);
        }

        public void FixedUpdateMe()
        {
            // 画面に見えている || 自由に行動できる
            if (GetSpriteRender.isVisible || m_data.GetAllowAction)
            {
                MoveMotion();
            }
        }

        /// <summary>
        /// 移動方向を反転する
        /// </summary>
        private void ReverseDirection()
        {
            m_rb.velocity = Vector2.zero;

            AudioManager.Instance.PlayVoice(AudioVoice.Than);

            // 移動方向変更
            m_rightDirection = !m_rightDirection;
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            EnemyMovement.AddConstantForce(
                    m_rb,
                    moveSpeed: m_data.GetMoveSpeed,
                    moveDirection: m_data.MoveDirection(m_rightDirection),
                    moveForceMultiplier: m_data.GetMoveForceMultiplier);
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            var enemyController = collision.GetComponent<BaseEnemyController>();
            if (!enemyController.IsNull()) { ReverseDirection(); }
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            DeathMotion(playerController);
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            base.CollideWithFlag(flagController);

            // 壁に触れたら
            if (flagController.GetData.OnWall) { ReverseDirection(); }
        }

        public void OnEnable()
        {
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            FixedUpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}