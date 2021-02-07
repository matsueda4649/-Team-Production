using UnityEngine;

namespace ItemSystem
{
    /// <summary>
    /// フラグの管理
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Item/Flag", fileName = "NewFlag")]
    public class BaseFlagData : ScriptableObject
    {
        #region 変数宣言

        [SerializeField] bool m_onGround = false;

        [SerializeField] bool m_forceGround = false;

        [SerializeField] bool m_onWall   = false;

        [SerializeField] bool m_onDead   = false;

        [SerializeField] bool m_onClear  = false;

        /// <summary>
        /// 地面かどうか
        /// </summary>
        public bool OnGround { get => m_onGround; }

        /// <summary>
        /// 強制的に地面判定させるかどうか
        /// </summary>
        public bool OnForceGround{ get => m_forceGround; }

        /// <summary>
        /// 壁かどうか
        /// </summary>
        public bool OnWall { get => m_onWall; }

        /// <summary>
        /// デットゾーン
        /// </summary>
        public bool OnDead   { get => m_onDead; }

        /// <summary>
        /// クリアゾーン
        /// </summary>
        public bool OnClear  { get => m_onClear; }

        #endregion
    }
}