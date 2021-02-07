using UnityEngine;

/// <summary>
/// 飛ぶ敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/ThrowEnemy", fileName = "NewThrowEnemy")]
public class ThrowEnemyData : ScriptableObject
{
    [SerializeField, Range(1f, 70f)]
    private float m_throwingAngle = 45f;

    /// <summary>
    /// 飛ばす角度を取得
    /// </summary>
    public float GetThowingAngle { get => m_throwingAngle; }
}