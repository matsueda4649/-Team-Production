using System.Collections.Generic;
using ManagerSystem;
using static DefNum;

/// <summary>
/// セーブデータ
/// </summary>
[System.Serializable]
public class SaveData
{
    /// <summary>
    /// 難易度を簡単にするかどうか
    /// </summary>
    public bool easyMode = false;

    /// <summary>
    /// 新規かどうか
    /// </summary>
    public bool onNewGame = true;

    /// <summary>
    /// クエストのID
    /// </summary>
    public int questId;

    /// <summary>
    /// 新しく開放したクエスト
    /// </summary>
    public List<int> newStageIds;

    /// <summary>
    /// 解放済みのクエスト
    /// </summary>
    public List<int> openStageIds;

    /// <summary>
    /// クリア済みのクエスト
    /// </summary>
    public List<int> clearStageIds;

    /// <summary>
    /// スコア
    /// </summary>
    public Score score;

    /// <summary>
    /// BGM
    /// </summary>
    public AudioSetting bgmAudioSetting;

    /// <summary>
    /// Voice
    /// </summary>
    public AudioSetting voiceAudioSetting;

    /// <summary>
    /// SE
    /// </summary>
    public AudioSetting seAudioSetting;

    public SaveData()
    {
        this.score = new Score();

        this.bgmAudioSetting = new AudioSetting();
        this.voiceAudioSetting = new AudioSetting();
        this.seAudioSetting = new AudioSetting();

        ResetStageIdList();
        ResetAudioSetting();
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void Reset()
    {
        this.onNewGame = true;
        this.easyMode = false;
        ResetScore(DataManager.Instance.GetDataStages);
        ResetStageIdList();
        ResetAudioSetting();
    }

    /// <summary>
    /// ステージリストの初期化
    /// </summary>
    private void ResetStageIdList()
    {
        this.newStageIds   = new List<int>(48);
        this.openStageIds  = new List<int>(48);
        this.clearStageIds = new List<int>(48);
    }

    /// <summary>
    /// スコアの初期化
    /// </summary>
    /// <param name="dataStages"></param>
    private void ResetScore(DataStages dataStages)
    {
        score = new Score
        {
            scoreDic = new Dictionary<int, float>(48)
        };

        for(int i = 0, count = dataStages.GetList.Count; i < count; ++i)
        {
            if(dataStages.GetList[i] != null)
            {
                score.scoreDic.Add(dataStages.GetList[i].id, 999f);
            }
        }
    }

    /// <summary>
    /// Audio設定の初期化
    /// </summary>
    public void ResetAudioSetting()
    {
        // ボリューム
        bgmAudioSetting.volume = MAX_VOLUME / 2f;
        voiceAudioSetting.volume = MAX_VOLUME / 2f;
        seAudioSetting.volume = MAX_VOLUME / 2f;

        // ミュートの設定
        bgmAudioSetting.mute = false;
        voiceAudioSetting.mute = false;
        seAudioSetting.mute = false;

        // ループ再生
        bgmAudioSetting.onLoop = true;
        voiceAudioSetting.onLoop = false;
        seAudioSetting.onLoop = false;

    }
}
