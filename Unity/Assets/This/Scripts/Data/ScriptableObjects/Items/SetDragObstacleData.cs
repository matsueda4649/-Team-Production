using UnityEngine;

namespace ItemSystem
{
    /// <summary>
    /// SetDragObstacleのデータ
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Item/Obstacles/SetDragObstacle", fileName = "SetDragObstacleData")]
    public class SetDragObstacleData : ScriptableObject
    {
        #region 変数宣言

        /// <summary>
        /// 抗力係数
        /// </summary>
        [SerializeField] float m_dragCoe = 0f;

        /// <summary>
        /// 密度
        /// </summary>
        [SerializeField] float density = 0f;

        /// <summary>
        /// 抗力係数の取得
        /// </summary>
        public float GetDragCoe { get => m_dragCoe; }

        /// <summary>
        /// 密度の取得
        /// </summary>
        public float GetDensity { get => density; }

        #endregion
    }
}