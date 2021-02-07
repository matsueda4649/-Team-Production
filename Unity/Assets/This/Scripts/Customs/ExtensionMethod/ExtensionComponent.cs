using UnityEngine;

/// <summary>
/// Componentの拡張メソッド
/// </summary>
public static class ExtensionComponent
{
    public static T GetComponent<T>(this Collision2D self) where T : Component
    {
        return self.gameObject.GetComponent<T>();
    }

    /// <summary>
    /// 指定されたコンポーネントを返す
    /// アタッチされていない場合は追加してから返す
    /// </summary>
    public static T SafeGetComponent<T> (this GameObject self) where T : Component
    {
        T component = null;
        self.SetComponent(ref component);
        if (component.IsNull()) { component = self.AddComponent<T>(); }
        return component; 
    }

    /// <summary>
    /// 指定されたコンポーネントを返す
    /// アタッチされていない場合は追加してから返す
    /// </summary>
    public static T SafeGetComponent<T> (this Component self) where T : Component
    {
        T component = null;
        self.gameObject.SetComponent(ref component);
        if (component.IsNull()) { component = self.gameObject.AddComponent<T>(); }
        return component;
    }

    /// <summary>
    /// コンポーネントを取得し設定する
    /// </summary>
    /// <param name="set">コンポーネントを設定する対象</param>
    /// <returns></returns>
    public static void SetComponent<T>(this GameObject self, ref T set) where T : Component
    {
        set = self.GetComponent<T>();
    }

    /// <summary>
    /// 自身のコンポーネントを設定する
    /// </summary>
    /// <param name="set">コンポーネントを設定する対象</param>
    /// <returns></returns>
    public static void SetComponent<T>(this Component self, ref T set) where T : Component
    {
        set = self.GetComponent<T>();
    }

    /// <summary>
    /// FindGameObjectWithTag
    /// </summary>
    /// <returns></returns>
    public static bool FindTagComponent<T>(ref T set, string tag) where T : Component
    {
        var obj = GameObject.FindGameObjectWithTag(tag);
        obj.SetComponent(ref set);
        return set != null;
    }

    /// <summary>
    /// FindObjectOfType
    /// </summary>
    /// <returns></returns>
    public static bool FindTypeComponent<T>(ref T set) where T : Component
    {
        set = GameObject.FindObjectOfType<T>();
        return set != null;
    }
}
