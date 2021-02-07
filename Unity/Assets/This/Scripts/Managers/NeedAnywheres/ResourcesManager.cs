using UnityEngine;

namespace ManagerSystem
{
    /// <summary>
    /// データの取得
    /// </summary>
    public static class ResourcesManager
    {
        #region メソッド

        /// <summary>
        /// データの読み込み
        /// </summary>
        /// <param name="scriptableDataName">データの名前</param>
        /// <returns>データ</returns>
        private static T LoadData<T>(string scriptableDataName) where T : Data
        {
            // データを読み込む
            T data = (T)Resources.Load($"Data/{scriptableDataName}");

            // データがなければ
            if(data.IsNull())
            {
                DebugManager.Error($"{scriptableDataName} = Null");
            }

            // データを初期化
            data.Initialize();

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
            return LoadData<DataAudios>("VoiceData");;
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
            var audioClip = Resources.Load<AudioClip>($"Audios/{audioClipName}");

            // AudioClipがない場合
            if (audioClip.IsNull()) { DebugManager.Error($"{audioClipName} = Null"); }

            return audioClip;
        }

        /// <summary>
        /// BGMの取得
        /// </summary>
        /// <param name="bgmName">名前</param>
        /// <returns>BGM</returns>
        public static AudioClip LoadBgm(string bgmName)
        {
            return LoadAudioClip($"BGMs/{ bgmName}");
        }

        /// <summary>
        /// Voiceの取得
        /// </summary>
        /// <param name="voiceName">名前</param>
        /// <returns>Voice</returns>
        public static AudioClip LoadVoice(string voiceName)
        {
            return LoadAudioClip($"Voices/{voiceName}");
        }

        /// <summary>
        /// Seの取得
        /// </summary>
        /// <param name="seName">名前</param>
        /// <returns>SE</returns>
        public static AudioClip LoadSe(string seName)
        {
            return LoadAudioClip($"SEs/{seName}");
        }

        /// <summary>
        /// Prefabの読み込み
        /// </summary>
        /// <param name="prefabName">プレハブの名前</param>
        /// <returns>オブジェクト</returns>
        private static GameObject LoadPrefab(string prefabName)
        {
            var prefab = Resources.Load<GameObject>($"Prefabs/{prefabName }");

            // Prefabがない場合
            if (prefab.IsNull()) { DebugManager.Error($"{prefabName} = Null"); }

            return prefab;
        } 

        /// <summary>
        /// ManagerのPrefabの読み込み
        /// </summary>
        /// <param name="managerName"></param>
        public static GameObject LoadManagerPrefab(string managerName)
        {
            return LoadPrefab($"Managers/{managerName }");
        }

        #endregion
    }
}