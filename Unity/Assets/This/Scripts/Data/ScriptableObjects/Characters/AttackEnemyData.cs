using UnityEngine;

/// <summary>
/// 攻撃する敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/AttackEnemy", fileName = "NewAttackEnemy")]
public class AttackEnemyData : ScriptableObject
{
    [SerializeField] float m_attackTime = 1.5f;

    [SerializeField] float m_recoveryTime = 2.5f;

    [SerializeField] float m_stanTime = 2f;

    /// <summary>
    /// 攻撃のインターバル
    /// </summary>
    public float GetAttackTime { get => m_attackTime; }

    /// <summary>
    /// スタン回復時間
    /// </summary>
    public float GetRecoveryTime { get => m_recoveryTime; }

    /// <summary>
    /// スタン時間
    /// </summary>
    public float GetStanTime { get => m_stanTime; }
}
