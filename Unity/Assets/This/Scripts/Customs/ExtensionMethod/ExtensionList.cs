using System.Collections.Generic;

/// <summary>
/// Listの拡張メソッド
/// </summary>
public static class ExtensionList
{
    /// <summary>
    /// Listが空かどうか
    /// </summary>
    /// <returns>リストが空ならtrue</returns>
    public static bool IsEmpty<T>(this List<T> self)
    {
        return self.Count == 0;
    }

    /// <summary>
    /// Listが空かどうか
    /// </summary>
    /// <returns>リストが空ならtrue</returns>
    public static bool IsEmpty<T>(this IList<T> self)
    {
        return self.Count == 0;
    }

    /// <summary>
    /// Listが空かどうか
    /// </summary>
    /// <returns>リストが空ならtrue</returns>
    public static bool IsEmpty<T>(this IReadOnlyList<T> self)
    {
        return self.Count == 0;
    }
}
