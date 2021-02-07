using UnityEngine;

/// <summary>
/// Rigidbodyを使うキャラクタ―のデータ
/// </summary>
[CreateAssetMenu(menuName = "Data/Character", fileName = "NewCharacter")]
public class PlayerData : ScriptableObject
{
    [SerializeField] PhysicsMaterial2D m_physicsMaterial2D = default;

    [SerializeField] float m_moveForceMultiplier = 50f;

    [SerializeField] float m_jumpForce = 11f;

    [SerializeField] float m_moveSpeed = 6f;

    [SerializeField] int m_maxJumpCount = 2;

    /// <summary>
    /// Physicsマテリアルを取得
    /// </summary>
    public PhysicsMaterial2D GetPhysicsMaterial2D { get => m_physicsMaterial2D; }

    /// <summary>
    /// 移動速度入力に対する追従速度
    /// </summary>
    public float GetMoveForceMultiplier { get => m_moveForceMultiplier; }

    /// <summary>
    /// ジャンプ力
    /// </summary>
    public float GetJumpForce { get => m_jumpForce; }

    /// <summary>
    /// 移動速度
    /// </summary>
    public float GetMoveSpeed { get => m_moveSpeed; }

    /// <summary>
    /// ジャンプできる回数
    /// </summary>
    public int GetMaxJumpCount { get => m_maxJumpCount; }
}
