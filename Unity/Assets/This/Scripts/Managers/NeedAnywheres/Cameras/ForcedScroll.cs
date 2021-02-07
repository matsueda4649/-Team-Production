using ControllerSystem;
using UnityEngine;

namespace ManagerSystem
{
    /// <summary>
    /// 強制スクロールさせる
    /// </summary>
    public class ForcedScroll : CameraTarget
    {
        /// <summary>
        /// 移動方向
        /// </summary>
        private enum MoveDirection
        {
            /// <summary>
            /// 縦
            /// </summary>
            Vertical,
            /// <summary>
            /// 横
            /// </summary>
            Horizontal,
        };

        #region 変数宣言

        /// <summary>
        /// 移動方向
        /// </summary>
        [SerializeField] MoveDirection m_direction = MoveDirection.Horizontal;

        /// <summary>
        /// 移動速度
        /// </summary>
        [SerializeField] float m_moveSpeed = default;

        #endregion


        #region メソッド

        public override void Initialize(BasePlayerController target)
        {
            base.Initialize(target);
            // カメラの座標
            this.transform.SetLocalPosition(Target.transform.localPosition + TargetOffSet);
        }

        /// <summary>
        /// 通常のカメラ座標更新
        /// </summary>
        public override void LateUpdatePlayPosition()
        {
            if (Target.IsNull()) { return; }

            // ターゲットの座標 + オフセット
            var localPosition = this.transform.localPosition;

            switch (m_direction)
            {
                case MoveDirection.Horizontal:
                    localPosition.y = Target.transform.localPosition.y + TargetOffSet.y;
                    localPosition.x += m_moveSpeed * Time.deltaTime;
                    break;
                case MoveDirection.Vertical:
                    localPosition.x = Target.transform.localPosition.x + TargetOffSet.x;
                    localPosition.y += m_moveSpeed * Time.deltaTime;
                    break;
            }

            float minPosY = MinPosition.y;
            float maxPoxY = MaxPosition.y;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            minPosY += 0.5f;
            maxPoxY += 0.5f;
#endif

            localPosition.x = Mathf.Clamp(localPosition.x, MinPosition.x, MaxPosition.x);
            localPosition.y = Mathf.Clamp(localPosition.y, minPosY, maxPoxY);

            // カメラの座標
            this.transform.SetLocalPosition(localPosition);
        }

        #endregion
    }
}