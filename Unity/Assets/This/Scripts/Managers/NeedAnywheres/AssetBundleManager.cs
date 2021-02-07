using System.IO;
using UnityEngine;

public static class AssetBundleManager
{
    /// <summary>
    /// データの読み込み
    /// </summary>
    /// <param name="scriptableDataName">データの名前</param>
    /// <returns>データ</returns>
    private static T LoadData<T>(string scriptableDataName) where T : Data
    {

        string path = "Assets/AssetBundles/Android/data";

#if UNITY_ANDROID

        path = Path.Combine(Application.streamingAssetsPath, "data");

#endif

        // AsssetBundleを読み込む
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

        // データを読み込む
        var data = assetBundle.LoadAsset<T>(scriptableDataName);

        // データがなければ
        if (data.IsNull()){ DebugManager.Error($"{scriptableDataName} = Null"); }

        // データを初期化
        data.Initialize();

        assetBundle.Unload(false);

        return data;
    }

    /// <summary>
    /// ステージデータの読み込み
    /// </summary>
    /// <returns>ステージデータ</returns>
    public static DataStages LoadDataStages()
    {
        return LoadData<DataStages>("StageData");
    }

    /// <summary>
    /// BGMデータの読み込み
    /// </summary>
    /// <returns>BGMのデータ</returns>
    public static DataAudios LoadDataBgms()
    {
        return LoadData<DataAudios>("BGMData");
    }

    /// <summary>
    /// Voiceデータの読み込み
    /// </summary>
    /// <returns>Voiceのデータ</returns>
    public static DataAudios LoadDataVoices()
    {
        return LoadData<DataAudios>("VoiceData"); ;
    }

    /// <summary>
    /// Seデータの読み込み
    /// </summary>
    /// <returns>Seのデータ</returns>
    public static DataAudios LoadDataSes()
    {
        return LoadData<DataAudios>("SEData");
    }

    /// <summary>
    /// AudioClipの読み込み
    /// </summary>
    /// <param name="audioClipName">AudioClipの名前</param>
    /// <returns>AudioClip</returns>
    private static AudioClip LoadAudioClip(string audioClipName)
    {
        // AsssetBundleを読み込む
        AssetBundle assetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/Android/audios");

        var audioClip = assetBundle.LoadAsset<AudioClip>(audioClipName);

        // AudioClipがない場合
        if (audioClip.IsNull()) { DebugManager.Error($"{audioClipName} = Null"); }

        assetBundle.Unload(false);

        return audioClip;
    }

    /// <summary>
    /// BGMの取得
    /// </summary>
    /// <param name="bgmName">名前</param>
    /// <returns>BGM</returns>
    public static AudioClip LoadBgm(string bgmName)
    {
        return LoadAudioClip(bgmName);
    }

    /// <summary>
    /// Voiceの取得
    /// </summary>
    /// <param name="voiceName">名前</param>
    /// <returns>Voice</returns>
    public static AudioClip LoadVoice(string voiceName)
    {
        return LoadAudioClip(voiceName);
    }

    /// <summary>
    /// Seの取得
    /// </summary>
    /// <param name="seName">名前</param>
    /// <returns>SE</returns>
    public static AudioClip LoadSe(string seName)
    {
        return LoadAudioClip(seName);
    }
}
