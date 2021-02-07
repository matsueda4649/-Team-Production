using ControllerSystem;
using ItemSystem;
using ManagerSystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// おもちゃの兵隊
    /// </summary>
    public class ToySoldier : BaseEnemyController, IFixedUpdatable
    {
        #region 変数名

        /// <summary>
        /// 移動データ
        /// </summary>
        [SerializeField] AcceleratingEnemyData m_movingData = default;

        /// <summary>
        /// 攻撃データ
        /// </summary>
        [SerializeField] AttackEnemyData m_attackData = default;

        /// <summary>
        /// 右に移動するかどうか
        /// </summary>
        private bool m_rightDirection = true;

        /// <summary>
        /// 弾の管理
        /// </summary>
        private BulletManagement m_bullets;

        /// <summary>
        /// プレイヤー
        /// </summary>
        private BasePlayerController m_playerController;

        /// <summary>
        /// 攻撃許可
        /// </summary>
        private bool m_allowAttack = false;

        /// <summary>
        /// 攻撃経過時間
        /// </summary>
        private float m_attackInterval = 0f;

        private Rigidbody2D m_rb;

        private Animator m_animator;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_bullets);
            this.SetComponent(ref m_animator);
            this.SetComponent(ref m_rb);
            m_bullets.Initialize();
            GetSpriteRender.flipX = m_rightDirection;
        }

        public void FixedUpdateMe()
        {
            if (GetSpriteRender.isVisible)
            {
                AttackMotion();

                MoveMotion();
            }
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion()
        {
            m_attackInterval += Time.deltaTime;

            // 攻撃可能
            if (m_allowAttack)
            {
                // 経過時間を超えたら
                if(m_attackInterval > m_attackData.GetAttackTime)
                {
                    // 座標
                    var start = this.transform.localPosition;
                    var target = m_playerController.transform.localPosition;

                    EnemyAttack.InitializeBullet(m_bullets.GetBullet(), start, target, true);

                    AudioManager.Instance.PlayVoice(AudioVoice.ToyGun);

                    m_attackInterval = 0f;
                }
            }
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            EnemyMovement.AddConstantForce(
                m_rb,
                moveSpeed: m_movingData.GetMoveSpeed,
                moveDirection: m_movingData.MoveDirection(m_rightDirection),
                moveForceMultiplier: m_movingData.GetMoveForceMultiplier);
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            base.CollideWithFlag(flagController);

            var flag = flagController.GetData;
            if(flag.OnGround || flag.OnWall)
            {
                // 移動方向変更
                m_rightDirection = !m_rightDirection;
                GetSpriteRender.flipX = m_rightDirection;
            }
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            DeathMotion(playerController);
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);
            m_allowAttack = true;
            m_playerController = playerController;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var playerController = collision.GetComponent<BasePlayerController>();
            if (!playerController.IsNull())
            {
                m_allowAttack = false;
                m_playerController = null;
            }
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