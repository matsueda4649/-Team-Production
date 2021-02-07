using ControllerSystem;
using ItemSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionComponent;
using static ExtensionMathf;
using static UnityEngine.Mathf;

namespace CharacterSystem
{
    /// <summary>
    /// 彦星
    /// </summary>
    public class Hikoboshi : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// Playerに狙いを定めるかどうか
        /// </summary>
        [SerializeField] bool m_lockOn = true;

        /// <summary>
        /// 移動関係のデータ
        /// </summary>
        [SerializeField] AcceleratingEnemyData m_movingData = default;

        /// <summary>
        /// 攻撃データ
        /// </summary>
        [SerializeField] AttackEnemyData m_attackData = default;

        /// <summary>
        /// プレイヤー
        /// </summary>
        private BasePlayerController m_playerController;

        /// <summary>
        /// PlayerのTransform
        /// </summary>
        private Transform m_playerTransform
        {
            get => m_playerController.transform;
        }

        /// <summary>
        /// 攻撃経過時間
        /// </summary>
        private float m_attackInterval = 0f;

        /// <summary>
        /// 移動方向
        /// </summary>
        private Vector2 GetDirection
        {
            get
            {
                var self   = this.transform.localPosition;
                var target = m_playerTransform.localPosition;

                if (target.x > self.x)
                {
                    return Vector2.right;
                }

                return Vector2.left;
            }
        }

        private Rigidbody2D m_rb = null;

        private BulletManagement m_bullets;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            // プレイヤーを取得
            FindTagComponent(ref m_playerController, "PlayerController");

            this.SetComponent(ref m_rb);
            this.SetComponent(ref m_bullets);
            m_bullets.Initialize();
        }

        public void FixedUpdateMe()
        {
            if (GetSpriteRender.isVisible)
            {
                AttackMotion();
            }

            MoveMotion();
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion()
        {
            m_attackInterval += Time.deltaTime;

            // 経過時間を超えたら
            if (m_attackInterval > m_attackData.GetAttackTime)
            {
                // 座標
                var start  = this.transform.localPosition;
                var target = m_playerTransform.localPosition;

                EnemyAttack.InitializeBullet(m_bullets.GetBullet(), start, target, m_lockOn);

                AudioManager.Instance.PlayVoice(AudioVoice.DarkMasic);

                m_attackInterval = 0f;
            }
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            EnemyMovement.AddConstantForce(m_rb,
                                           m_movingData.GetMoveSpeed,
                                           GetDirection,
                                           m_movingData.GetMoveForceMultiplier);
        }

        public void OnEnable()
        {
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            m_attackInterval = 0f;
            FixedUpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}