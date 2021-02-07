using ManagerSystem;
using UnityEngine;

namespace ItemSystem
{
    /// <summary>
    /// 救済ブロック
    /// </summary>
    public class ReliefBlock : MonoBehaviour
    {
        #region 変数宣言

        [SerializeField] GameObject m_blocks = default;

        #endregion

        #region メソッド

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            if (!m_blocks.IsNull())
            {
                var saveData = SaveManager.Instance.GetSaveData;
                m_blocks.SetActive(saveData.easyMode);
            }
        }

        #endregion
    }
}