using UnityEngine;

namespace ItemSystem 
{
    /// <summary>
    /// Singerのアイテム
    /// </summary>
    public class SoundWave : NormalBullet
    {
        #region 変数宣言

        [SerializeField] ColorData m_colorData = default;

        /// <summary>
        /// 初期カラー
        /// </summary>
        private Color32 m_initColor;

        /// <summary>
        /// アルファ値
        /// </summary>
        private float m_alpha = 0f;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            m_initColor = GetSpriteRenderer.color;
        }

        protected override void MovementBullet()
        {
            base.MovementBullet();
            GetSpriteRenderer.SetAlpha(m_initColor, m_alpha, m_colorData.GetLerpSpeed);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            var angle = MoveDirection == Vector2.left ? 180f : 0f;
            this.transform.SetLocalRotation(angle, Vector3.up);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            ResetParameter();
        }

        /// <summary>
        /// Parameterをリセットする
        /// </summary>
        private void ResetParameter()
        {
            if (GetSpriteRenderer.IsNull()) { return; }
            GetSpriteRenderer.color = m_initColor;
        }

        #endregion
    }
}