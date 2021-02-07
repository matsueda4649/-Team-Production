using UnityEngine;
/// <summary>
/// ステージデータ
/// </summary>
[System.Serializable]
public class DataStage : MasterData
{
    /// <summary>
    /// 制限時間
    /// </summary>
    public float limitTime;

    /// <summary>
    /// 次のクエストのID
    /// </summary>
    public int nextId;

    /// <summary>
    /// 再生するBGM
    /// </summary>
    public AudioBgm audio;

    /// <summary>
    /// ステージ侵入可能かどうか
    /// </summary>
    public bool allow;
}
