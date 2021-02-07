using UnityEngine;

/// <summary>
/// DOTweenを使用して移動するキャラクターのデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/DOtweenMovingEnemy", fileName = "NewDOtweenMovingEnemy")]
public class DOtweenMovingEnemyData : ScriptableObject
{
    #region 変数宣言

    /// <summary>
    /// 移動方向
    /// </summary>
    [SerializeField] Direction m_direction = Direction.Right;

    /// <summary>
    /// 移動量
    /// </summary>
    [SerializeField, Range(1, 30)]
    private float m_moveLength = 1;

    /// <summary>
    /// 移動方向を取得
    /// </summary>
    public Direction GetMoveDirection { get => m_direction; }

    /// <summary>
    /// 移動量を取得
    /// </summary>
    public float GetMoveLength { get => m_moveLength; }

    #endregion
}

