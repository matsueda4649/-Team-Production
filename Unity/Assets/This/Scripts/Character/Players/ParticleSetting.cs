using UnityEngine;
using ControllerSystem;
using ManagerSystem;

namespace CharacterSystem 
{
    /// <summary>
    /// PlayerのParticleの設定
    /// </summary>
    public class ParticleSetting : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// 使用するダッシュパーティクル
        /// </summary>
        [SerializeField] GameObject m_dashParticle = default;

        /// <summary>
        /// 使用するParticle
        /// </summary>
        private ParticleSystem m_particle;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            if (m_dashParticle.IsNull()) { return; }
            m_dashParticle = Instantiate(m_dashParticle, this.transform);
            m_particle = SetParticle(m_dashParticle.transform);
        }

        /// <summary>
        /// パーティクルの再生設定
        /// </summary>
        /// <param name="isPlaying">再生するかどうか</param>
        public void SetParticle(bool isPlaying)
        {
            this.enabled = isPlaying;
        }

        /// <summary>
        /// パーティクルの設定
        /// </summary>
        /// <param name="parent"></param>
        private ParticleSystem SetParticle(Transform parent)
        {
            for (int i = 0, count = parent.childCount; i < count; ++i)
            {
                var child = parent.GetChild(i);
                var component = child.GetComponent<ParticleSystem>();
                if (!component.IsNull()) { return component; }

                SetParticle(child);
            }

            return null;
        }

        public void OnEnable()
        {
            if (m_particle.IsNull()) { return; }
            m_particle.Play();
        }

        public void OnDisable()
        {
            if (m_particle.IsNull()) { return; }
            m_particle.Stop();
        }

        #endregion
    }
}