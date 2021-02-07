using ControllerSystem;
using UnityEngine;
using static ExtensionMathf;

namespace CharacterSystem 
{
    public class BounceEnemy : BaseEnemyController
    {
        #region 変数宣言

        [SerializeField] PhysicsMaterial2D m_physicsMaterial = default;

        /// <summary>
        /// 跳ねる速度
        /// </summary>
        private float m_bounceVelocity;

        private Rigidbody2D m_rb = null;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_rb);

            m_bounceVelocity = CalculateFallingSpeed(this.transform.localPosition);
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            BounceMotion();
        }

        /// <summary>
        /// 跳ねるモーション
        /// </summary>
        private void BounceMotion()
        {
            m_rb.velocity = Vector2.zero;

            // 跳ねる前の落下速度
            var before = m_bounceVelocity;
            // 反発係数
            var elasticity = m_physicsMaterial.bounciness;

            m_bounceVelocity = Mathf.Abs(CalculateBounceVelocity(before, elasticity));
            m_rb.AddForce(Vector3.up * m_bounceVelocity, ForceMode2D.Impulse);
        }

        #endregion
    }
}