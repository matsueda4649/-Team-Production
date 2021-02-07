using UnityEngine;
/// <summary>
/// Colorの拡張メソッド
/// </summary>
public static class ExtensionColor
{
    /// <summary>
    /// アルファ値の設定
    /// </summary>
    /// <param name="setColor">設定する色</param>
    /// <param name="setAlpha">設定するアルファ値</param>
    /// <param name="leapSpeed">ラープ速度</param>
    public static Color SetAlpha(this SpriteRenderer self, Color setColor, float setAlpha, float leapSpeed)
    {
        Color b = new Color(setColor.r, setColor.g, setColor.b, setAlpha);
        self.color = Color.Lerp(self.color, b, leapSpeed);
        return self.color;
    }
}

public static class Colors
{
    /// <summary>
    /// 強い緑　植物を連想させる
    /// </summary>
    public static readonly Color StrongGreen = new Color32(0, 175, 0, 255);
}