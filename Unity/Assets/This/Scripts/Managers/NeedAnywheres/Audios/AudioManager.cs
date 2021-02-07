using Cysharp.Threading.Tasks;
using UnityEngine;
using static DefNum;

namespace ManagerSystem
{
    /// <summary>
    /// オーディオを管理
    /// </summary>
    public class AudioManager : SingletonMonoBehaviour<AudioManager>, IUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// BGMを管理
        /// </summary>
        [SerializeField] AudioSourceManager m_bgmAudioSource = default;

        /// <summary>
        /// Voiceを管理
        /// </summary>
        [SerializeField] AudioSourceManager m_voiceAudioSource = default;

        /// <summary>
        /// SEを管理
        /// </summary>
        [SerializeField] AudioSourceManager m_seAudioSource = default;

        /// <summary>
        /// BGMのオーディオ設定
        /// </summary>
        private AudioSetting m_bgmAudioSetting = new AudioSetting();

        /// <summary>
        /// Voiceのオーディオ設定
        /// </summary>
        private AudioSetting m_voiceAudioSetting = new AudioSetting();

        /// <summary>
        /// SEのオーディオ設定
        /// </summary>
        private AudioSetting m_seAudioSetting = new AudioSetting();

        #endregion

        #region メソッド

        public void UpdateMe()
        {
            m_bgmAudioSource.CheckAudioStop();
            m_seAudioSource.CheckAudioStop();
            m_voiceAudioSource.CheckAudioStop();
        }

        /// <summary>
        /// AudioClipの読み込み
        /// </summary>
        /// <returns></returns>
        public async UniTask LoadAudioClips()
        {
            // AudioClipを取得
            GetBgmAudioClips();
            GetVoiceAudioClips();
            GetSeAudioClips();

            await UniTask.Delay(1);
        }

        /// <summary>
        /// 初期設定
        /// </summary>
        public async UniTask Initialize()
        {
            await LoadAudioClips();

            // 必要な数のAudioSorceを追加
            m_bgmAudioSource.AddAudioSource(BGM_CHANNEL_NUM, true, true);
            m_voiceAudioSource.AddAudioSource(VOICE_CHANNEL_NUM);
            m_seAudioSource.AddAudioSource(SE_CHANNEL_NUM);

            // AudioSettingの初期化
            InitializeAudioSetting();

            await UniTask.Delay(DefTime.DELAY_TIME);
        }

        /// <summary>
        /// BGMの取得
        /// </summary>
        private void GetBgmAudioClips()
        {
            m_bgmAudioSource.ClearList();

            var bgmClipList = DataManager.Instance.GetBgmClipList;

            for (int i = 0, count = bgmClipList.Count; i < count; ++i)
            {
                if (!bgmClipList[i].IsNull())
                {
                    m_bgmAudioSource.AddAudioClip(bgmClipList[i]);
                }
            }
        }

        /// <summary>
        /// Voiceの取得
        /// </summary>
        private void GetVoiceAudioClips()
        {
            m_voiceAudioSource.ClearList();

            var voiceClipList = DataManager.Instance.GetVoiceClipList;

            for (int i = 0, count = voiceClipList.Count; i < count; ++i)
            {
                if (!voiceClipList[i].IsNull())
                {
                    m_voiceAudioSource.AddAudioClip(voiceClipList[i]);
                }
            }
        }

        /// <summary>
        /// SEの取得
        /// </summary>
        private void GetSeAudioClips()
        {
            m_seAudioSource.ClearList();

            var seClipList = DataManager.Instance.GetSeClipList;

            for (int i = 0, count = seClipList.Count; i < count; ++i)
            {
                if (!seClipList[i].IsNull())
                {
                    m_seAudioSource.AddAudioClip(seClipList[i]);
                }
            }
        }

        /// <summary>
        /// AudioSettingの初期化
        /// </summary>
        public void InitializeAudioSetting()
        {
            // セーブデータを取得
            var saveData = SaveManager.Instance.GetSaveData;

            if(saveData == null) { DebugManager.Error("SaveData = Null"); }

            // NULLチェック
            if (saveData.bgmAudioSetting == null || saveData.voiceAudioSetting == null || saveData.seAudioSetting == null)
            {
                saveData.ResetAudioSetting();
            }

            m_bgmAudioSetting   = saveData.bgmAudioSetting;
            m_voiceAudioSetting = saveData.voiceAudioSetting;
            m_seAudioSetting    = saveData.seAudioSetting;

            m_bgmAudioSource.InitializeAudioSetting(m_bgmAudioSetting, BGM_CHANNEL_NUM);
            m_voiceAudioSource.InitializeAudioSetting(m_voiceAudioSetting, VOICE_CHANNEL_NUM);
            m_seAudioSource.InitializeAudioSetting(m_seAudioSetting, SE_CHANNEL_NUM);
        }

        /// <summary>
        /// BGMのAudioSourceの設定
        /// </summary>
        /// <param name="volume">音量</param>
        /// <param name="mute">ミュート</param>
        public void SetBgmAudioSource(float volume, bool mute)
        {
            m_bgmAudioSource.SetAudioSource(volume, mute);
        }

        /// <summary>
        /// VoiceのAudioSourceの設定
        /// </summary>
        /// <param name="volume">音量</param>
        /// <param name="mute">ミュート</param>
        public void SetVoiceAudioSource(float volume, bool mute)
        {
            m_voiceAudioSource.SetAudioSource(volume, mute);
        }

        /// <summary>
        /// SeのAudioSourceの設定
        /// </summary>
        /// <param name="volume">音量</param>
        /// <param name="mute">ミュート</param>
        public void SetSeAudioSource(float volume, bool mute)
        {
            m_seAudioSource.SetAudioSource(volume, mute);
        }

        /// <summary>
        /// BGMを再生
        /// </summary>
        /// <param name="index">再生するBGMのインデックス</param>
        public void PlayBgm(AudioBgm index)
        {
            m_bgmAudioSource.PlayAudio((int)index, BGM_CHANNEL_NUM);
        }

        /// <summary>
        /// Voiceを再生
        /// </summary>
        /// <param name="index">再生するVoiceのインデックス</param>
        public void PlayVoice(AudioVoice index)
        {
            m_voiceAudioSource.PlayAudio((int)index, VOICE_CHANNEL_NUM, true);
        }
        /// <summary>
        /// SEを再生
        /// </summary>
        /// <param name="index">再生するSeのインデックス</param>
        public void PlaySe(AudioSe index)
        {
            m_seAudioSource.PlayAudio((int)index, SE_CHANNEL_NUM, true);
        }

        /// <summary>
        /// BGMの停止
        /// </summary>
        public void StopBgm()
        {
            m_bgmAudioSource.CheckAudioStop(true);
        }

        /// <summary>
        /// Seの停止
        /// </summary>
        public void StopSe()
        {
            m_seAudioSource.CheckAudioStop(true);
        }

        /// <summary>
        /// Voiceの停止
        /// </summary>
        public void StopVoice()
        {
            m_voiceAudioSource.CheckAudioStop(true);
        }

        public void OnEnable()
        {
            UpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            UpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}