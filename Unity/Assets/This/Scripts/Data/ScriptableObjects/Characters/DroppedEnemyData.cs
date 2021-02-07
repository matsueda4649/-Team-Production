using UnityEngine;

/// <summary>
/// 落下する敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/DropEnemyData", fileName = "NewDropCharacter")]
public class DroppedEnemyData : RayCastData
{
    [SerializeField] float m_gravity = 1f;

    [SerializeField] float m_interval = 10f;

    /// <summary>
    /// 重力
    /// </summary>
    public float GetGravity { get => m_gravity; }

    /// <summary>
    /// 初期位置に戻るまでの時間
    /// </summary>
    public float GetInterval { get => m_interval; }
}
