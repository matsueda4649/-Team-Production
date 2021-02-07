using UnityEngine;

public static class RaycastSystem
{
    
    /// <summary>
    /// 2Dのレイキャスト
    /// </summary>
    /// <param name="origin">開始地点</param>
    /// <param name="direcion">方向</param>
    /// <param name="distance">距離</param>
    /// <param name="mask">当たり判定を検出するレイヤー</param>
    public static bool RayHit2D(Vector2 origin, Vector2 direcion, float distance, LayerMask mask)
    {
        // Rayに衝突したオブジェクトを検出
        RaycastHit2D hit = Physics2D.Raycast(origin, direcion, distance, mask);
        return hit;
    }

}
