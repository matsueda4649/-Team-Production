public class DefNum
{
    /// <summary>
    /// ステージの初期インデックス
    /// </summary>
    public static readonly int STAGE_INIT_NUMBER = 1000;

    /// <summary>
    /// ステージの初期ID
    /// </summary>
    public static readonly int STAGE_FIRST_NUMBER = 1001;

    /// <summary>
    /// 最大音量
    /// </summary>
    public static readonly float MAX_VOLUME = 1f;

    /// <summary>
    /// BGMのチャンネル数
    /// </summary>
    public static readonly int BGM_CHANNEL_NUM = 1;

    /// <summary>
    /// Voiceのチャンネル数
    /// </summary>
    public static readonly int VOICE_CHANNEL_NUM = 3;

    /// <summary>
    /// Seのチャンネル数
    /// </summary>
    public static readonly int SE_CHANNEL_NUM = 5;
}

/// <summary>
/// 時間の設定
/// </summary>
public class DefTime
{
    /// <summary>
    /// 停止
    /// </summary>
    public static readonly float STOP = 0.0f;

    /// <summary>
    /// スロー
    /// </summary>
    public static readonly float SLOW = 0.5f;

    /// <summary>
    /// 再生
    /// </summary>
    public static readonly float PLAY = 1.0f;

    /// <summary>
    /// 次のフェーズに映る時間
    /// </summary>
    public static readonly float NEXT_PHASE_TIME  = 1.5f;

    /// <summary>
    /// ゲーム終了の待機時間
    /// </summary>
    public static readonly float WAIT_PHASE_TIME = 1.5f;

    /// <summary>
    /// フェードイン時間
    /// </summary>
    public static readonly float FADE_IN_TIME = 0.5f;

    /// <summary>
    /// フェードアウト時間
    /// </summary>
    public static readonly float FADE_OUT_TIME = 0.8f;

    /// <summary>
    /// 待機時間
    /// </summary>
    public static readonly float WAIT_TIME = 1.5f;

    /// <summary>
    /// UniTask Delayの時間
    /// </summary>
    public static readonly int DELAY_TIME = 1;
}

/// <summary>
/// 弾の設定
/// </summary>
public class DefsBullet
{
    /// <summary>
    /// 初期時の弾の最大生成数
    /// </summary>
    public static readonly int NORMAL_MAX_COUNT = 2;
}

/// <summary>
/// ウインドウの設定
/// </summary>
public class DefsWindow
{
    /// <summary>
    /// 最小サイズ
    /// </summary>
    public static readonly float MIN_SIZE = 0.0f;

    /// <summary>
    /// 最大サイズ
    /// </summary>
    public static readonly float MAX_SIZE = 1.0f;

    /// <summary>
    /// ダイヤログ表示速度
    /// </summary>
    public static readonly float POP_OPEN_SPEED = 0.25f;

    /// <summary>
    /// ダイヤログ非表示速度
    /// </summary>
    public static readonly float POP_CLOSE_SPEED = 0.4f;
}