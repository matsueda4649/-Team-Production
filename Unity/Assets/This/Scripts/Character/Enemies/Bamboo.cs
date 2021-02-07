using ControllerSystem;
using ManagerSystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// 竹
    /// </summary>
    public class Bamboo : BaseEnemyController
    {
        #region 変数宣言

        private Animator m_animator;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_animator);
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);
            m_animator.SetBool("Attack", true);
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            // アニメーションの再生中なら
            if (!m_animator.IsEnd(0))
            {
                AudioManager.Instance.PlayVoice(AudioVoice.Stab);
                playerController.DeathMotion();
            }
        }

        #endregion
    }
}