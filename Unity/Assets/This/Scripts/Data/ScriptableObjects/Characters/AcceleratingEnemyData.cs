using UnityEngine;

/// <summary>
/// 加速する敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/AcceleratingEnemy", fileName = "NewAcceleratingEnemy")]
public class AcceleratingEnemyData : MovingEnemyData
{
    #region 変数宣言

    [SerializeField] float m_moveForceMultiplier = 5f;

    [SerializeField] float m_rightAcceleration = 1f;

    [SerializeField] float m_leftAcceleration = 1f;

    /// <summary>
    /// 移動速度入力に対する追従速度
    /// </summary>
    public float GetMoveForceMultiplier { get => m_moveForceMultiplier; }

    /// <summary>
    /// 移動方向
    /// </summary>
    /// <param name="rightDirection">右に移動するかどうか</param>
    /// <returns>移動方向</returns>
    public Vector2 MoveDirection(bool rightDirection)
    {
        // 加速度の計算
        float velocity = rightDirection ? m_rightAcceleration : m_leftAcceleration;

        // 移動方向
        Vector2 direction = rightDirection ? Vector2.right : Vector2.left;

        return direction * velocity;
    }

    #endregion
}
