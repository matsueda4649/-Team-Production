using UnityEngine;

public static class EnemyMovement
{
    /// <summary>
    /// 一定速度の移動モーション
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="moveSpeed">移動速度</param>
    /// <param name="moveDirection">移動方向</param>
    /// <param name="moveForceMultiplier">追従速度</param>
    public static void AddConstantForce(Rigidbody2D rb, float moveSpeed, Vector2 moveDirection, float moveForceMultiplier)
    {
        // ベクトルを制限する
        Vector2 velocity = moveSpeed * moveDirection;
        Vector2 force = moveForceMultiplier * (velocity - rb.velocity);
        rb.AddForce(force * Time.deltaTime, ForceMode2D.Force);
    }
}
