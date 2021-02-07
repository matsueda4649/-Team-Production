using UnityEngine;

/// <summary>
/// AudioSorceの拡張メソッド
/// </summary>
public static class ExtensionAudioSorce
{
    /// <summary>
    /// AudioClipが設定されていたら再生停止
    /// </summary>
    public static void StopPlaying(this AudioSource audioSource)
    {
        if (!audioSource.clip.IsNull())
        {
            // ノイズ対策
            var mute = audioSource.mute;
            audioSource.mute = true;
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.mute = mute;
        }
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="audioClip">再生する音</param>
    /// <param name="oneShot">oneShotかどうか</param>
    public static void Play(this AudioSource audioSource, AudioClip audioClip, bool oneShot = false)
    {
        audioSource.clip = audioClip;

        if (!oneShot) { audioSource.Play(); return; }

        audioSource.PlayOneShot(audioClip);
    }
}
