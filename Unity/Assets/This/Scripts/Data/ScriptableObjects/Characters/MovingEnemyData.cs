using UnityEngine;

/// <summary>
/// 移動する敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/MovingEnemy", fileName = "NewMovingEnemy")]
public class MovingEnemyData : ScriptableObject
{
    #region 変数宣言

    [SerializeField] float m_moveSpeed = 100f;

    [SerializeField] bool m_allowAction = false;

    /// <summary>
    /// 移動速度
    /// </summary>
    public float GetMoveSpeed { get => m_moveSpeed; }

    /// <summary>
    /// 自由にアクション出来るかどうか
    /// </summary>
    public bool GetAllowAction { get => m_allowAction; }

    #endregion
}
