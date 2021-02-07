using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionComponent;
using ItemSystem;

namespace CharacterSystem
{
    /// <summary>
    /// 弾を飛ばす敵
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ShootingEnemyController : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] AttackEnemyData m_data = default;

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

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_bullets);
            m_bullets.Initialize();

            FindTagComponent(ref m_playerController, "PlayerController");
        }

        public void FixedUpdateMe()
        {
            AttackMotion();
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion()
        {
            // 時間計測
            m_elapsedTime += Time.deltaTime;

            // 攻撃可能時間になったら
            if (m_elapsedTime > m_data.GetAttackTime)
            {
                // 自身の座標
                var start = this.transform.localPosition;
                var target = m_playerController.transform.localPosition;

                EnemyAttack.InitializeBullet(m_bullets.GetBullet(), start, target, false);

                AudioManager.Instance.PlayVoice(AudioVoice.Hash);

                m_elapsedTime = 0f;
            }
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            DeathMotion(playerController);
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