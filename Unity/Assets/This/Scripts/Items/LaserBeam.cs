using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionMathf;

namespace ItemSystem 
{
    /// <summary>
    /// レーザー
    /// </summary>
    public class LaserBeam : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// Laser
        /// </summary>
        [SerializeField] ParticleCollider m_laser = default;

        /// <summary>
        /// レーザーの開始地点
        /// </summary>
        private Vector2 m_startPos;

        private BasePlayerController m_playerController;

        #endregion

        #region 変数宣言

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="playerController">プレーヤー</param>
        public void Initialize(BasePlayerController playerController)
        {
            m_playerController = playerController;
            m_laser.Initialize(OnHitLaser);

            // レーザーの開始地点の設定
            var thisPos = this.transform.localPosition;
            var parentPos = this.transform.parent.localPosition;
            m_startPos = thisPos + parentPos;
        }

        /// <summary>
        /// PlayerにLaserが当たったとき
        /// </summary>
        private void OnHitLaser()
        {
            AudioManager.Instance.PlayVoice(AudioVoice.Bomb);
            m_playerController.DeathMotion();
        }

        /// <summary>
        /// レーザーを放つ
        /// </summary>
        public void FireLasers()
        {
            var target = m_playerController.transform.localPosition;
            var angle = 180f - CalculateRadian(m_startPos, target);
            this.transform.SetLocalRotation(angle, Vector3.forward);
        }

        /// <summary>
        /// レーザーの表示管理
        /// </summary>
        public void SetActiveLaser(bool active)
        {
            m_laser.SetActive(active);
        }

        #endregion
    }
}