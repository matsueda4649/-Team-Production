using UnityEngine;

/// <summary>
/// Transformの拡張メソッド
/// </summary>
public static class ExtensionTransform
{
    /// <summary>
    /// transform.localPositionを設定
    /// </summary>
    /// <param name="vector">Vector3</param>
    public static void SetLocalPosition(this Transform self, Vector3 vector)
    {
        self.transform.localPosition = vector;
    }

    /// <summary>
    /// transform.localPositionのx座標を設定
    /// </summary>
    /// <param name="x">Y座標</param>
    public static void SetLocalPositionX(this Transform self, float x)
    {
        self.transform.localPosition = new Vector2(x, self.localPosition.y);
    }

    /// <summary>
    /// transform.localPositionのy座標を設定
    /// </summary>
    /// <param name="y">Y座標</param>
    public static void SetLocalPositionY(this Transform self, float y)
    {
        self.transform.localPosition = new Vector2(self.localPosition.x, y);
    }

    /// <summary>
    /// transform.localPositionに追加( x座標 )
    /// </summary>
    /// <param name="x">Y座標</param>
    public static void AddLocalPositionX(this Transform self, float x)
    {
        self.transform.localPosition += new Vector3(x, self.localPosition.y);
    }

    /// <summary>
    /// transform.localPositionに追加( y座標 )
    /// </summary>
    /// <param name="y">Y座標</param>
    public static void AddLocalPositionY(this Transform self, float y)
    {
        self.transform.localPosition += new Vector3(self.localPosition.x, y);
    }

    /// <summary>
    /// transform.localPositionのz座標を設定
    /// </summary>
    /// <param name="z">Y座標</param>
    public static void SetLocalPositionZ(this Transform self, float z)
    {
        self.transform.localPosition = new Vector3(self.localPosition.x, self.localPosition.y, z);
    }

    /// <summary>
    /// transform.localPositionのy座標をLerpで設定
    /// </summary>
    /// <param name="y">Y座標</param>
    /// <param name="lerpSpeed">ラープ速度</param>
    public static void SetLerpLocalPositionY(this Transform self, float y, float lerpSpeed)
    {
        var start  = self.transform.localPosition;
        var target = new Vector2(self.localPosition.x, y);
        self.transform.localPosition = Vector2.Lerp(start, target, lerpSpeed);
    }

    /// <summary>
    /// 軸と角度を設定
    /// </summary>
    /// <param name="angle">回転角度</param>
    /// <param name="vector">回転軸</param>
    public static void SetLocalRotation(this Transform self, float angle, Vector3 vector)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, vector);
        self.localRotation = rotation;
    }

    /// <summary>
    /// パラメータを代入
    /// </summary>
    /// <param name="angles">設定するAngles</param>
    public static void SetLocalRotation(this Transform self, Vector3 angles)
    {
        self.transform.localRotation = Quaternion.Euler(angles);
    }

    /// <summary>
    /// scaleの設定
    /// </summary>
    /// <param name="scales">スケール</param>
    public static void SetLocalScale(this Transform self, Vector2 scales)
    {
        self.transform.localScale = scales;
    }

    /// <summary>
    /// scaleのyをy倍する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="y"></param>
    public static void SetLocalScaleYs(this Transform self, float y)
    {
        Vector2 scale = self.transform.localScale;
        self.transform.localScale = new Vector2(scale.x, scale.y * y);
    }

    /// <summary>
    /// 対象が自身より下にいるかどうか
    /// </summary>
    /// <param name="target">対象</param>
    /// <returns></returns>
    public static bool UnderPosition(this Transform self, Transform target)
    {
        return self.position.y <= target.position.y;
    }

    /// <summary>
    /// 対象が自身より下にいるかどうか
    /// </summary>
    /// <param name="target">対象</param>
    /// <returns></returns>
    public static bool UnderLocalPosition(this Transform self, Transform target)
    {
        return self.localPosition.y <= target.localPosition.y;
    }
}