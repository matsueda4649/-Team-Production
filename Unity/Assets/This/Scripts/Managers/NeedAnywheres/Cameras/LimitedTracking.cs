using UnityEngine;

namespace ManagerSystem
{
    /// <summary>
    /// 制限ありカメラターゲット
    /// </summary>
    public class LimitedTracking : CameraTarget
    {
        #region メソッド

        /// <summary>
        /// 通常のカメラ座標更新
        /// </summary>
        public override void LateUpdatePlayPosition()
        {
            if (!Target.IsNull())
            {
                // ターゲットの座標 + オフセット
                var localPosition = Target.transform.localPosition + TargetOffSet;

                // 座標の範囲を制限
                localPosition.x = Mathf.Clamp(localPosition.x, MinPosition.x, MaxPosition.x);

                float minPosY = MinPosition.y;
                float maxPoxY = MaxPosition.y;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
                //minPosY += 0.5f;
                //maxPoxY += 0.5f;
#endif

                localPosition.x = Mathf.Clamp(localPosition.x, MinPosition.x, MaxPosition.x);
                localPosition.y = Mathf.Clamp(localPosition.y, minPosY, maxPoxY);

                // カメラの座標
                this.transform.SetLocalPosition(localPosition);
            }
        }

        #endregion
    }
}