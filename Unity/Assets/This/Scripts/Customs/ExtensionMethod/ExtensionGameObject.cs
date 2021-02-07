using UnityEngine;

/// <summary>
/// GameObjectの拡張メソッド
/// </summary>
public static class ExtensionGameObject
{
    /// <summary>
    /// アクティブにする
    /// </summary>
    /// <param name="self"></param>
    public static void Activation(this GameObject self)
    {
        self.SetActive(true);
    }

    /// <summary>
    /// 非アクティブにする
    /// </summary>
    /// <param name="self"></param>
    public static void Deactovation(this GameObject self)
    {
        self.SetActive(false);
    }

    /// <summary>
    /// オブジェクトの表示管理
    /// </summary>
    /// <param name="active"></param>
    public static void SetActive(this Component self, bool active)
    {
        self.gameObject.SetActive(active);
    }

    /// <summary>
    /// オブジェクトの表示
    /// </summary>
    public static void Activation(this Component self)
    {
        self.SetActive(true);
    }

    /// <summary>
    /// オブジェクトの非表示
    /// </summary>
    public static void Deactovation(this Component self)
    {
        self.SetActive(false);
    }
}
