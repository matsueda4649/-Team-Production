using UnityEngine;
using static UnityEngine.Mathf;

/// <summary>
/// Rigidbody2Dの拡張メソッド
/// <para>※ 2DスプライトのためのRigidbody 物理挙動コンポーネント</para>
/// </summary>
public static class ExtensionRigidbody2D
{
    /// <summary>
    /// <para>回転の抵抗の設定</para>
    /// <para>値( 小 ) 止まりにくくなる</para>
    /// <para>値( 大 ) 止まりやすくなる</para>
    /// </summary>
    /// <param name="angularDrag">回転の抵抗</param>
    public static float SetAngularDrag(this Rigidbody2D self, float angularDrag)
    {
        return self.angularDrag += angularDrag;
    }

    /// <summary>
    /// bodyTypeを設定する
    /// <para>Dynamic      物理演算が適応される</para>
    /// <para>Kinematic    物理演算が適応されない ( 動かせる )</para>
    /// <para>Static       全く動かない ( 処理が軽い )</para>
    /// </summary>
    /// <param name="bodyType">物理的な動作のタイプ</param>
    public static void SetBodyType(this Rigidbody2D self, RigidbodyType2D bodyType)
    {
        self.bodyType = bodyType;
    }

    /// <summary>
    /// 自身にかかる重力による影響を受ける度合いを設定
    /// </summary>
    /// <param name="gravityScale">重力による影響を受ける度合い</param>
    public static void SetGravityScale(this Rigidbody2D self, float gravityScale)
    {
        self.gravityScale = gravityScale;
    }

    /// <summary>
    /// 落下判定
    /// </summary>
    /// <param name="containsZero">0を含むかどうか</param>
    public static bool IsDown(this Rigidbody2D self, bool containsZero = true)
    {
        var velocity = Mathf.Round(self.velocity.y);
        return containsZero ? velocity <= 0f : velocity < 0f;
    }


    /// <summary>
    /// 地面に着地しているかどうか
    /// </summary>
    /// <returns></returns>
    public static bool OnGround(this Rigidbody2D self)
    {
        return self.velocity.y == 0;
    }

    /// <summary>
    /// 左右に移動しているかどうか
    /// </summary>
    public static bool IsMove(this Rigidbody2D self)
    {
        return self.velocity.x != 0;
    }

    /// <summary>
    /// 等速でX軸にAddForceする
    /// </summary>
    /// <param name="velocity">移動ベクトル</param>
    public static void ConstantAddForceX(this Rigidbody2D self, Vector2 velocity)
    {
        if (self.velocity.magnitude < Abs(velocity.x))
        {
            self.AddForce(velocity, ForceMode2D.Force);
        }
    }
}