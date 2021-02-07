using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ManagerSystem 
{
    /// <summary>
    /// AudioSorceの管理
    /// </summary>
    public class AudioSourceManager : MonoBehaviour
    {
        #region 変数宣言

        [SerializeField] AudioMixerGroup m_audioMixer = default;

        /// <summary>
        /// AudioClipsのリスト
        /// </summary>
        private List<AudioClip> m_audioClipList = new List<AudioClip>(20);

        /// <summary>
        /// AudioSorceのリスト
        /// </summary>
        private List<AudioSource> m_audioSourceList = new List<AudioSource>(10);

        /// <summary>
        /// AudioSourceの数
        /// </summary>
        private int m_audioCount = 0;

        #endregion

        #region メソッド

        /// <summary>
        /// AudioSorceの追加
        /// </summary>
        /// <param name="channelNum">チャンネル数</param>
        /// <param name="onLoop">ループ</param>
        public void AddAudioSource(int channelNum, bool onLoop = false, bool playOnAwake = false)
        {
            // 設定されたチェンネル数より少なかったら
            if(m_audioSourceList.Count < channelNum)
            {
                for (int i = 0; i < channelNum; ++i)
                {
                    // AudioSorceを追加
                    var audioSource = this.gameObject.AddComponent<AudioSource>();

                    // 2D音声にする
                    audioSource.spatialBlend = 0;
                    audioSource.outputAudioMixerGroup = m_audioMixer;
                    audioSource.playOnAwake = playOnAwake;
                    audioSource.loop = onLoop;
                    m_audioCount++;

                    // リストに追加
                    m_audioSourceList.Add(audioSource);
                }

                m_audioSourceList.TrimExcess();
            }
        }

        /// <summary>
        /// AudioSorceの初期化
        /// </summary> 
        /// <param name="audioSetting">オーディオ設定</param>
        /// <param name="channelNum">チャンネル数</param>
        public void InitializeAudioSetting(AudioSetting audioSetting, int channelNum)
        {
            if (audioSetting == null) { DebugManager.Error("AudioSetting = Null");  }

            // 設定された数以内だったら
            if(channelNum >= m_audioCount)
            {
                SetAudioSource(audioSetting.volume, audioSetting.mute);
            }
        }

        /// <summary>
        /// AudioSorceの初期設定
        /// </summary>
        /// <param name="volume">音量</param>
        /// <param name="mute">ミュート</param>
        /// <param name="onLoop">ループ</param>
        public void SetAudioSource(float volume, bool mute)
        {
            for(int i = 0; i < m_audioCount; ++i)
            {
                var audioSource = m_audioSourceList[i];

                // NULLチェック
                if (!audioSource.IsNull())
                {
                    audioSource.volume = volume;
                    audioSource.mute   = mute;
                }
            }
        }

        /// <summary>
        /// Audio再生
        /// </summary>
        /// <param name="index">再生する音の番号</param>
        /// <param name="channelNum">チャンネル数</param>
        /// <param name="onShot">再生方法</param>
        public void PlayAudio(int index, int channelNum, bool onShot = false)
        {
            // index < 設定されているAudioClip

            var listCount = m_audioClipList.Count;
            if (index < listCount)
            {
                AudioSource audioSource = m_audioSourceList[0];

                if(m_audioCount <= channelNum)
                {
                    // 空きを探す
                    for (int i = 0; i < m_audioCount; ++i)
                    {
                        // 使用されていなければ
                        if (m_audioSourceList[i].clip.IsNull())
                        {
                            // AudioSorceを設定
                            audioSource = m_audioSourceList[i];
                            break;
                        }
                    }
                }

                // 再生
                Play(audioSource, m_audioClipList[index], onShot);
            }
        }

        /// <summary>
        /// オーディオ再生
        /// </summary>
        /// <param name="audioSource">AudioSorce</param>
        /// <param name="audioClip">再生する音</param>
        /// <param name="oneShot">再生方法</param>
        /// <param name="volume">音量</param>
        private static void Play(AudioSource audioSource, AudioClip audioClip, bool oneShot)
        {
            // 停止確認
            audioSource.StopPlaying();

            audioSource.Play(audioClip, oneShot);
        }

        /// <summary>
        /// AudioSorceの再生の管理
        /// </summary>
        public void CheckAudioStop(bool force = false)
        {
            for (int i = 0; i < m_audioCount; ++i)
            {
                // 強制停止 ||再生していない
                if ( force  || !m_audioSourceList[i].isPlaying)
                {
                    m_audioSourceList[i].StopPlaying();
                }
            }
        }

        /// <summary>
        /// AudioClipの追加
        /// </summary>
        /// <param name="clip">追加するAudioClip</param>
        public void AddAudioClip(AudioClip clip)
        {
            m_audioClipList.Add(clip);
        }

        /// <summary>
        /// リストの初期化
        /// </summary>
        public void ClearList()
        {
            m_audioClipList.Clear();
        }

        #endregion
    }
}