using ControllerSystem;
using ManagerSystem;
using UnityEngine;
using static ExtensionComponent;

namespace ItemSystem
{
    /// <summary>
    //  透明になるブロック
    /// </summary>
    public class TransparentBlock : SpecialBlock, IFixedUpdatable
    {
        #region 変数宣言

        private Material m_material;

        /// <summary>
        /// プレイヤーの座標
        /// </summary>
        private BasePlayerController m_playerController;

        /// <summary>
        /// 自身の座標
        /// </summary>
        private Vector3 m_thisPosition;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            m_material = GetRenderer.material;

            FindTagComponent(ref m_playerController, "PlayerController");

            m_thisPosition = this.transform.localPosition;
        }

        public void FixedUpdateMe()
        {
            if (GetRenderer.isVisible)
            {
                SetShaderParameter();
            }
        }

        /// <summary>
        /// Shaderのパラメータを設定
        /// </summary>
        private void SetShaderParameter()
        {
            var distance = (m_playerController.transform.localPosition - m_thisPosition).magnitude;
            m_material.SetFloat("_Distance", distance);
        }

        public void OnEnable()
        {
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            FixedUpdateManager.RemoveUpdatable(this);
        }

        #endregion
    }
}