using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionMathf;

namespace CharacterSystem
{
    /// <summary>
    /// 飛んでくる敵
    /// </summary>
    [RequireComponent(typeof( BoxCollider2D ), typeof(Rigidbody2D))]
    public class ThrowEnemyController : BaseEnemyController
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] ThrowEnemyData m_data = default; 

        /// <summary>
        /// 攻撃許可
        /// </summary>
        private bool m_allowShot = false;

        private Rigidbody2D m_rb = null;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Initialize()
        {
            this.SetComponent(ref m_rb);
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);
            AttackMotion(playerController.transform);
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
            rb.AddForce(velocity * rb.mass, ForceMode2D.Impulse);

            AudioManager.Instance.PlayVoice(AudioVoice.Asterisk);
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);
            playerController.DeathMotion();
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            base.CollideWithFlag(flagController);

            // 地面に触れたら攻撃可能
            if (flagController.GetData.OnGround) { m_allowShot = true; }
        }

        #endregion
    }
}