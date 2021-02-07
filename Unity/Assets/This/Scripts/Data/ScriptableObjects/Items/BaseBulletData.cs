using UnityEngine;

namespace ItemSystem
{
    /// <summary>
    /// 基本の弾の処理
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Item/Bullet/Base", fileName = "NewBullet")]
    public class BaseBulletData : ScriptableObject
    {
        #region 変数宣言

        [SerializeField] float m_moveSpeed = 10f;

        [SerializeField] float m_limitTime = 2f;

        /// <summary>
        /// 移動速度
        /// </summary>
        public float GetMoveSpeed { get => m_moveSpeed; }

        /// <summary>
        /// 消滅までの時間
        /// </summary>
        public float GetLimitTime { get => m_limitTime; }

        #endregion
    }
}