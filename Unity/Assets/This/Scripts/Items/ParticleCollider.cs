using System;
using System.Collections;
using ControllerSystem;
using UnityEngine;

namespace ItemSystem
{
    /// <summary>
    /// ParticleCollider
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleCollider : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// Particle
        /// </summary>
        [SerializeField] ParticleSystem m_particle = default;

        /// <summary>
        /// パーティクルに当たったときのコールバック
        /// </summary>
        public Action CallBack { get; private set; }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="callback"></param>
        public void Initialize(Action callback)
        {
            m_particle.Stop();
            CallBack = callback;
        }

        private void OnEnable()
        {
            StartCoroutine(IDisableParticle());
        }

        /// <summary>
        /// Particleの非表示管理
        /// </summary>
        private IEnumerator IDisableParticle()
        {
            yield return new WaitWhile( ()=> m_particle.IsAlive(true) );
            this.gameObject.Deactovation();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.tag.Contains("PlayerController"))
            {
                // Default
                if (CallBack == null)
                {
                    var player = other.GetComponent<BasePlayerController>();
                    if (!player.IsNull()) { player.DeathMotion(); }
                }
                else
                {
                    CallBack();
                }
            }
        }

        #endregion
    }
}