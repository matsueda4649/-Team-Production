using ControllerSystem;
using UnityEngine;

namespace ManagerSystem
{
    /// <summary>
    /// カメラの管理
    /// </summary>
    public class InGameCamera : MonoBehaviour
    {
        #region 変数宣言

        /// <summary>
        /// 画面の横幅
        /// </summary>
        [SerializeField] float m_width = 2560;

        /// <summary>
        /// 画面の縦幅
        /// </summary>
        [SerializeField] float m_height = 1440;

        /// <summary>
        /// TextureのPixel Par Unit
        /// </summary>
        [SerializeField] float m_pixelPerUnit = 100f;

        /// <summary>
        /// ターゲット
        /// </summary>
        [SerializeField] CameraTarget m_target = default;

        /// <summary>
        /// ラープ速度
        /// </summary>
        [SerializeField] float m_lerpSpeed = 5f;

        private Camera m_camera;

        #endregion

        #region メソッド

        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="player">プレイヤー</param>
        public void Initialize(BasePlayerController player)
        {
            m_target.Initialize(player);
            this.SetComponent(ref m_camera);
            SettingCamera.SettingCameraSize(ref m_camera, m_width, m_height, m_pixelPerUnit);
        }

        /// <summary>
        /// 通常のカメラ座標の更新
        /// </summary>
        public void LateUpdatePhase()
        {
            // ターゲットがいたら
            if (!m_target.IsNull())
            {
                m_target.LateUpdatePlayPosition();

                // ターゲットの
                var offset = m_target.transform.localPosition;
                var target = Vector3.Lerp(this.transform.localPosition, offset, m_lerpSpeed);

                this.transform.SetLocalPosition(target);
            }
        }

        #endregion
    }
}