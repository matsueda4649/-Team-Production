using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 落下する敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Item/RayCastData", fileName = "NewRayCast")]
public class RayCastData : ScriptableObject
{
    [SerializeField] LayerMask m_colliderMask = default;

    [SerializeField] Vector2 m_direction = Vector2.down;

    [SerializeField] float m_distance = 5f;

    /// <summary>
    /// 当たり判定を検出するレイヤー
    /// </summary>
    public LayerMask GetColliderMask { get => m_colliderMask; }

    /// <summary>
    /// Rayの飛ばす方向
    /// </summary>
    public Vector2 GetDirection { get => m_direction; }

    /// <summary>
    /// Rayの長さ
    /// </summary>
    public float GetDistance { get => m_distance; }
}
