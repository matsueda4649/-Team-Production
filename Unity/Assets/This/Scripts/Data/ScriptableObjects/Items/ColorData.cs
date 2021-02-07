using UnityEngine;

namespace ItemSystem
{
    /// <summary>
    /// 基本の弾の処理
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Item/Color/Speed", fileName = "NewLerpSpeed")]
    public class ColorData : ScriptableObject
    {
        #region 変数宣言

        [SerializeField] float m_lerpSpeed = 1f;

        /// <summary>
        /// ラープ速度
        /// </summary>
        public float GetLerpSpeed { get => m_lerpSpeed; }

        #endregion
    }
}