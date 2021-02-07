using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagerSystem
{
    public class SettingCamera : MonoBehaviour
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

        private Camera m_camera;

        #endregion

        #region メソッド    

        private void Awake()
        {
            this.SetComponent(ref m_camera);

            SettingCameraSize(ref m_camera, m_width, m_height, m_pixelPerUnit);
        }

        /// <summary>
        /// カメラのサイズの設定
        /// </summary>
        /// <param name="camera">カメラ</param>
        /// <param name="targetWidth">ターゲットWidth</param>
        /// <param name="targetHeight">ターゲットHeight</param>
        /// <param name="pixelPerUnit">1ユニットのピクセル数</param>
        public static void SettingCameraSize(ref Camera camera, float targetWidth, float targetHeight, float pixelPerUnit)
        {
            var screenHeight = (float)Screen.height;
            var screenWidth = (float)Screen.width;

            // 表示画面のアスペクト比
            var aspect = screenHeight / screenWidth;

            // 理想のアスペクト比
            var targetAspect = targetHeight / targetWidth;

            var orthographicSize = targetHeight * 0.5f / pixelPerUnit;

            // 画面が縦に広いとき
            if (targetAspect > aspect)
            {
                var widthScale = targetWidth / screenWidth;
                var cameraHeight = targetHeight / (screenHeight * widthScale);
                camera.rect = new Rect(0f, (1f - cameraHeight) * 0.5f, 1f, cameraHeight);
            }
            // 画面が横に広いとき
            else
            {
                var aspectScale = aspect / targetAspect;
                orthographicSize *= aspectScale;

                var heightScale = targetHeight / screenHeight;
                var cameraWidth = targetWidth / (screenWidth * heightScale);
                camera.rect = new Rect((1f - cameraWidth) * 0.5f, 0f, cameraWidth, 1f);
            }

            if (Screen.fullScreen)
            {
                Screen.SetResolution(
                    Screen.width,
                    Screen.height,
                    Screen.fullScreen);
            }

            camera.orthographicSize = orthographicSize;
        }

        #endregion
    }
}
