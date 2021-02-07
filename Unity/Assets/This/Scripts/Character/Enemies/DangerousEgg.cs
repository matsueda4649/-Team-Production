using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionMathf;

namespace CharacterSystem 
{
    /// <summary>
    /// デンジャラスエッグ
    /// </summary>
    public class DangerousEgg : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        [SerializeField] PhysicsMaterial2D m_physicsMaterial = default;

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] ThrowEnemyData m_data = default;

        /// <summary>
        /// 跳ねる速度
        /// </summary>
        private float m_bounceVelocity;

        /// <summary>
        /// 初期落下速度
        /// </summary>
        private float m_initFallingSpeed;

        /// <summary>
        /// 攻撃許可
        /// </summary>
        private bool m_allowShot = false;

        private Animator m_animator;

        private Rigidbody2D m_rb;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_animator);
            this.SetComponent(ref m_rb);

            m_initFallingSpeed = CalculateFallingSpeed(this.transform.localPosition);
            m_bounceVelocity = m_initFallingSpeed;
        }

        public void FixedUpdateMe()
        {
            var isPlay = m_rb.velocity.x != 0f;
            m_animator.SetBool("Attack", isPlay);
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            BounceMotion(m_rb, ref m_bounceVelocity, m_physicsMaterial.bounciness);
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);
            playerController.DeathMotion();
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            base.CollideWithFlag(flagController);

            m_allowShot = flagController.GetData.OnGround;
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);

            // 攻撃
            AttackMotion(playerController.transform);

            // バウンドベクトルを初期化する
            m_bounceVelocity = m_initFallingSpeed;
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion(Transform playerController)
        {
            // 攻撃可能なら
            if (m_allowShot)
            {
                // 目標座標
                var start = this.transform.localPosition;
                var target = playerController.localPosition;

                AudioManager.Instance.PlayVoice(AudioVoice.Asterisk);

                // キャラクターの向きの設定
                GetSpriteRender.flipX = start.x < target.x; 

                // 射出するベクトル
                Vector2 velocity = CalculateThrowVelocity(start, target, m_data.GetThowingAngle);

                ShotMotion(m_rb, velocity);

                m_allowShot = false;
            }
        }

        /// <summary>
        /// 飛ばす
        /// </summary>
        /// <param name="velocity">移動ベクトル</param>
        private static void ShotMotion(Rigidbody2D rb, Vector2 velocity)
        {
            rb.velocity = Vector2.zero;

            rb.AddForce(velocity * rb.mass, ForceMode2D.Impulse);
            AudioManager.Instance.PlayVoice(AudioVoice.Asterisk);
        }

        /// <summary>
        /// 跳ねる
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="bounceVelocity">跳ねるときに加わるVelocity</param>
        /// <param name="elasticity">反発係数</param>
        private static void BounceMotion(Rigidbody2D rd, ref float bounceVelocity, float elasticity)
        {
            rd.velocity = new Vector2(rd.velocity.x, 0f);

            bounceVelocity = Mathf.Abs(CalculateBounceVelocity(bounceVelocity, elasticity));
            rd.AddForce(Vector3.up * bounceVelocity * rd.mass, ForceMode2D.Impulse);
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