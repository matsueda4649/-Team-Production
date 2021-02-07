using ControllerSystem;
using DG.Tweening;
using UnityEngine;
using static ExtensionDOTween;

namespace ItemSystem
{
    /// <summary>
    /// 減少するブロック
    /// </summary>
    public class DecreasingBlock : SpecialBlock
    {
        #region 変数宣言

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
            }
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);

            DeleteRoute();
        }

        /// <summary>
        /// 道をなくす
        /// </summary>
        public void DeleteRoute()
        {
            m_mainSequence = DOTween.Sequence();

            for (int i = 0; i < m_length; ++i)
            {
                m_mainSequence.Append(StartSequence(m_routeBlockArray[i]));
            }

            m_mainSequence.Play();
        }

        /// <summary>
        /// Block表示
        /// </summary>
        /// <param name="block">表示するブロック</param>
        private static Sequence StartSequence(Transform block)
        {
            var duration = 1f;

            return DOTween.Sequence().Append(DOScale(block, Vector3.zero, duration, null));
        }

        private void OnDisable()
        {
            m_mainSequence.Kill();
        }

        #endregion
    }
}