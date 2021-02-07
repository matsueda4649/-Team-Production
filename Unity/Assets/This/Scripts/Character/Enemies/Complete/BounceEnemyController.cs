using ControllerSystem;
using ItemSystem;
using ManagerSystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// 跳ねる敵
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class BounceEnemyController : BounceEnemy
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] ShootingEnemyData m_data = default;

        /// <summary>
        /// 弾の管理
        /// </summary>
        private BulletManagement m_bullets;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            base.Initialize();

            this.SetComponent(ref m_bullets);

            m_bullets.Initialize();
        }

        protected override void CollideWithFlag(FlagController flagController)
        {
            base.CollideWithFlag(flagController);
            if (flagController.GetData.OnGround) 
            {
                if(GetSpriteRender.isVisible) 
                {
                    AudioManager.Instance.PlayVoice(AudioVoice.Drum);
                }

                ShootSoundWaves(m_data.GetShootingDirections, m_bullets, this.transform);
            }
        }

        /// <summary>
        /// SoundWaveを打つ
        /// </summary>
        /// <param name="directions">移動方向の配列</param>
        /// <param name="bullets">弾の管理</param>
        /// <param name="parent">自身の座標</param>
        private static void ShootSoundWaves(Vector2[] directions, BulletManagement bullets, Transform parent)
        {
            for (int i = 0, length = directions.Length; i < length; ++i)
            {
                // 弾を取得
                var bullet = bullets.GetBullet();
                bullet.transform.SetLocalPosition(parent.localPosition);
                bullet.MoveDirection = directions[i % length];
                bullet.Activation();
            }
        }

        #endregion
    }
}