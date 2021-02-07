using System;
using System.Collections.Generic;
using ControllerSystem;
using UnityEngine;
using static ExtensionMathf;

namespace ItemSystem
{
    /// <summary>
    /// RightMinister用の弓矢
    /// </summary>
    public class ArcArrow : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// 初期値
        /// </summary>
        private Vector2 m_initPos;

        /// <summary>
        /// 開始座標
        /// </summary>
        private Vector2 m_startPos;

        /// <summary>
        /// 初期ローテーション
        /// </summary>
        private Quaternion m_initRot;

        /// <summary>
        /// AddForceされているかどうか
        /// </summary>
        private bool m_onForce = false;

        /// <summary>
        /// 当たったオブジェクト
        /// </summary>
        private static IList<GameObject> m_hitObjList = new List<GameObject>(5);

        /// <summary>
        /// Hit時のコールバック
        /// </summary>
        public Action OnHit { get; set; }

        private Rigidbody2D m_rb = null;

        private BoxCollider2D m_collider;

        /// <summary>
        /// 追加アングル
        /// </summary>
        [SerializeField] float m_addAngle = 90f;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            this.SetComponent(ref m_rb);
            this.SetComponent(ref m_collider);

            m_rb.SetBodyType(RigidbodyType2D.Static);
            m_collider.enabled = false;

            // 初期値設定
            m_initPos = this.transform.localPosition;
            m_initRot = this.transform.localRotation;
        }

        /// <summary>
        /// Anglesを返す
        /// </summary>
        private Quaternion GetLerpAngles()
        {
            var targetPos = this.transform.localPosition;

            // 回転値
            var x = 0f;
            var y = this.transform.localEulerAngles.y;
            var z = CalculateRadian(m_startPos, targetPos) + m_addAngle;

            var startRot = this.transform.localRotation;
            var targetRot = Quaternion.Euler(x, y, Mathf.Round(z));
            var lerpSpeed = 0.05f;

            return Quaternion.Slerp(startRot, targetRot, lerpSpeed);
        }

        public void FixedUpdateMe()
        {
            if (m_onForce)
            {
                m_startPos = this.transform.localPosition;
            }
        }

        /// <summary>
        /// 移動べクトルを与える
        /// </summary>
        /// <param name="velocity"></param>
        public void AddForce(Vector2 velocity)
        {
            m_rb.SetBodyType(RigidbodyType2D.Dynamic);
            m_collider.enabled = true;
            m_rb.AddForce(velocity * m_rb.mass, ForceMode2D.Impulse);
            m_onForce = true;
        }

        /// <summary>
        /// Rotationの設定
        /// </summary>
        public void SetRotation()
        {
            if (m_onForce)
            {
                this.transform.localRotation = GetLerpAngles();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Invoke("ResetParameter", time: 0.5f);

            if (!m_hitObjList.Contains(collision.gameObject))
            {
                m_hitObjList.Add(collision.gameObject);

                var playerController = collision.GetComponent<BasePlayerController>();
                if (!playerController.IsNull()) { playerController.DeathMotion(); }
            }
        }

        /// <summary>
        /// パラメータの初期化
        /// </summary>
        private void ResetParameter()
        {
            m_rb.SetBodyType(RigidbodyType2D.Static);
            m_collider.enabled = false;
            this.transform.localPosition = m_initPos;
            this.transform.localRotation = m_initRot;
            OnHit?.Invoke();
            m_onForce = false;
        }

        #endregion
    }
}