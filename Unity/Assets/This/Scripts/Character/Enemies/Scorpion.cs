using ControllerSystem;
using ItemSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionComponent;

namespace CharacterSystem
{
    public class Scorpion : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// 移動データ
        /// </summary>
        [SerializeField] AcceleratingEnemyData m_movingData = default;

        /// <summary>
        /// 攻撃データ
        /// </summary>
        [SerializeField] AttackEnemyData m_attackData = default;

        /// <summary>
        /// Bullet発射開始地点
        /// </summary>
        [SerializeField] Transform m_launchingPoint = default;

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
        /// 経過時間
        /// </summary>
        private float m_elapsedTime = 0f;

        private Rigidbody2D m_rb;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_bullets);
            this.SetComponent(ref m_rb);
            FindTagComponent(ref m_playerController, "PlayerController");
            m_bullets.Initialize();

            var angle = m_rightDirection ? 180f : 0f;
            this.transform.SetLocalRotation(angle, Vector3.up);
        }

        public void FixedUpdateMe()
        {
            if (GetSpriteRender.isVisible)
            {
                MoveMotion();

                AttackMotion();
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

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion()
        {
            m_elapsedTime += Time.deltaTime;

            if(m_elapsedTime > m_attackData.GetAttackTime)
            {
                // 座標
                var start = m_launchingPoint.localPosition + this.transform.localPosition;
                var target = m_playerController.transform.localPosition;

                EnemyAttack.InitializeBullet(m_bullets.GetBullet(), start, target, true);

                AudioManager.Instance.PlayVoice(AudioVoice.Arrow);

                m_elapsedTime = 0f;
            }
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            base.CollideWithFlag(flagController);

            if (flagController.GetData.OnWall)
            {
                // 移動方向変更
                m_rightDirection = !m_rightDirection;

                var angle = m_rightDirection ? 180f : 0f;
                this.transform.SetLocalRotation(angle, Vector3.up);
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