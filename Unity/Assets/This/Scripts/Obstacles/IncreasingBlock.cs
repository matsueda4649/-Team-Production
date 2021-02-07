using ControllerSystem;
using DG.Tweening;
using UnityEngine;
using static ExtensionDOTween;

namespace ItemSystem
{
    /// <summary>
    /// 増えるブロック
    /// </summary>
    public class IncreasingBlock : SpecialBlock
    {
        /// <summary>
        /// ブロックの生成パターン(インデックス)
        /// </summary>
        private enum GenerationPattern
        {
            /// <summary>
            /// 昇順
            /// </summary>
            Up,
            /// <summary>
            /// 降順
            /// </summary>
            Down,
            /// <summary>
            /// ランダム
            /// </summary>
            Romdom,
        }

        #region 変数宣言

        /// <summary>
        /// ブロックの生成パターン
        /// </summary>
        [SerializeField] GenerationPattern m_generationPattern = GenerationPattern.Down;

        /// <summary>
        /// RouteBlockの親オブジェクト
        /// </summary>
        [SerializeField] Transform m_routeBlocksParent = default;

        /// <summary>
        /// RouteBlockの配列
        /// </summary>
        private Transform[] m_routeBlockArray = default;

        /// <summary>
        /// m_routeBlockArrayのLength
        /// </summary>
        private int m_length = 0;

        /// <summary>
        /// サイズの倍率
        /// </summary>
        [SerializeField] float m_magnification = 0.5f;

        private Sequence m_mainSequence;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            if (m_routeBlocksParent.IsNull()) { Destroy(this); }

            m_length = m_routeBlocksParent.childCount;

            Trigger.enabled = m_length > 0;

            m_routeBlockArray = new Transform[m_length];
            for ( int i = 0; i < m_length; ++i )
            {
                m_routeBlockArray[i] = m_routeBlocksParent.GetChild(i);
                m_routeBlockArray[i].SetLocalScale(Vector2.zero);
            }
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);

            CreateRoute();
        }

        /// <summary>
        /// 道を作成
        /// </summary>
        public void CreateRoute()
        {
            m_mainSequence = DOTween.Sequence();

            int index = 0;

            var romdom = new ExtensionMathf.RomdomIndexList();
            romdom.InitializeIndexList(0, m_length);

            for (int i = 0; i < m_length; ++i)
            {
                switch (m_generationPattern)
                {
                    case GenerationPattern.Up:
                        index = i;
                        break;
                    case GenerationPattern.Down:
                        index = m_length - i - 1;
                        break;
                    case GenerationPattern.Romdom:
                        index = romdom.GetRomdomIndex();
                        break;
                }

                m_mainSequence.Append(StartSequence(m_routeBlockArray[index], m_magnification));
            }

            m_mainSequence.Play();
        }

        /// <summary>
        /// Block表示
        /// </summary>
        /// <param name="block">表示するブロック</param>
        /// <param name="mag">サイズの倍率</param>
        private static Sequence StartSequence(Transform block, float mag)
        {
            var duration = 1f;

            return DOTween.Sequence().Append(DOScale(block, Vector3.one * mag, duration, null));
        }

        private void OnDisable()
        {
            m_mainSequence.Kill();
        }

        #endregion
    }
}