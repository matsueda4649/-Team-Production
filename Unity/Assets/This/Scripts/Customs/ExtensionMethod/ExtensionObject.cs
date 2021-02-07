using UnityEngine;
/// <summary>
/// Objectの拡張メソッド
/// </summary>
public static class ExtensionObject
{
    /// <summary>
    /// NULLチェック
    /// </summary>
    /// <param name="self"></param>
    /// <returns>NULLだったらTrue</returns>
    public static bool IsNull(this Object self)
    {
        return self == null;
    }
}
