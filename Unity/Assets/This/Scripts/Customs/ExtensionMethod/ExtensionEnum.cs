using System;

/// <summary>
/// Enumの拡張メソッド
/// </summary>
public static class ExtensionEnum
{
    /// <summary>
    /// 指定された列挙型のLengthを取得
    /// </summary>
    /// <typeparam name="T">列挙型の種類</typeparam>
    /// <returns>指定された列挙型のLength</returns>
    public static int GetLength<T>()
    {
        return Enum.GetValues(typeof(T)).Length;
    }
}