/// <summary>
/// オーディオ設定
/// </summary>
[System.Serializable]
public class AudioSetting
{
    #region 変数宣言

    /// <summary>
    /// 音量
    /// </summary>
    public float volume;

    /// <summary>
    /// ミュート
    /// </summary>
    public bool mute;

    /// <summary>
    /// ループ再生するかどうか
    /// </summary>
    public bool onLoop;

    #endregion
}