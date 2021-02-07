using UnityEngine;

/// <summary>
/// シューティングエネミーのデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy/ShootingEnemy", fileName = "NewShootingEnemy")]
public class ShootingEnemyData : ScriptableObject
{
    #region 変数宣言

    /// <summary>
    /// 弾を打つ方向
    /// </summary>
    [SerializeField] Vector2[] m_shootingDirections = new Vector2[] { Vector2.right };

    /// <summary>
    /// 弾を打つ方向を取得
    /// </summary>
    public Vector2[] GetShootingDirections { get => m_shootingDirections; }


    #endregion
}
