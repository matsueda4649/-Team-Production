using ControllerSystem;
using UnityEngine;

namespace ManagerSystem
{
    /// <summary>
    /// カメラターゲット
    /// </summary>
    public abstract class CameraTarget : MonoBehaviour
    {
        #region 変数宣言

        [SerializeField] Vector3 m_targetOffSet = default;

        [SerializeField] Vector2 m_minPosition = default;

        [SerializeField] Vector2 m_maxPosition = default;

        /// <summary>
        /// オフセットを取得
        /// </summary>
        public Vector3 TargetOffSet { get => m_targetOffSet; }

        /// <summary>
        /// カメラの座標制限 : 最小
        /// </summary>
        public Vector2 MinPosition { get => m_minPosition; }

        /// <summary>
        /// カメラの座標制限 : 最大
        /// </summary>                                    
        public Vector2 MaxPosition { get => m_maxPosition; }

        /// <summary>
        /// ターゲット
        /// </summary>
        public BasePlayerController Target { get; private set; }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="target"> キャラクター </param>
        public virtual void Initialize(BasePlayerController target)
        {
            Target = target;
        }

        /// <summary>
        /// カメラ座標更新
        /// </summary>
        public abstract void LateUpdatePlayPosition();

        #endregion
    }
}