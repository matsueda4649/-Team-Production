using TMPro;
using UnityEngine;

namespace ManagerSystem
{
    /// <summary>
    /// 時間を管理
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// タイマー用のテキスト
        /// </summary>
        [SerializeField] TextMeshProUGUI m_timerText = default;

        /// <summary>
        /// 制限時間
        /// </summary>
        private float m_timeLimit;

        /// <summary>
        /// 経過時間
        /// </summary>
        public float ElapsedTime { get; private set; } = 0f;

        /// <summary>
        /// フェーズ
        /// </summary>
        private Phase m_phase = Phase.Init;

        /// <summary>
        /// 
        /// </summary>
        public System.Action onTimeOver;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="limitTime"> 制限時間 </param>
        public void Initialize(float limitTime)
        {
            m_timeLimit = limitTime;
            // 制限時間をテキストに設定
            m_timerText.text = m_timeLimit.ToString("F1");
        }

        /// <summary>
        /// フェーズの更新
        /// </summary>
        /// <param name="phase">設定するフェーズ</param>
        public void UpdatePhase(Phase phase)
        {
            m_phase = phase;
        }

        /// <summary>
        /// フェーズの更新
        /// </summary>
        public void UpdatePhase()
        {
            switch (m_phase)
            {
                // ゲームプレイ中なら
                case Phase.Play:
                    // 時間を計る
                    MeasureTime();
                    break;
            }
        }

        /// <summary>
        /// 時間を計る
        /// </summary>
        private void MeasureTime()
        {
            // 現在の時間を計る
            ElapsedTime += Time.deltaTime;

            // 残り時間を求める
            float remainingTime = m_timeLimit - ElapsedTime;
            remainingTime = Mathf.Clamp(remainingTime, 0f, m_timeLimit);

            // タイムオーバーしたら終了
            if(remainingTime <= 0 )  { onTimeOver?.Invoke(); }

            // テキストに表示する
            m_timerText.text = remainingTime.ToString("F1");
        }

        #endregion
    }
}