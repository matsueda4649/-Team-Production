using System;
using System.Collections.Generic;
using ControllerSystem;
using DG.Tweening;
using ManagerSystem;
using UnityEngine;
using static ExtensionDOTween;
using static ExtensionMathf;
using static UnityEngine.Mathf;

namespace CharacterSystem
{
    /// <summary>
    /// 鬼
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class Demon : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] DOtweenMovingEnemyData m_data = default;

        /// <summary>
        /// 武器を持つ手
        /// </summary>
        [SerializeField] RectTransform m_hand = default;

        /// <summary>
        /// 移動時間
        /// </summary>
        [SerializeField] float m_moveDuration = 5f;

        /// <summary>
        /// 目的地
        /// </summary>
        private float m_targetPositionX;

        /// <summary>
        /// 目標座標 Hand
        /// </summary>
        private Vector3 m_targetHandPos;

        /// <summary>
        /// 移動方向
        /// </summary>
        private Direction m_direction;

        /// <summary>
        /// 移動許可
        /// </summary>
        private bool m_allowMove = true;

        /// <summary>
        /// 攻撃許可
        /// </summary>
        private bool m_allowAttack = false;

        /// <summary>
        /// Tweenのリスト
        /// </summary>
        private IList<Tween> m_tweenList = new List<Tween>(8);

        private RectTransform m_rect = null;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_rect);

            m_direction = m_data.GetMoveDirection;

            // Handの初期値設定
            m_targetHandPos = GetInitPosition(m_hand, m_data.GetMoveDirection);
            m_hand.transform.SetLocalPosition(m_targetHandPos);

            // Handの次の移動方向を設定
            ReverseSign(ref m_targetHandPos.x);
            m_targetPositionX = GetMoveLength(m_rect, m_data.GetMoveLength, m_direction);
        }

        public void FixedUpdateMe()
        {
            // カメラに映っていなかったら
            if(!GetSpriteRender.isVisible) { return; }

            // 止まっていたら
            if(Time.timeScale == 0f) { return; }

            if (m_allowMove){ MoveMotion(); }
            else if (m_allowAttack){ AttackMotion(); }
        }

        /// <summary>
        /// Handの初期位置を取得する
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="direction">方向</param>
        /// <returns></returns>
        private static Vector3 GetInitPosition(RectTransform target, Direction direction)
        {
            var handPos = target.localPosition;
            handPos.x = Abs(handPos.x) * (int)direction;
            return handPos;
        }

        /// <summary>
        /// 移動距離を取得
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="length">移動距離</param>
        /// <param name="direction">移動方向</param>
        /// <returns></returns>
        private static float GetMoveLength(RectTransform target, float length, Direction direction)
        {
            var start = target.localPosition.x;
            return  start + length * (int)direction ;
        }

        /// <summary>
        /// 移動方向を設定する
        /// </summary>
        private static void SetDirection(ref Direction direction)
        {
            int dir = (int)direction;
            direction = (Direction)ReverseSign(ref dir);
        }

        /// <summary>
        /// 移動モーション
        /// </summary>
        private void MoveMotion()
        {
            m_allowMove = false;

            m_tweenList.Add(DOLocalMoveX(m_rect, m_targetPositionX, m_moveDuration, ()=> {
                m_allowAttack = true;
            }));
        }

        /// <summary>
        /// 攻撃モーション
        /// </summary>
        private void AttackMotion()
        {
            // 回転
            var rotateValue = new Vector3(0f, 0f, 90f * Sign(m_targetHandPos.x));

            // 移動時間
            float moveDuration = Ceil(Abs(m_targetHandPos.x));

            // 回転時間
            float rotateDuration = Sign(moveDuration).Divide();

            m_allowAttack = false;
            ShakeClubAnimation( rotateValue, rotateDuration, ()=> {

                // Handを初期位置に戻す
                m_tweenList.Add(DOLocalMoveX(m_hand, m_targetHandPos.x, moveDuration, () => {

                    ResetParameter();

                }));
            });
        }

        /// <summary>
        /// 必要なパラメータ―の初期化
        /// </summary>
        private void ResetParameter()
        {
            ReverseSign(ref m_targetHandPos.x);
            SetDirection(ref m_direction);
            m_targetPositionX = GetMoveLength(m_rect, m_data.GetMoveLength, m_direction);
            m_allowMove = true;
        }

        /// <summary>
        /// こん棒を振るアニメーション
        /// </summary>
        /// <param name="rotateValue"></param>
        /// <param name="rotateDuration"></param>
        /// <param name="callback"></param>
        private void ShakeClubAnimation(Vector3 rotateValue, float rotateDuration, Action callback)
        {
            // こん棒を振るアニメーション
            m_tweenList.Add(DOLocalRotate(m_hand, rotateValue, rotateDuration, () => {

                    AudioManager.Instance.PlayVoice(AudioVoice.Demon);

                    // こん棒を振り終わる
                    m_tweenList.Add(DOLocalRotate(m_hand, Vector3.zero, rotateDuration, () => {
                        callback?.Invoke();
                    }));
            }));
        }

        protected override void CollideWithPlayer(BasePlayerController playerController)
        {
            base.CollideWithPlayer(playerController);

            DeathMotion(playerController);
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);

            playerController.DeathMotion();
        }

        public void OnEnable()
        {
            FixedUpdateManager.AddUpdatable(this);
        }

        public void OnDisable()
        {
            FixedUpdateManager.RemoveUpdatable(this);

            Kill(m_tweenList);
        }

        #endregion
    }
}