using ItemSystem;
using UnityEngine;

namespace ControllerSystem 
{
    /// <summary>
    /// フラグ管理
    /// </summary>
    public class FlagController : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// フラグデータ
        /// </summary>
        [SerializeField] BaseFlagData m_data = default;

        /// <summary>
        /// フラグデータ
        /// </summary>
        public BaseFlagData GetData { get => m_data; }

        #endregion
    }
}