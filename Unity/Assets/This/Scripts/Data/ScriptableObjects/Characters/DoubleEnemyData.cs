using UnityEngine;

/// <summary>
/// 倍になる敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/DoubleEnemy", fileName = "NewDoubleEnemy")]
public class DoubleEnemyData : AcceleratingEnemyData
{
    public enum Type 
    {
        /// <summary>
        /// サイズ
        /// </summary>
        Size,
        /// <summary>
        /// 速度
        /// </summary>
        Speed,
        /// <summary>
        /// 数
        /// </summary>
        Number,
    }

    #region 変数宣言

    [SerializeField] Type m_type = Type.Speed;

    [SerializeField] float m_magnification = 2f;

    /// <summary>
    /// タイプの取得
    /// </summary>
    public Type GetTpe { get => m_type; }

    /// <summary>
    /// 倍率を取得
    /// </summary>
    public float GetMagnification { get => m_magnification; }

    #endregion
}
