using System.Collections.Generic;
using ControllerSystem;
using UnityEngine;
using static DefTime;
using static ExtensionDOTween;
using static ExtensionEnum;
using static ExtensionMathf;

namespace CharacterSystem
{
    /// <summary>
    /// ？マーク
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Question : BaseEnemyController
    {
        /// <summary>
        /// アクションタイプ
        /// </summary>
        private enum ActionType
        {
            /// <summary>
            /// 転送
            /// </summary>
            Transfer,
            /// <summary>
            /// 回転
            /// </summary>
            Rotation,
        }

        #region 変数宣言

        /// <summary>
        /// 飛ばす座標のリスト
        /// </summary>
        [SerializeField] List<Transform> m_setPositionList = new List<Transform>(5);

        /// <summary>
        /// アクションタイプのフラグ
        /// </summary>
        private bool[] m_allowAction = new bool[GetLength<ActionType>()];

        /// <summary>
        /// 設定する色 
        /// 敵の種類を区別させる
        /// </summary>
        private readonly Color[] m_color = new Color[] { Color.yellow, Color.blue, };

        /// <summary>
        /// カメラ
        /// </summary>
        [SerializeField] RectTransform m_camera = default;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            // アクションタイプを決める
            var random = RandomInt(0, GetLength<ActionType>());
            for (int i = 0, length = m_allowAction.Length; i < length; ++i)
            {
                m_allowAction[i] = false;
                if (i == random)
                {
                    m_allowAction[i] = true;
                    GetSpriteRender.color = m_color[i];
                }
            }

            m_setPositionList.TrimExcess();
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            if (m_allowAction[(int)ActionType.Transfer]) { TransferPlayer(playerController); }

            else if (m_allowAction[(int)ActionType.Rotation]) { RotateCamera(); }
        }

        /// <summary>
        /// プレイヤーを転送する
        /// </summary>
        /// <param name="player">プレイヤー</param>
        private void TransferPlayer(BasePlayerController player)
        {
            Vector3 localPosition = Vector3.zero;

            // 座標が設定されていれば
            if(!m_setPositionList.IsEmpty())
            {
                int index = RandomInt(0, m_setPositionList.Count);
                if (!m_setPositionList[index].IsNull())
                {
                    localPosition = m_setPositionList[index].localPosition;
                }
            }

            player.transform.SetLocalPosition(localPosition);
        }

        /// <summary>
        /// カメラを回転させる
        /// </summary>
        private void RotateCamera()
        {
            Time.timeScale = STOP;
            var rotationValue   = new Vector3(0, 0, 180f) + m_camera.localEulerAngles;
            var moveDuration    = 1.5f;
            var rotaionDuration = 5f;
            GoDownRotation(m_camera, rotationValue, moveDuration, rotaionDuration, () => {
                Time.timeScale = PLAY;
            });
        }

        #endregion
    }
}