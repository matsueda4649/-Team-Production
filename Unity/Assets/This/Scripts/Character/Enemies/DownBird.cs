using System;
using ControllerSystem;
using UnityEngine;

namespace CharacterSystem
{
    /// <summary>
    /// 下降Bird
    /// </summary>
    public class DownBird : BaseBird
    {
        #region 変数宣言

        /// <summary>
        /// 初期座標
        /// </summary>
        private Vector2 m_initPosition;

        /// <summary>
        /// 移動距離
        /// </summary>
        private Vector2 m_movingDistance;

        /// <summary>
        /// Y座標の移動距離
        /// </summary>
        private float MoveDistanceY
        {
            get
            {
                var addDistance = m_downSpeed * Time.deltaTime;
                return m_movingDistance.y + addDistance;
            }
        }

        /// <summary>
        /// 落下速度
        /// </summary>
        private float m_downSpeed;

        /// <summary>
        /// 下降判定
        /// </summary>
        private bool m_down = false;

        /// <summary>
        /// 経過時間
        /// </summary>
        private float m_elapsedTime = 0f;

        #endregion

        protected override void Initialize()
        {
            base.Initialize();

            m_initPosition = GetRb.position;

            var moveSpeed = Math.Abs(GetData.GetMoveSpeed);
            m_downSpeed = this.transform.localScale.x > Vector2.one.x ? -moveSpeed : moveSpeed;
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        protected override void MoveMotion()
        {
            // 移動距離の計算
            m_elapsedTime += Time.deltaTime;
            m_movingDistance.y = m_down ? MoveDistanceY : Mathf.Sin(m_elapsedTime);
            m_movingDistance.x += GetData.GetMoveSpeed * Time.deltaTime;
            var movingPosition = m_initPosition + m_movingDistance;

            GetRb.MovePosition(movingPosition);
            SetVelocityOfPlayer(PlayerController, distance: movingPosition - GetRb.position);
        }

        protected override void CollisionWithPlayer(BasePlayerController playerController)
        {
            // 落下開始
            m_down = true;
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            // 落下終了
            m_down = false;
            m_initPosition.y = GetRb.position.y;
            m_elapsedTime = 0f;
        }
    }
}