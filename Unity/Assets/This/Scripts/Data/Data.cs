using UnityEngine;

/// <summary>
/// データの初期化
/// </summary>
[System.Serializable]
public abstract class Data : ScriptableObject
{
    /// <summary>
    /// 初期化
    /// </summary>
    public abstract void Initialize();
}
