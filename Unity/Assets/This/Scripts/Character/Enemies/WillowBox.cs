using ControllerSystem;
using ItemSystem;
using UnityEngine;
using static ExtensionComponent;

namespace CharacterSystem 
{
    /// <summary>
    /// 豆まきをする敵
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class WillowBox : BaseEnemyController
    {
        #region 変数宣言

        /// <summary>
        /// パーティクル
        /// </summary>
        [SerializeField] ParticleCollider m_particle = default;

        private BasePlayerController m_playerController;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            var parent = this.transform.GetChild(0);
            FindTagComponent(ref m_playerController, "PlayerController");
            m_particle.Initialize(m_playerController.DeathMotion);
        }

        /// <summary>
        /// 豆まきをする
        /// </summary>
        private void SowingBeans()
        {
            m_particle.Activation();
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);
            SowingBeans();
        }

        #endregion
    }
}