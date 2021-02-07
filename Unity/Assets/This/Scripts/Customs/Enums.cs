/// <summary>
/// フェーズ
/// </summary>
public enum Phase
{
    /// <summary>
    /// 無し
    /// </summary>
    None = 0,
    /// <summary>
    /// 初期時
    /// </summary>
    Init,
    /// <summary>
    /// 進行中
    /// </summary>
    Play,
    /// <summary>
    /// ゲームクリア
    /// </summary>
    GameClear,
    /// <summary>
    /// ゲームオーバー
    /// </summary>
    GameOver
}

/// <summary>
/// フェーズ内のステップ
/// </summary>
public enum PhaseStep
{
    First = 1,
    Second,
    Third,
    Fourth,
    Fifith,
}

/// <summary>
/// 使用するBGMのインデックス
/// </summary>
public enum AudioBgm
{
    /// <summary>
    /// タイトル
    /// </summary>
    Title,
    /// <summary>
    /// ステージ選択画面
    /// </summary>
    StageSelect,
    /// <summary>
    /// オープニング　のどか
    /// </summary>
    Opening_1,
    /// <summary>
    /// オープニング　はらはら
    /// </summary>
    Opening_2,
    /// <summary>
    /// 村
    /// </summary>
    Village,
    /// <summary>
    /// 雪
    /// </summary>
    Snow,
    /// <summary>
    /// 神秘的
    /// </summary>
    Mysterious,
    /// <summary>
    /// 静か
    /// </summary>
    Quiet_1,
    /// <summary>
    /// 静か
    /// </summary>
    Quiet_2,
    /// <summary>
    /// 渓谷, 民族風
    /// </summary>
    Sky,
    /// <summary>
    /// 宇宙っぽい
    /// </summary>
    Space,
    /// <summary>
    /// ラスボス感
    /// </summary>
    LastBoss,
}

/// <summary>
/// 使用するVoiceのインデックス
/// </summary>
public enum AudioVoice
{
    /// <summary>
    /// ジャンプ
    /// </summary>
    Jump,
    /// <summary>
    /// 大なり、小なり
    /// </summary>
    Than,
    /// <summary>
    /// アスタリスク
    /// </summary>
    Asterisk,
    /// <summary>
    /// ハッシュ, フラット
    /// </summary>
    Hash,
    /// <summary>
    /// シャープ
    /// </summary>
    Sharp,
    /// <summary>
    /// 鬼( こん棒攻撃 )
    /// </summary>
    Demon,
    /// <summary>
    /// 刺す
    /// </summary>
    Stab,
    /// <summary>
    /// 右大臣 弓飛ばし
    /// </summary>
    Arrow,
    /// <summary>
    /// 太鼓 Singer
    /// </summary>
    Drum,
    /// <summary>
    /// 爆発
    /// </summary>
    Bomb,
    /// <summary>
    /// ビーム
    /// </summary>
    Beam,
    /// <summary>
    /// おもちゃの兵隊　銃
    /// </summary>
    ToyGun,
    /// <summary>
    /// 宇宙攻撃
    /// </summary>
    DarkMasic,
}

/// <summary>
/// 使用するSEのインデックス
/// </summary>
public enum AudioSe
{
    /// <summary>
    /// 決定音
    /// </summary>
    Decision,
    /// <summary>
    /// 閉じる
    /// </summary>
    Close,
    /// <summary>
    /// ステージ選択
    /// </summary>
    SelectStage,
    /// <summary>
    /// ゲームオーバー
    /// </summary>
    GameOver,
    /// <summary>
    /// ゲームクリア
    /// </summary>
    GameClear,
}

/// <summary>
/// 移動方向
/// </summary>
public enum Direction
{
    Right = 1,
    Left = -1,
}

/// <summary>
/// 移動方向
/// </summary>
public enum Coordinate
{
    /// <summary>
    /// 横
    /// </summary>
    Vertical,
    /// <summary>
    /// 縦
    /// </summary>
    Horizontal,
}

/// <summary>
/// 入力タイプ
/// </summary>
public enum InputType
{
    /// <summary>
    /// 無し
    /// </summary>
    None = -1,
    /// <summary>
    /// 開始
    /// </summary>
    Began = 0,
    /// <summary>
    /// 移動
    /// </summary>
    Moved = 1,
    /// <summary>
    /// 静止
    /// </summary>
    Stationary = 2,
    /// <summary>
    /// 終了
    /// </summary>
    Ended = 3,
    /// <summary>
    /// キャンセル
    /// </summary>
    Canceled = 4,
}