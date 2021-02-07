using ItemSystem;
using UnityEngine;
using static UnityEngine.Mathf;
using static ExtensionMathf;

public static class EnemyAttack 
{
    /// <summary>
    /// Bulletの初期化
    /// </summary>
    /// <param name="bullet">設定するBullet</param>
    /// <param name="start">自身の座標</param>
    /// <param name="target">目標の座標</param>
    /// <param name="lockOn">ターゲットを狙うかどうか</param>
    public static void InitializeBullet(NormalBullet bullet, Vector2 start, Vector2 target, bool lockOn)
    {
        // 座標を初期化し、表示する
        bullet.transform.SetLocalPosition(start);
        bullet.gameObject.Activation();

        // パラメータ設定
        var direction = Sign(target.x - start.x);  // 移動方向
        var angle = lockOn ? 90f - CalculateRadian(start, target) : 0f; // アングル
        bullet.MoveDirection = new Vector2(direction, Sin(angle * Deg2Rad));
        bullet.transform.SetLocalRotation(angle, Vector3.forward);
    }
}
