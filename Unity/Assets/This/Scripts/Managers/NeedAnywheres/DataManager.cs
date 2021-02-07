using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static DefTime;

namespace ManagerSystem
{
    /// <summary>
    /// データの管理
    /// </summary>
    public class DataManager : SingletonMonoBehaviour<DataManager>
    {
        #region 変数宣言

        private DataStages m_dataStages;

        private DataAudios m_dataBgms;

        private DataAudios m_dataVoices;

        private DataAudios m_dataSes;

        private List<AudioClip> m_bgmClipList = new List<AudioClip>(20);

        private List<AudioClip> m_voiceClipList = new List<AudioClip>(20);

        private List<AudioClip> m_seClipList = new List<AudioClip>(10);

        /// <summary>
        /// ステージデータ
        /// </summary>
        public DataStages GetDataStages { get => m_dataStages; }

        /// <summary>
        /// BGMのデータ
        /// </summary>
        public DataAudios GetDataBgms { get => m_dataBgms; }

        /// <summary>
        /// Voiceのデータ
        /// </summary>
        public DataAudios GetDataVoices { get => m_dataVoices; }
        
        /// <summary>
        /// Seのデータ
        /// </summary>
        public DataAudios GetDataSes { get => m_dataSes; }

        /// <summary>
        /// BGMのAudioClipのリストを取得
        /// </summary>
        public List<AudioClip> GetBgmClipList { get => m_bgmClipList; }

        /// <summary>
        /// VoiceのAudioClipのリストを取得
        /// </summary>
        public List<AudioClip> GetVoiceClipList { get => m_voiceClipList; }

        /// <summary>
        /// SeのAudioClipのリストを取得
        /// </summary>
        public List<AudioClip> GetSeClipList { get => m_seClipList; }

        #endregion

        #region メソッド

        /// <summary>
        /// データの初期化
        /// </summary>
        public async UniTask Initialize()
        {
            m_dataStages = ResourcesManager.LoadDataStages();

            m_dataBgms   = ResourcesManager.LoadDataBgms();

            m_dataVoices = ResourcesManager.LoadDataVoices();

            m_dataSes    = ResourcesManager.LoadDataSes();

            await LoadAudioClips();

            await UniTask.Delay(DELAY_TIME);
        }

        private async UniTask LoadAudioClips()
        {
            for (int i = 0; i < m_dataBgms.GetList.Count; ++i)
            {
                var bgmClip = ResourcesManager.LoadBgm(m_dataBgms.GetList[i].name);
                if (bgmClip.IsNull()) { continue; }
                m_bgmClipList.Add(bgmClip);
            }

            for (int i = 0; i < m_dataVoices.GetList.Count; ++i)
            {
                var voiceClip = ResourcesManager.LoadVoice(m_dataVoices.GetList[i].name);
                if (voiceClip.IsNull()) { continue; }
                m_voiceClipList.Add(voiceClip);
            }

            for (int i = 0; i < m_dataSes.GetList.Count; ++i)
            {
                var seClip = ResourcesManager.LoadSe(m_dataSes.GetList[i].name);
                if (seClip.IsNull()) { continue; }
                m_seClipList.Add(seClip);
            }

            await UniTask.Delay(1);
        }

        #region AssetBundle

        ///// <summary>
        ///// データの初期化
        ///// </summary>
        //public async UniTask Initialize()
        //{
        //    m_dataStages = AssetBundleManager.LoadDataStages();

        //    m_dataBgms = AssetBundleManager.LoadDataBgms();

        //    m_dataVoices = AssetBundleManager.LoadDataVoices();

        //    m_dataSes = AssetBundleManager.LoadDataSes();

        //    await LoadAudioClips();

        //    await UniTask.Delay(DELAY_TIME);
        //}

        //private async UniTask LoadAudioClips()
        //{
        //    for (int i = 0; i < m_dataBgms.GetList.Count; ++i)
        //    {
        //        var bgmClip = AssetBundleManager.LoadBgm(m_dataBgms.GetList[i].name);
        //        if (bgmClip.IsNull()) { continue; }
        //        m_bgmClipList.Add(bgmClip);
        //    }

        //    for (int i = 0; i < m_dataVoices.GetList.Count; ++i)
        //    {
        //        var voiceClip = AssetBundleManager.LoadVoice(m_dataVoices.GetList[i].name);
        //        if (voiceClip.IsNull()) { continue; }
        //        m_voiceClipList.Add(voiceClip);
        //    }

        //    for (int i = 0; i < m_dataSes.GetList.Count; ++i)
        //    {
        //        var seClip = AssetBundleManager.LoadSe(m_dataSes.GetList[i].name);
        //        if (seClip.IsNull()) { continue; }
        //        m_seClipList.Add(seClip);
        //    }

        //    await UniTask.Delay(1);
        //}

        #endregion

        #endregion
    }
}