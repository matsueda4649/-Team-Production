using ItemSystem;
using UnityEngine;

/// <summary>
/// 弓矢を飛ばす敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/ThrowShotEnemy", fileName = "NewThrowShotEnemy")]
public class ThrowShotEnemyData : ThrowEnemyData
{
    [SerializeField, Range(1, 10)] 
    private int m_shotDistance = 5;

    /// <summary>
    /// 飛距離
    /// </summary>
    public int GetShotDistance { get => m_shotDistance; }
}
