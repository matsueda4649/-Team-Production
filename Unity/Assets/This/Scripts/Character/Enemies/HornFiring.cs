using System.Collections.Generic;
using ControllerSystem;
using DG.Tweening;
using ManagerSystem;
using UnityEngine;
using static ExtensionMathf;
using static ExtensionDOTween;

namespace CharacterSystem
{
    /// <summary>
    /// つの飛ばし
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class HornFiring : BaseEnemyController, IFixedUpdatable
    {
        #region 変数宣言

        /// <summary>
        /// データ
        /// </summary>
        [SerializeField] DOtweenMovingEnemyData m_data = default;

        /// <summary>
        /// 目標座標
        /// </summary>
        private float m_rectPositionX;

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
        private IList<Tween> m_tweenList = new List<Tween>(4);

        /// <summary>
        /// つの
        /// </summary>
        [SerializeField] RectTransform m_horn = default;

        /// <summary>
        /// つのの追加座標
        /// </summary>
        [SerializeField] Vector2 m_hornAddPosition = Vector2.zero;

        /// <summary>
        /// つのの初期位置
        /// </summary>
        private Vector2 m_initHornPosition;

        private RectTransform m_rect = null;

        #endregion

        #region メソッド

        protected override void Initialize()
        {
            this.SetComponent(ref m_rect);

            m_direction = m_data.GetMoveDirection;
            m_rectPositionX = GetMoveLength(m_rect, m_data.GetMoveLength, m_direction);

            m_initHornPosition = m_horn.localPosition;
        }

        public void FixedUpdateMe()
        {
            if (!GetSpriteRender.isVisible) { return; }
            if (Time.timeScale == 0f) { return; }

            // 移動
            if ( m_allowMove ) { MoveMotion(); }

            // 攻撃
            if ( m_allowAttack ) { FireHorns(); }
        }

        /// <summary>
        /// 移動方向の取得
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="length">移動距離</param>
        /// <param name="direction">移動方向</param>
        /// <returns>移動距離</returns>
        private static float GetMoveLength(RectTransform target, float length, Direction direction)
        {
            var start = target.localPosition.x;
            return (start + length * (int)direction);
        }

        /// <summary>
        /// 移動方向を反転する
        /// </summary>
        private static void ReverseDirection(ref Direction direction)
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

            // 移動時間
            var moveDuration = m_data.GetMoveLength.Divide();

            m_tweenList.Add(DOLocalMoveX(m_rect, m_rectPositionX, moveDuration, () =>
            {
                m_allowAttack = true;
            }));
        }

        /// <summary>
        /// つのを飛ばす
        /// </summary>
        private void FireHorns()
        {
            m_allowAttack = false;

            float duration = m_data.GetMoveLength.Divide();

            Vector2 start = m_horn.localPosition;
            Vector2 end = start + m_hornAddPosition;
            Vector3 rotation = new Vector3(0f, 0f, -CalculateRadian(start, end));

            m_horn.SetLocalRotation(rotation);
            m_tweenList.Add( m_horn.DOLocalMove(end, duration)
                  .OnComplete(() => {

                      // つのの移動後、必要なパラメータを初期化する
                      ResetParameter();

                  }));
        }

        /// <summary>
        /// 必要なパラメータの初期化
        /// </summary>
        private void ResetParameter()
        {
            m_horn.SetLocalRotation(Vector3.zero);
            m_horn.SetLocalPosition(m_initHornPosition);

            ReverseDirection(ref m_direction);
            m_rectPositionX = GetMoveLength(m_rect, m_data.GetMoveLength, m_direction);

            m_allowMove = true;
        }

        protected override void TriggerWithPlayer(BasePlayerController playerController)
        {
            base.TriggerWithPlayer(playerController);

            AudioManager.Instance.PlayVoice(AudioVoice.Stab);
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