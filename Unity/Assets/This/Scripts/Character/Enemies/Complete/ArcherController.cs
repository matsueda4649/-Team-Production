using ControllerSystem;
using ItemSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionMathf;

namespace CharacterSystem
{
    /// <summary>
    /// 弓を打つ敵
    /// </summary>
    public class ArcherController : BaseEnemyController, IUpdatable, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// Angleデータ
        /// </summary>
        [SerializeField] ThrowShotEnemyData m_angleData = default;

        /// <summary>
        /// Timeデータ
        /// </summary>
        [SerializeField] AttackEnemyData m_timeData = default;

        /// <summary>
        /// 攻撃アイテム 弓
        /// </summary>
        [SerializeField] ArcArrow m_arcArrow = default;

        /// <summary>
        /// 経過時間
        /// </summary>
        private float m_elapsedTime = 0f;

        /// <summary>
        /// 攻撃開始フラグ
        /// </summary>
        private bool m_start = true;

        /// <summary>
        /// 攻撃開始場所
        /// </summary>
        private Vector2 m_startPos;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            m_arcArrow.Initialize();
            m_arcArrow.OnHit = PrepareForAttack;
            m_startPos = this.transform.localPosition;
        }

        public void UpdateMe()
        {
            m_arcArrow.SetRotation();

            if (!GetSpriteRender.isVisible) { return; }

            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime > m_timeData.GetAttackTime && m_start)
            {
                m_start = false;
                ShotMotion();
            }
        }

        public void FixedUpdateMe()
        {
            m_arcArrow.FixedUpdateMe();
        }

        /// <summary>
        /// 攻撃の準備をする
        /// </summary>
        private void PrepareForAttack()
        {
            m_start = true;
            m_elapsedTime = 0f;
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void ShotMotion()
        {
            AudioManager.Instance.PlayVoice(AudioVoice.Arrow);

            var target = new Vector3(m_startPos.x + m_angleData.GetShotDistance, m_startPos.y);

            var velocity = CalculateThrowVelocity(m_startPos, target, m_angleData.GetThowingAngle);
            m_arcArrow.AddForce(velocity);
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);
            DeathMotion(playerController);
        }

        public void OnEnable()
        {
            UpdateManager.AddUpdatable(this);
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            UpdateManager.RemoveUpdatable(this);
            FixedUpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}