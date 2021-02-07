using ManagerSystem;
using UnityEngine;
using UnityEngine.UI;
using static DefNum;

namespace WindowSystem
{
    /// <summary>
    /// オーディオ設定
    /// </summary>
    public class AudioWindow : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// ミュート時のアイコン
        /// </summary>
        [SerializeField] Sprite m_muteIcon = default;

        /// <summary>
        /// ミュート解除時のアイコン
        /// </summary>
        [SerializeField] Sprite m_unMuteIcon = default;

        #region BGM

        /// <summary>
        /// BGMのスライダー
        /// </summary>
        [SerializeField] Slider m_bgmVolumeSlider = default;
        /// <summary>
        /// ミュートボタンの画像
        /// </summary>
        [SerializeField] Image m_bgmMuteIcon = default;
        /// <summary>
        /// BGMのミュート
        /// </summary>
        private bool m_bgmMute = false;
        /// <summary>
        /// BGMの音量
        /// </summary>
        private float m_bgmVolume = 0.0f;

        #endregion

        #region Voice

        /// <summary>
        /// Voiceのスライダー
        /// </summary>
        [SerializeField] Slider m_voiceVolumeSlider = default;
        /// <summary>
        /// ミュートボタンの画像
        /// </summary>
        [SerializeField] Image m_voiceMuteIcon = default;
        /// <summary>
        /// Voiceのミュート
        /// </summary>
        private bool m_voiceMute = false;
        /// <summary>
        /// Voiceの音量
        /// </summary>
        private float m_voiceVolume = 0.0f;

        #endregion

        #region SE
        /// <summary>
        /// SEのスライダー
        /// </summary>
        [SerializeField] Slider m_seVolumeSlider = default;
        /// <summary>
        /// ミュートボタンの画像
        /// </summary>
        [SerializeField] Image m_seMuteIcon = default;
        /// <summary>
        /// SEのミュート
        /// </summary>
        private bool m_seMute = false;
        /// <summary>
        /// SEの音量
        /// </summary>
        private float m_seVolume = 0.0f;

        private SaveData m_saveData;

        #endregion

        #endregion

        #region メソッド

        /// <summary>
        /// 初期設定
        /// </summary>
        public void Initialize()
        {
            SetVolumeSlider();

            m_saveData = SaveManager.Instance.GetSaveData;

            m_bgmVolumeSlider.value = m_saveData.bgmAudioSetting.volume;
            m_voiceVolumeSlider.value = m_saveData.voiceAudioSetting.volume;
            m_seVolumeSlider.value = m_saveData.seAudioSetting.volume;

            SetBgmAudioSource(m_saveData.bgmAudioSetting.volume, m_saveData.bgmAudioSetting.mute);
            SetVoiceAudioSource(m_saveData.voiceAudioSetting.volume, m_saveData.voiceAudioSetting.mute);
            SetSeAudioSource(m_saveData.seAudioSetting.volume, m_saveData.seAudioSetting.mute);
        }

        /// <summary>
        /// 音量スライダーの設定
        /// </summary>
        private void SetVolumeSlider()
        {
            // スライダーの最大値を設定
            m_bgmVolumeSlider.maxValue = MAX_VOLUME;
            m_voiceVolumeSlider.maxValue = MAX_VOLUME;
            m_seVolumeSlider.maxValue = MAX_VOLUME;

            // スライダーの設定
            m_bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
            m_voiceVolumeSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);
            m_seVolumeSlider.onValueChanged.AddListener(OnSeVolumeChanged);
        }

        /// <summary>
        /// BGMの音量設定
        /// </summary>
        /// <param name="volume">音量</param>
        public void OnBgmVolumeChanged(float volume)
        {
            // 強制的にミュート解除する
            SetBgmAudioSource(volume, false);
        }

        /// <summary>
        /// Voiceの音量設定
        /// </summary>
        /// <param name="volume">音量</param>
        public void OnVoiceVolumeChanged(float volume)
        {
            // 強制的にミュート解除する
            SetVoiceAudioSource(volume, false);
        }

        /// <summary>
        /// SEの音量設定
        /// </summary>
        /// <param name="volume">音量</param>
        public void OnSeVolumeChanged(float volume)
        {
            m_seVolume = volume;

            // 強制的にミュート解除する
            SetSeAudioSource(volume, false);
        }

        /// <summary>
        /// BGMの設定
        /// </summary>
        /// <param name="volume">音量</param>
        /// <param name="mute">ミュート</param>
        private void SetBgmAudioSource(float volume, bool mute)
        {
            m_bgmVolume = volume;
            m_bgmMute = mute;

            AudioManager.Instance.SetBgmAudioSource(volume, mute);

            m_bgmMuteIcon.sprite = null;
            m_bgmMuteIcon.sprite = m_bgmMute ? m_muteIcon : m_unMuteIcon;
        }

        /// <summary>
        /// Voiceの設定
        /// </summary>
        /// <param name="volume">音量</param>
        /// <param name="mute">ミュート</param>
        private void SetVoiceAudioSource(float volume, bool mute)
        {
            m_voiceVolume = volume;
            m_voiceMute = mute;

            AudioManager.Instance.SetVoiceAudioSource(volume, mute);

            m_voiceMuteIcon.sprite = null;
            m_voiceMuteIcon.sprite = m_voiceMute ? m_muteIcon : m_unMuteIcon;
        }

        /// <summary>
        /// Seの設定
        /// </summary>
        /// <param name="volume">音量</param>
        /// <param name="mute">ミュート</param>
        private void SetSeAudioSource(float volume, bool mute)
        {
            m_seVolume = volume;
            m_seMute = mute;

            AudioManager.Instance.SetSeAudioSource(volume, mute);

            m_seMuteIcon.sprite = null;
            m_seMuteIcon.sprite = m_seMute ? m_muteIcon : m_unMuteIcon;
        }

        /// <summary>
        /// ミュートボタンクリック時
        /// </summary>
        /// <param name="muteButtn"></param>
        public void OnClickMuteButton(RectTransform muteButtn)
        {
            // SEを再生
            AudioManager.Instance.PlaySe(AudioSe.Decision);

            switch (muteButtn.name) 
            {
                // Bgm
                case "BgmMuteButton":
                    SetBgmAudioSource(m_bgmVolume, !m_bgmMute);
                    break;

                // Voice
                case "VoiceMuteButton":
                    SetVoiceAudioSource(m_voiceVolume, !m_voiceMute);
                    break;

                // Se
                case "SeMuteButton":
                    SetSeAudioSource(m_seVolume, !m_seMute);
                    break;
            }
        }

        /// <summary>
        /// オーディオ設定時のVoiceの音量チェック
        /// </summary>
        public void OnClickCheckVoice()
        {
            // SEを再生
            AudioManager.Instance.PlayVoice(AudioVoice.Jump);
        }

        /// <summary>
        /// オーディオ設定時のSeの音量チェック
        /// </summary>
        public void OnClickCheckSe()
        {
            // SEを再生
            AudioManager.Instance.PlaySe(AudioSe.Decision);
        }

        /// <summary>
        /// 閉じるボタンクリック時
        /// </summary>
        public void OnClickClose()
        {
            UpdateNewAudioData();

            SaveManager.Save(m_saveData);

            AudioManager.Instance.InitializeAudioSetting();

            OptionManager.Instance.ReturnToTopWindow();
        }

        /// <summary>
        /// 新しいAudioのデータを更新する
        /// </summary>
        private void UpdateNewAudioData()
        {
            // 変更内容の保存
            // Audio

            m_saveData.bgmAudioSetting = new AudioSetting
            {
                volume = m_bgmVolume,
                mute = m_bgmMute,
                onLoop = true,
            };

            m_saveData.voiceAudioSetting = new AudioSetting
            {
                volume = m_voiceVolume,
                mute = m_voiceMute,
                onLoop = false,
            };

            m_saveData.seAudioSetting = new AudioSetting
            {
                volume = m_seVolume,
                mute = m_seMute,
                onLoop = false,
            };
        }

        #endregion
    }
}